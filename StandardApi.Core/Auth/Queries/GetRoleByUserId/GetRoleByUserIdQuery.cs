using Microsoft.EntityFrameworkCore;
using StandardApi.Core.Auth.ViewModels;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetRoleByUserId
{
    public class GetRoleByUserIdQuery : IGetRoleByUserIdQuery
    {
        private readonly IRepository<UserEntity> _userRepository;

        public GetRoleByUserIdQuery(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserRoleViewModel>> ExecuteAsync(int userId)
        {
            var result = await _userRepository.TableNoTracking
                .Where(n => n.Id == userId)
                .Select(n => new UserRoleViewModel()
                {
                    RoleId = n.Role.Id,
                    RoleName = n.Role.Role,
                    RoleTitle = n.Role.Title
                })
                .ToListAsync();

            return new List<UserRoleViewModel>();
        }
    }
}
