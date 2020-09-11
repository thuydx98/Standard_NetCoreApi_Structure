using StandardApi.Core.User.ViewModels;
using System.Threading.Tasks;

namespace StandardApi.Core.User.Queries.GetUserAvatar
{
    public interface IGetUserAvatarQuery
    {
        Task<UserAvatarViewModel> ExecuteAsync(int userId);
        Task<UserAvatarViewModel> ExecuteAndSaveCacheAsync(int userId);
    }
}