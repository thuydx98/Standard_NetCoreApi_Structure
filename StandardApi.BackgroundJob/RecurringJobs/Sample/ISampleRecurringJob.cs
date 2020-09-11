namespace StandardApi.BackgroundJob.RecurringJobs.Sample
{
    public interface ISampleRecurringJob
    {
        void ExecuteAsync(string input);
    }
}