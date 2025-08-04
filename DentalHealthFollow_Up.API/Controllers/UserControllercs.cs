using DentalHealthFollow_Up.Shared.DTOs;
using DentalHealthFollow_Up.API.Services;
using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFollow_Up.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Bu e-posta zaten kayıtlı.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = SecurityHelper.Encrypt(dto.Password),
                birthdate = dto.BirthDate
            };
       
            var encryptionService = new EncryptionService();
            user.Password = encryptionService.Encrypt(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayıt başarılı.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı.");
            var encryptionService = new EncryptionService();
            string decryptedPassword = encryptionService.Decrypt(user.Password);

           
            if (decryptedPassword != dto.Password)
            {
                return BadRequest("Parola hatalı.");
            }

            return Ok("Giriş başarılı.");
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword([FromQuery] string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok("Mail adresi kayıtlı.");
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(PasswordResetDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var encryptionService = new EncryptionService();
            user.Password = encryptionService.Encrypt(dto.NewPassword);

            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok("Parola başarıyla güncellendi.");
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                BirthDate = user.birthdate
            };

            return Ok(userDto);
        }

        [HttpPut("password-reset/{id}")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Parola boş olamaz");


            user.Password = SecurityHelper.Encrypt(dto.Password);


            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

