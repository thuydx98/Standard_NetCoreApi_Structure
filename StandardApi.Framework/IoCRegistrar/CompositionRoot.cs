using DryIoc;
using System;
using System.Net.Http;
using StandardApi.Data.Services;
using StandardApi.Common.Mail;
using Microsoft.Extensions.Options;
using AutoMapper;
using StandardApi.Framework.Mapping;
using StandardApi.Core.Auth.Queries.GetInfoFromToken;
using StandardApi.Core.Auth.Queries.GetRoleByUserId;
using StandardApi.Core.Auth.Queries.GetUserByAccount;
using StandardApi.Core.Auth.Commands.ChangePassword;
using StandardApi.CrossCutting.BackgroundWorker;
using StandardApi.BackgroundJob.Jobs.Sample;
using StandardApi.BackgroundJob.ScheduleJobs.Sample;
using StandardApi.BackgroundJob.RecurringJobs.Sample;
using StandardApi.Core.User.Commands.UploadUserAvatar;
using StandardApi.Core.User.Queries.GetUserAvatar;
using StandardApi.Common.Extentions;
using StandardApi.Core.Auth.Commands.ForgotPassword;
using StandardApi.BackgroundJob.Jobs.Mails.SendRecoveryCode;
using StandardApi.Core.Auth.Queries.GetCurrentUser;

namespace StandardApi.Framework.IoCRegistrar
{
    public static class CompositionRoot
    {
        public static void Register(IRegistrator registrator)
        {
            #region General
            registrator.RegisterDelegate<IMapper>(resolver =>
            {
                var mapperConfiguration = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                return mapperConfiguration.CreateMapper();
            }, Reuse.Singleton);

            registrator.Register<Lazy<HttpClient>>(Reuse.Scoped);

            registrator.Register<IDbContext, ApplicationDbContext>(Reuse.Scoped);
            registrator.Register(typeof(IRepository<>), typeof(EfRepository<>), Reuse.Scoped);

            registrator.RegisterDelegate(resolver => new Lazy<IMailService>(() =>
            {
                var mailServerSetting = resolver.Resolve<IOptionsSnapshot<MailServerSetting>>().Value;
                return new MailService(mailServerSetting);
            }), Reuse.Scoped);
            #endregion

            #region Common
            registrator.Register<RandomGenerator>(Reuse.Singleton);
            #endregion

            #region Background Worker
            registrator.Register<IBackgroundJobWrapper, BackgroundJobWrapper>(Reuse.Scoped);
            registrator.Register<IRecurringJobWrapper, RecurringJobWrapper>(Reuse.Scoped);

            // Jobs
            registrator.Register<ISampleJob, SampleJob>(Reuse.Scoped);
            registrator.Register<ISendRecoveryCode, SendRecoveryCode>(Reuse.Scoped);

            // Schedule Jobs
            registrator.Register<ISampleScheduleJob, SampleScheduleJob>(Reuse.Scoped);

            // Recurring Jobs
            registrator.Register<ISampleRecurringJob, SampleRecurringJob>(Reuse.Scoped);
            #endregion

            #region StandardApi.Chatbot
            #endregion

            #region StandardApi.Core

            #region Auth
            registrator.Register<IGetCurrentUserQuery, GetCurrentUserQuery>(Reuse.Scoped);
            registrator.Register<IGetInfoFromTokenQuery, GetInfoFromTokenQuery>(Reuse.Scoped);
            registrator.Register<IGetRoleByUserIdQuery, GetRoleByUserIdQuery>(Reuse.Scoped);
            registrator.Register<IGetUserByAccountQuery, GetUserByAccountQuery>(Reuse.Scoped);
            registrator.Register<IChangePasswordCommand, ChangePasswordCommand>(Reuse.Scoped);
            registrator.Register<IForgotPasswordCommand, ForgotPasswordCommand>(Reuse.Scoped);
            #endregion

            #region User
            registrator.Register<IUploadUserAvatarCommand, UploadUserAvatarCommand>(Reuse.Scoped);
            registrator.Register<IGetUserAvatarQuery, GetUserAvatarQuery>(Reuse.Scoped);
            #endregion

            #endregion
        }
    }
}
