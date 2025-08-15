using DentalHealthFollow_Up.Shared.DTOs;
using DentalHealthFollow_Up.API.Services;
using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFollow_Up.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EncryptionService _encryption;
        private readonly MailService _mail; 

        public UserController(AppDbContext context, EncryptionService encryption, MailService mail)
        {
            _context = context;
            _encryption = encryption;
            _mail = mail;
        }

        // 1) KAYIT
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email);
            if (exists) return BadRequest("Bu e-posta ile kayıt var.");

            var encryptedPassword = _encryption.Encrypt(dto.Password); 

            var user = new User
            {
                FullName = dto.Name.Trim(),
                Email = email,
                Password = encryptedPassword,
                BirthDate = dto.BirthDate
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kayıt başarılı." });
        }

        // 2) GİRİŞ
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("E-posta ve şifre gerekli.");

            var email = dto.Email.Trim().ToLowerInvariant();

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (userEntity is null)
                return BadRequest("Kullanıcı bulunamadı.");

           
            string? decrypted = null;
            bool decryptedOk = false;

            try
            {
                
                decrypted = _encryption.Decrypt(userEntity.Password);
                decryptedOk = string.Equals(decrypted, dto.Password, StringComparison.Ordinal);
            }
            catch
            {
                decryptedOk = string.Equals(userEntity.Password, dto.Password, StringComparison.Ordinal);
            }

            if (!decryptedOk)
                return BadRequest("Parola hatalı.");

           
            var user = new UserDto
            {
                UserId = userEntity.UserId,
                Name = userEntity.FullName,
                Email = userEntity.Email,
                BirthDate = userEntity.BirthDate
            };

            return Ok(user);
        }


        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var code = Random.Shared.Next(100000, 999999).ToString();

            _context.PasswordResets.Add(new PasswordReset
            {
                UserId = user.UserId,
                Token = _encryption.Encrypt(code),  
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)  
            });
            await _context.SaveChangesAsync();

            await _mail.SendAsync(user.Email, "Parola Sıfırlama Kodu",
                $"<p>Kodun: <b>{code}</b> (15 dk geçerli)</p>");

            return Ok("Sıfırlama kodu mail adresine gönderildi.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var pr = await _context.PasswordResets
                .Where(x => x.UserId == user.UserId)
                .OrderByDescending(x => x.PasswordResetId)                
                .FirstOrDefaultAsync();

            if (pr == null) return BadRequest("Geçerli bir sıfırlama isteği bulunamadı.");
            if (pr.ExpiresAt < DateTime.UtcNow)             
                return BadRequest("Sıfırlama kodunun süresi dolmuş.");

            var codePlain = DecryptSafe(pr.Token);          
            if (!string.Equals(codePlain, dto.Code))
                return BadRequest("Sıfırlama kodu hatalı.");

            user.Password = _encryption.Encrypt(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("Parola başarıyla güncellendi.");
        }


        // 5) E-posta ile kullanıcı getir 
        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.FullName,
                BirthDate = user.BirthDate
            };
            return Ok(userDto);
        }

       
        private string DecryptSafe(string s)
        {
            try { return _encryption.Decrypt(s); }
            catch { return s; } 
        }

        private static int RandomNumber(int min, int max)
            => Random.Shared.Next(min, max + 1);
    }
}
