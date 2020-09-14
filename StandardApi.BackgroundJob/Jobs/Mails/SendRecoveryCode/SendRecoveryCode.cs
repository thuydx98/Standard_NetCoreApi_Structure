using StandardApi.Common.Mail;
using System;
using System.Threading.Tasks;

namespace StandardApi.BackgroundJob.Jobs.Mails.SendRecoveryCode
{
    public class SendRecoveryCode : ISendRecoveryCode
    {
        private readonly Lazy<IMailService> _lazyMailService;

        public SendRecoveryCode(Lazy<IMailService> lazyMailService)
        {
            _lazyMailService = lazyMailService;
        }

        public async Task ExecuteAsync(string email, string recoveryCode)
        {
            MailMessage recoveryMail = new MailMessage()
            {
                To = email,
                Subject = "Recovery Code",
                HtmlMessage = "Your Revocery Code is: " + recoveryCode
            };

            await _lazyMailService.Value.SendAsync(recoveryMail);
        }
    }
}
