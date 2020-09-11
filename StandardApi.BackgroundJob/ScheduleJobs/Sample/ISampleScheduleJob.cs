namespace StandardApi.BackgroundJob.ScheduleJobs.Sample
{
    public interface ISampleScheduleJob
    {
        void ExecuteAsync(string input);
    }
}