using StandardApi.Core.Auth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetRoleByUserId
{
    public interface IGetRoleByUserIdQuery
    {
        Task<List<UserRoleViewModel>> ExecuteAsync(int userId);
    }
}
