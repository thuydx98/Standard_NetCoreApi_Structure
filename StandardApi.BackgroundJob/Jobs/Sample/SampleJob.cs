using System;
using System.Threading;

namespace StandardApi.BackgroundJob.Jobs.Sample
{
    public class SampleJob : ISampleJob
    {
        public void ExecuteAsync(string input)
        {
            Console.WriteLine(string.Format("[QUEUE JOB] Executing at {0} and sleep 10s with input value: {1} ", DateTime.Now, input));
            Thread.Sleep(10 * 1000);
        }
    }
}
