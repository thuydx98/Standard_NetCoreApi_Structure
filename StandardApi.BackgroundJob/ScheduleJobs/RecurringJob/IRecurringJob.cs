namespace StandardApi.BackgroundJob.ScheduleJobs.RecurringJob
{
    public interface IRecurringJob
    {
        void ExecuteAsync(string input);
    }
}