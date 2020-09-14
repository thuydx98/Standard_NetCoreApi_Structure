using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StandardApi.Auth.ViewModels;
using StandardApi.Business.Auth.ViewModels;
using StandardApi.Constants;
using StandardApi.Constants.Message;
using StandardApi.Core.Auth.Commands.ChangePassword;
using StandardApi.Core.Auth.Commands.ForgotPassword;
using StandardApi.Core.Auth.Queries.GetInfoFromToken;
using StandardApi.Core.Auth.Queries.GetUserByAccount;
using StandardApi.Core.Auth.ViewModels;
using StandardApi.CrossCutting.Commands;
using StandardApi.Framework.Authentication;
using StandardApi.Framework.Configuration.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StandardApi.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptionsSnapshot<JwtOptions> _jwtConfiguration;
        private readonly IGetUserByAccountQuery _getUserByAccountQuery;
        private readonly IGetInfoFromTokenQuery _getInfoFromTokenQuery;
        private readonly IChangePasswordCommand _changePasswordCommand;
        private readonly IForgotPasswordCommand _forgotPasswordCommand;

        public AuthController(
            IOptionsSnapshot<JwtOptions> jwtConfiguration,
            IGetUserByAccountQuery getUserByAccountQuery,
            IGetInfoFromTokenQuery getInfoFromTokenQuery,
            IChangePasswordCommand changePasswordCommand,
            IForgotPasswordCommand forgotPasswordCommand)
        {
            _jwtConfiguration = jwtConfiguration;
            _getUserByAccountQuery = getUserByAccountQuery;
            _getInfoFromTokenQuery = getInfoFromTokenQuery;
            _changePasswordCommand = changePasswordCommand;
            _forgotPasswordCommand = forgotPasswordCommand;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            UserLoginViewModel user = await _getUserByAccountQuery.ExecuteAsync(loginViewModel.Username, loginViewModel.Password);

            if (user != null)
            {
                if (!user.Enabled)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new { message = user.Reason });
                }

                var jwtOptions = _jwtConfiguration.Value;
                jwtOptions.ValidFor = loginViewModel.RememberMe ? TimeSpan.FromHours(JwtValidTime.HAS_REMEMBER_ME_BY_HOURS)
                    : TimeSpan.FromHours(JwtValidTime.NOT_HAVE_REMEMBER_ME_BY_HOURS);

                var jwtUserAccount = new JwtUserAccount
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserCode = user.UserCode,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    FirstName = user.FirstName
                };

                return new ObjectResult(new { accessToken = jwtUserAccount.GenerateToken(jwtOptions) });
            }

            return StatusCode((int)HttpStatusCode.Unauthorized, new { message = Message.WRONG_USERNAME_PASSWORD });
        }

        #region un-use
        //[HttpPost]
        //[Route("customer/login")]
        //[AllowAnonymous]
        //public async Task<ActionResult> CustomerLoginAsync(LoginViewModel loginViewModel)
        //{
        //    UserLoginViewModel user = await _getCustomerByAccountQuery.ExecuteAsync(loginViewModel.Username, loginViewModel.Password);

        //    if (user != null)
        //    {
        //        if (!user.Enabled)
        //        {
        //            return StatusCode((int)HttpStatusCode.Forbidden, new { message = user.Reason });
        //        }

        //        var jwtOptions = _jwtConfiguration.Value;
        //        jwtOptions.ValidFor = TimeSpan.FromHours(JwtValidTime.FOR_CUSTOMER_BY_HOURS);

        //        var jwtUserAccount = new JwtUserAccount
        //        {
        //            UserId = user.Id,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            LastName = user.LastName,
        //            MiddleName = user.MiddleName,
        //            FirstName = user.FirstName
        //        };

        //        TokenViewModel tokenVM = new TokenViewModel()
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            LastName = user.LastName,
        //            MiddleName = user.MiddleName,
        //            FirstName = user.FirstName,
        //            Birthday = user.Birthday,
        //            Gender = user.Gender,
        //            CreatedDate = user.CreatedDate,
        //            AccessToken = jwtUserAccount.GenerateToken(jwtOptions)
        //        };

        //        return new ObjectResult(tokenVM);
        //    }

        //    return StatusCode((int)HttpStatusCode.Unauthorized, new { message = MessageConstant.WRONG_USERNAME_PASSWORD });
        //}

        //[HttpPost]
        //[Route("customer/register")]
        //[AllowAnonymous]
        //public async Task<ActionResult> RegisterAsync([FromBody] SaveUserViewModel model)
        //{
        //    var result = await _saveCustomerCommand.ExecuteAsync(model);

        //    if (!result.Succeeded)
        //    {
        //        return new ObjectResult(result);
        //    }

        //    CustomerViewModel user = result.Data as CustomerViewModel;

        //    if (user != null)
        //    {
        //        var jwtOptions = _jwtConfiguration.Value;
        //        jwtOptions.ValidFor = TimeSpan.FromHours(JwtValidTime.FOR_CUSTOMER_BY_HOURS);

        //        var jwtUserAccount = new JwtUserAccount
        //        {
        //            UserId = user.Id,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            LastName = user.LastName,
        //            MiddleName = user.MiddleName,
        //            FirstName = user.FirstName
        //        };

        //        TokenViewModel tokenVM = new TokenViewModel()
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            LastName = user.LastName,
        //            MiddleName = user.MiddleName,
        //            FirstName = user.FirstName,
        //            Birthday = user.Birthday,
        //            Gender = user.Gender,
        //            CreatedDate = user.CreatedDate,
        //            AccessToken = jwtUserAccount.GenerateToken(jwtOptions)
        //        };

        //        return new ObjectResult(CommandResult.SuccessWithData(tokenVM));
        //    }

        //    return new ObjectResult(CommandResult.Failed(new CommandResultError()
        //    {
        //        Code = (int)HttpStatusCode.InternalServerError,
        //        Description = MessageConstant.SYSTEM_ERROR
        //    }));
        //}

        //[HttpPost]
        //[Route("social-login")]
        //[AllowAnonymous]
        //public async Task<ActionResult> RegisterSocialAsync([FromBody] SaveCustomerSocialViewModel model)
        //{
        //    var result = await _saveCustomerCommand.ExecuteSocialAsync(model);

        //    if (!result.Succeeded)
        //    {
        //        return new ObjectResult(result);
        //    }

        //    CustomerViewModel user = result.Data as CustomerViewModel;

        //    if (user != null)
        //    {
        //        var jwtOptions = _jwtConfiguration.Value;
        //        jwtOptions.ValidFor = TimeSpan.FromHours(JwtValidTime.FOR_CUSTOMER_BY_HOURS);

        //        var jwtUserAccount = new JwtUserAccount
        //        {
        //            UserId = user.Id,
        //            Email = user.Email,
        //            FirstName = user.FirstName
        //        };

        //        TokenViewModel tokenVM = new TokenViewModel()
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            FirstName = user.FirstName,
        //            CreatedDate = user.CreatedDate,
        //            AccessToken = jwtUserAccount.GenerateToken(jwtOptions)
        //        };

        //        return new ObjectResult(CommandResult.SuccessWithData(tokenVM));
        //    }

        //    return new ObjectResult(CommandResult.Failed(new CommandResultError()
        //    {
        //        Code = (int)HttpStatusCode.InternalServerError,
        //        Description = MessageConstant.SYSTEM_ERROR
        //    }));
        //}
        #endregion

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] JObject jsonObject)
        {
            var username = (string)jsonObject.SelectToken("username");
            var result = await _forgotPasswordCommand.ExecuteAsync(username);

            return StatusCode(result.GetStatusCode(), result.GetData());
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            var result = await _changePasswordCommand.ExecuteAsync(model);
            return StatusCode(result.GetStatusCode(), result.GetData());
        }

        [HttpPost("me")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoFromTokenAsync([FromBody] JObject jsonObject)
        {
            var accessToken = (string)jsonObject.SelectToken("accessToken");
            var result = await _getInfoFromTokenQuery.ExecuteAsync(accessToken);

            return new ObjectResult(result);
        }
    }
}
