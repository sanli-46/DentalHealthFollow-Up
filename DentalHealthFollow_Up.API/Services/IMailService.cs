using System.Threading.Tasks;

namespace DentalHealthFollow_Up.API.Services
{
    public interface IMailService
    {
        Task SendAsync(string to, string subject, string body, bool isHtml = true);
    }
}
