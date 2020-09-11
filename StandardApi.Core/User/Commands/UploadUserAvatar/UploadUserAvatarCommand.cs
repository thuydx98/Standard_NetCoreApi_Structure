using Microsoft.AspNetCore.Http;
using StandardApi.CrossCutting.Commands;
using System.Threading.Tasks;

namespace StandardApi.Core.User.Commands.UploadUserAvatar
{
    public class UploadUserAvatarCommand : IUploadUserAvatarCommand
    {
        public Task<CommandResult> ExecuteAsync(IFormFile file, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
