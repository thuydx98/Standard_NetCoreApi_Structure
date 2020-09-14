using StandardApi.CrossCutting.Commands;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Commands.ForgotPassword
{
    public interface IForgotPasswordCommand
    {
        Task<CommandResult> ExecuteAsync(string username);
    }
}