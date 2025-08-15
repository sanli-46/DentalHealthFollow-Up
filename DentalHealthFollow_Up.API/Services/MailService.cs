using System.Net;
using System.Net.Mail;
using System.Text;
using DentalHealthFollow_Up.API.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DentalHealthFollow_Up.API.Services
{
    public class MailService
    {
        private readonly SmtpOptions _opt;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<SmtpOptions> opt, ILogger<MailService> logger)
        {
            _opt = opt.Value;
            _logger = logger;
        }

        private SmtpClient CreateClient()
        {
            var client = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_opt.User, _opt.Password),
                Timeout = 15000
            };
            return client;
        }

        private MailMessage Build(string to, string subject, string htmlBody)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(_opt.FromAddress, _opt.FromName, Encoding.UTF8),
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = htmlBody,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                HeadersEncoding = Encoding.UTF8
            };
            msg.To.Add(to);
            msg.Priority = MailPriority.Normal;
            return msg;
        }

        public async Task SendRegisterInfoAsync(string to, string fullName)
        {
            var subject = "Kayıt Başarılı - Ağız ve Diş Sağlığı Uygulaması";
            var html = $@"
<html><body style=""font-family:Segoe UI,Arial,sans-serif"">
  <h2>Merhaba {System.Net.WebUtility.HtmlEncode(fullName)},</h2>
  <p>Uygulamamıza kaydınız başarıyla oluşturuldu.</p>
  <p>Artık hedeflerinizi belirleyebilir, durum kayıtlarınızı ekleyebilirsiniz.</p>
  <hr style=""border:0;border-top:1px solid #ddd"" />
  <p style=""font-size:12px;color:#666"">Bu e-posta bilgilendirme amaçlıdır. Yanıtlamayın.</p>
</body></html>";

            try
            {
                using var client = CreateClient();
                using var msg = Build(to, subject, html);
                await client.SendMailAsync(msg);
                _logger.LogInformation("Register mail sent to {To}", to);
            }
            catch (SmtpException ex)
            {
                // Burada loglayıp tekrar fırlatmak yerine loglayıp YUTUYORUZ ki register akışı bozulmasın
                _logger.LogWarning(ex, "Register mail failed for {To}", to);
            }
        }

        public async Task SendPasswordResetAsync(string to, string fullName, string resetLink)
        {
            var subject = "Parola Sıfırlama Talebi";
            var safeLink = System.Net.WebUtility.HtmlEncode(resetLink);
            var html = $@"
<html><body style=""font-family:Segoe UI,Arial,sans-serif"">
  <h2>Merhaba {System.Net.WebUtility.HtmlEncode(fullName)},</h2>
  <p>Parolanızı sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
  <p><a href=""{safeLink}"">{safeLink}</a></p>
  <p>Bu isteği siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>
  <hr style=""border:0;border-top:1px solid #ddd"" />
  <p style=""font-size:12px;color:#666"">Bağlantı güvenliğiniz için süreli olabilir.</p>
</body></html>";

            try
            {
                using var client = CreateClient();
                using var msg = Build(to, subject, html);
                await client.SendMailAsync(msg);
                _logger.LogInformation("Reset mail sent to {To}", to);
            }
            catch (SmtpException ex)
            {
                _logger.LogWarning(ex, "Reset mail failed for {To}", to);
                
            }
        }
    }
}

