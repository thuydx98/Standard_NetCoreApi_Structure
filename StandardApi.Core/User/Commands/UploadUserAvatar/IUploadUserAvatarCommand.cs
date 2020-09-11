using Microsoft.AspNetCore.Http;
using StandardApi.CrossCutting.Commands;
using System.Threading.Tasks;

namespace StandardApi.Core.User.Commands.UploadUserAvatar
{
    public interface IUploadUserAvatarCommand
    {
        Task<CommandResult> ExecuteAsync(IFormFile file, int userId);
    }
}