using System;
using System.Threading;

namespace StandardApi.BackgroundJob.Jobs.OneTime
{
    public class OneTimeJob : IOneTimeJob
    {
        public void ExecuteAsync(string input)
        {
            Console.WriteLine(string.Format("Hangfire background job with METHOD {0} {1}", DateTime.Now, input));
            Thread.Sleep(20000);
        }
    }
}
