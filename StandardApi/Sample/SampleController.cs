using Microsoft.AspNetCore.Mvc;
using StandardApi.BackgroundJob.Jobs.OneTime;
using StandardApi.BackgroundJob.ScheduleJobs.RecurringJob;
using StandardApi.CrossCutting.BackgroundWorker;
using StandardApi.Data.Entities.BackgroundWorker;
using System;

namespace StandardApi.Sample
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IRecurringJobWrapper _recurringJobWrapper;
        private readonly IBackgroundJobWrapper _backgroundJobWrapper;

        public SampleController(
            IRecurringJobWrapper recurringJobWrapper,
            IBackgroundJobWrapper backgroundJobWrapper)
        {
            _recurringJobWrapper = recurringJobWrapper;
            _backgroundJobWrapper = backgroundJobWrapper;
        }

        [HttpPost]
        [Route("create-jobs")]
        public IActionResult CreateBackgroundJobs()
        {
            // Queue Job and execute now
            _backgroundJobWrapper.Enqueue<IOneTimeJob>(job => job.ExecuteAsync("Executing one time schedule job now"));

            // Queue Job and execute in custom time (it will return Job ID)
            _backgroundJobWrapper.Schedule<IOneTimeJob>(job => job.ExecuteAsync("Executing one time schedule job after 2 minute"), DateTime.Now.AddMinutes(2));

            // Recurring Job
            HangfireServiceEntity service = new HangfireServiceEntity()
            {
                Id = 123,
                Name = "ID - Run every minute",
                Cron = "* * * * *"
            };
            _recurringJobWrapper.AddOrUpdate<IRecurringJob>(service.Name, job => job.ExecuteAsync(" input value"), service.Cron, TimeZoneInfo.Local);

            return new ObjectResult("Done");
        }
    }
}
