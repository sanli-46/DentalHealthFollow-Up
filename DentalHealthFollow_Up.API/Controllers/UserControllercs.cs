using System.Security.Cryptography;
using System.Text;
using DentalHealthFollow_Up.API.Services;
using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.Shared.DTOs;
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

        // --- KAYIT ---
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (dto is null) return BadRequest("Geçersiz istek.");

            var emailNorm = dto.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(emailNorm) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("E-posta ve şifre gerekli.");

            // Şifre politikası (hızlı)
            static bool Strong(string p) =>
                p.Length >= 8 && p.Any(char.IsUpper) && p.Any(char.IsLower) && p.Any(char.IsDigit);
            if (!Strong(dto.Password))
                return BadRequest("Şifre en az 8 karakter, büyük-küçük harf ve rakam içermeli.");

            var exists = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == emailNorm);
            if (exists) return BadRequest("Bu e-posta ile kayıt var.");

            var user = new User
            {
                FullName = dto.Name?.Trim() ?? dto.Name?.Trim(),
                Email = emailNorm,
                Password = "v1:" + _encryption.Encrypt(dto.Password),
                BirthDate = dto.BirthDate
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Mail gönderimi akışı bozmasın
            _ = _mail.SendRegisterInfoAsync(user.Email, user.FullName);

            return Ok(new { message = "Kayıt başarılı." });
        }

        // --- GİRİŞ ---
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (dto is null) return BadRequest("Geçersiz istek.");

            var emailNorm = dto.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(emailNorm) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("E-posta ve şifre gerekli.");

            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailNorm);
            if (userEntity is null) return BadRequest("Kullanıcı bulunamadı.");

            bool ok;
            if (userEntity.Password.StartsWith("v1:")) // yeni format
            {
                var plain = _encryption.Decrypt(userEntity.Password["v1:".Length..]);
                ok = string.Equals(plain, dto.Password, StringComparison.Ordinal);
            }
            else // eski düz metin kayıtlar için tek seferlik migrasyon
            {
                ok = string.Equals(userEntity.Password, dto.Password, StringComparison.Ordinal);
                if (ok)
                {
                    userEntity.Password = "v1:" + _encryption.Encrypt(dto.Password);
                    await _context.SaveChangesAsync();
                }
            }

            if (!ok) return BadRequest("Parola hatalı.");

            var user = new UserDto
            {
                UserId = userEntity.UserId,
                Name = userEntity.FullName,
                Email = userEntity.Email,
                BirthDate = userEntity.BirthDate
            };

            return Ok(user);
        }

        // --- (OPSİYONEL) PAROLA SIFIRLAMA: sadece e-posta gönderir, DB'ye token kaydetmez ---
        // İstersen tamamen silebilirsin; demo için yeterlidir ve compile hatası çıkarmaz.
        [HttpPost("password/forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotDto body)
        {
            var emailNorm = body.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(emailNorm)) return Ok(); // enumeration engelle

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == emailNorm);
            if (user is null) return Ok();

            // Sadece mail gönder (DB'de PasswordReset yoksa IsUsed vs. hatası çıkmasın)
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var resetLink = $"https://localhost:7250/password-reset?token={Uri.EscapeDataString(token)}";

            await _mail.SendPasswordResetAsync(user.Email, user.FullName, resetLink);
            return Ok();
        }

        public sealed class ForgotDto
        {
            public string Email { get; set; } = "";
        }
    }
}
