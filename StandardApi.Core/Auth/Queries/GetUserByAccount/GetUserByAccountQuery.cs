using Microsoft.EntityFrameworkCore;
using StandardApi.Business.Auth.ViewModels;
using StandardApi.Common.Securities;
using StandardApi.Constants.Message;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetUserByAccount
{
    public class GetUserByAccountQuery : IGetUserByAccountQuery
    {
        private readonly IRepository<UserEntity> _userRepository;

        public GetUserByAccountQuery(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserLoginViewModel> ExecuteAsync(string username, string inputPassword)
        {
            username = username.Trim().Replace(" ", "");

            UserLoginViewModel user = await _userRepository.TableNoTracking
                .Where(n =>
                n.Username == username &&
                n.Deleted != true)
                .Select(n => new UserLoginViewModel()
                {
                    Id = n.Id,
                    UserCode = n.UserCode,
                    FirstName = n.FirstName,
                    LastName = n.LastName,
                    MiddleName = n.MiddleName,
                    UserName = n.Username,
                    PasswordHash = n.Password,
                    CreatedAt = n.CreatedAt,
                    //Enabled = n.Status.IsBlockAccess == true ? false : true,
                    //Reason = MessageConstant.REASON_CANNOT_LOGIN + n.Status.Status.ToLower()
                })
                .SingleOrDefaultAsync();

            if (user == null || !BCrypt.CheckPassword(inputPassword, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}
