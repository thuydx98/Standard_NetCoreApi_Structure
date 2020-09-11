using DryIoc;
using System;
using System.Net.Http;
using StandardApi.Data.Services;
using StandardApi.Common.Mail;
using Microsoft.Extensions.Options;
using StandardApi.BackgroundJob.Queue;
using AutoMapper;
using StandardApi.Framework.Mapping;
using StandardApi.Core.Auth.Queries.GetInfoFromToken;
using StandardApi.Core.Auth.Queries.GetRoleByUserId;
using StandardApi.Core.Auth.Queries.GetUserByAccount;
using StandardApi.Core.Auth.Commands.ChangePassword;
using StandardApi.BackgroundJob.ScheduleJobs.RecurringJob;
using StandardApi.BackgroundJob.Jobs.OneTime;
using StandardApi.CrossCutting.BackgroundWorker;

namespace StandardApi.Framework.IoCRegistrar
{
    public static class CompositionRoot
    {
        public static void Register(IRegistrator registrator)
        {
            // General
            registrator.RegisterDelegate<IMapper>(resolver =>
            {
                var mapperConfiguration = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });

                return mapperConfiguration.CreateMapper();
            }, Reuse.Singleton);

            registrator.Register<IBackgroundQueue, BackgroundQueue>(Reuse.Singleton);
            registrator.Register<Lazy<HttpClient>>(Reuse.Scoped);

            registrator.Register<IDbContext, ApplicationDbContext>(Reuse.Scoped);
            registrator.Register(typeof(IRepository<>), typeof(EfRepository<>), Reuse.Scoped);

            registrator.Register<IBackgroundJobWrapper, BackgroundJobWrapper>(Reuse.Scoped);
            registrator.Register<IRecurringJobWrapper, RecurringJobWrapper>(Reuse.Scoped);

            registrator.RegisterDelegate(resolver => new Lazy<IMailService>(() =>
            {
                var mailServerSetting = resolver.Resolve<IOptionsSnapshot<MailServerSetting>>().Value;
                return new MailService(mailServerSetting);
            }), Reuse.Scoped);

            // Background Jobs
            registrator.Register<IOneTimeJob, OneTimeJob>(Reuse.Scoped);
            registrator.Register<IRecurringJob, RecurringJob>(Reuse.Scoped);

            // Auth
            registrator.Register<IGetInfoFromTokenQuery, GetInfoFromTokenQuery>(Reuse.Scoped);
            registrator.Register<IGetRoleByUserIdQuery, GetRoleByUserIdQuery>(Reuse.Scoped);
            registrator.Register<IGetUserByAccountQuery, GetUserByAccountQuery>(Reuse.Scoped);
            registrator.Register<IChangePasswordCommand, ChangePasswordCommand>(Reuse.Scoped);
        }
    }
}
