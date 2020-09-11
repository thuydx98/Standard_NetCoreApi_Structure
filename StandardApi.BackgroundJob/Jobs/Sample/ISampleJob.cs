namespace StandardApi.BackgroundJob.Jobs.Sample
{
    public interface ISampleJob
    {
        void ExecuteAsync(string input);
    }
}