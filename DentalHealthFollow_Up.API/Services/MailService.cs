using System.Net;
using System.Net.Mail;
using DentalHealthFollow_Up.API.Options;
using Microsoft.Extensions.Options;

namespace DentalHealthFollow_Up.API.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpOptions _opt;
        public MailService(IOptions<SmtpOptions> options) => _opt = options.Value;

        public async Task SendAsync(string to, string subject, string body, bool isHtml = true)
        {
            using var client = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                Credentials = new NetworkCredential(_opt.UserName, _opt.Password)
            };
            var msg = new MailMessage
            {
                From = new MailAddress(_opt.From, _opt.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            msg.To.Add(to);
            await client.SendMailAsync(msg);
        }
    }
}
