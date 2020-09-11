using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StandardApi.BackgroundJob.Jobs.Sample;
using StandardApi.BackgroundJob.RecurringJobs.Sample;
using StandardApi.BackgroundJob.ScheduleJobs.Sample;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("create-jobs")]
        public IActionResult CreateBackgroundJobs()
        {
            // Queue Job not concurrency
            _backgroundJobWrapper.Enqueue<ISampleJob>(job => job.ExecuteAsync("Input value of Queue Job 1"));
            _backgroundJobWrapper.Enqueue<ISampleJob>(job => job.ExecuteAsync("Input value of Queue Job 2"));

            // Schedule Job
            _backgroundJobWrapper.Schedule<ISampleScheduleJob>(job => job.ExecuteAsync("Input value of Schedule Job"), DateTime.Now.AddMinutes(2));

            // Recurring Job
            HangfireServiceEntity service = new HangfireServiceEntity()
            {
                Id = 123,
                Name = "ID - Run every minute",
                Cron = "* * * * *"
            };
            _recurringJobWrapper.AddOrUpdate<ISampleRecurringJob>(service.Name, job => job.ExecuteAsync("Input value of Recurring Job"), service.Cron, TimeZoneInfo.Local);

            return new ObjectResult(new { success = true });
        }
    }
}
