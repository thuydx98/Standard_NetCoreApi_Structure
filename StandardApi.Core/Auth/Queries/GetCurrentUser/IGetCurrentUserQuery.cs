using StandardApi.Core.Auth.ViewModels;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetCurrentUser
{
    public interface IGetCurrentUserQuery
    {
        int UserId { get; }

        Task<UserTokenInfoModel> ExecuteAsync();
    }
}