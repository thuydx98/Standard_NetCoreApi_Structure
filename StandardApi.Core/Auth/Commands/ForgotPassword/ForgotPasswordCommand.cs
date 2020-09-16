using Microsoft.EntityFrameworkCore;
using StandardApi.BackgroundJob.Jobs.Mails.SendRecoveryCode;
using StandardApi.Common.Enums;
using StandardApi.Common.Extentions;
using StandardApi.Constants.Message;
using StandardApi.CrossCutting.BackgroundWorker;
using StandardApi.CrossCutting.Commands;
using StandardApi.Data.Entities.User;
using StandardApi.Data.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IForgotPasswordCommand
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly RandomGenerator _randomGenerator;
        private readonly IBackgroundJobWrapper _backgroundJobWrapper;

        public ForgotPasswordCommand(
            IRepository<UserEntity> userRepository, 
            RandomGenerator randomGenerator, 
            IBackgroundJobWrapper backgroundJobWrapper)
        {
            _userRepository = userRepository;
            _randomGenerator = randomGenerator;
            _backgroundJobWrapper = backgroundJobWrapper;
        }

        public async Task<CommandResult> ExecuteAsync(string username)
        {
            if (username.IsEmpty())
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = HttpMessage.SomeDataEmptyOrInvalid
                });
            }

            var recoveryCode = _randomGenerator.RandomNumber(0, 99999).ToString("D5");
            var user = await _userRepository.Table
                .Where(n => n.Username == username || n.Email == username || n.PhoneNumber == username)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Message = HttpMessage.NotFound
                });
            }

            user.RecoveryCode = recoveryCode;
            user.RecoveryRequestTime = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            _backgroundJobWrapper.Enqueue<ISendRecoveryCode>(job => job.ExecuteAsync(user.Email, recoveryCode));

            return CommandResult.Success;
        }
    }
}
