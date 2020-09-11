using StandardApi.Core.Auth.ViewModels;
using StandardApi.CrossCutting.Commands;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Commands.ChangePassword
{
    public interface IChangePasswordCommand
    {
        Task<CommandResult> ExecuteAsync(ChangePasswordViewModel model);
    }
}