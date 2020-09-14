using System.Threading.Tasks;

namespace StandardApi.BackgroundJob.Jobs.Mails.SendRecoveryCode
{
    public interface ISendRecoveryCode
    {
        Task ExecuteAsync(string email, string recoveryCode);
    }
}