using System;
using System.Threading;

namespace StandardApi.BackgroundJob.ScheduleJobs.Sample
{
    public class SampleScheduleJob : ISampleScheduleJob
    {
        public void ExecuteAsync(string input)
        {
            Console.WriteLine(string.Format("[SCHEDULE JOB] Executing at {0} and sleep 5s with input value: {1} ", new DateTime(), input));
            Thread.Sleep(5 * 1000);
        }
    }
}
