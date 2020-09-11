using StandardApi.Business.Auth.ViewModels;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetUserByAccount
{
    public interface IGetUserByAccountQuery
    {
        Task<UserLoginViewModel> ExecuteAsync(string username, string inputPassword);
    }
}