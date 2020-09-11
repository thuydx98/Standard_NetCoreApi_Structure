using Microsoft.EntityFrameworkCore;
using StandardApi.Core.Auth.ViewModels;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetInfoFromToken
{
    public class GetInfoFromTokenQuery : IGetInfoFromTokenQuery
    {
        private readonly IRepository<UserEntity> _userRepository;

        public GetInfoFromTokenQuery(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserTokenInfoModel> ExecuteAsync(string accessToken)
        {
            accessToken = accessToken?.Replace("Bearer", "").Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var userId = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var user = await _userRepository.TableNoTracking
                .Where(n => n.Id.ToString() == userId)
                .Select(n => new UserTokenInfoModel()
                {
                    Id = n.Id,
                    FirstName = n.FirstName,
                    MiddleName = n.MiddleName,
                    LastName = n.LastName,
                    Birthday = n.Birthday,
                    Email = n.Email,
                    PhoneNumber = n.PhoneNumber,
                    Gender = n.Gender,
                    CreatedDate = n.CreatedAt
                })
                .SingleOrDefaultAsync();

            return user;
        }
    }
}
