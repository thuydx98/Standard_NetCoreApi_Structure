using System;
using System.Threading;

namespace StandardApi.BackgroundJob.RecurringJobs.Sample
{
    public class SampleRecurringJob : ISampleRecurringJob
    {
        public void ExecuteAsync(string input)
        {
            Console.WriteLine(string.Format("[RECURRING JOB] Executing at {0} and sleep 10s with input value: {1} ", new DateTime(), input));
            Thread.Sleep(10 * 1000);
        }
    }
}
