using StandardApi.Core.Auth.ViewModels;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetInfoFromToken
{
    public interface IGetInfoFromTokenQuery
    {
        Task<UserTokenInfoModel> ExecuteAsync(string accessToken);
    }
}
