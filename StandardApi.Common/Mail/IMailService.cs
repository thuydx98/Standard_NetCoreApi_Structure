using System.Threading.Tasks;

namespace StandardApi.Common.Mail
{
    public interface IMailService
    {
        Task SendAsync(MailMessage mailMessage, bool keepReceiver = false);
    }
}
