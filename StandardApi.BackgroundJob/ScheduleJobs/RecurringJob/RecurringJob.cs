using System;

namespace StandardApi.BackgroundJob.ScheduleJobs.RecurringJob
{
    public class RecurringJob : IRecurringJob
    {
        public void ExecuteAsync(string input)
        {
            Console.WriteLine($"Hangfire recurring job with METHOD" + new DateTime().ToString() + input);
        }
    }
}
