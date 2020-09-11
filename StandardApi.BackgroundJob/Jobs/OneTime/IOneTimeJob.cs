namespace StandardApi.BackgroundJob.Jobs.OneTime
{
    public interface IOneTimeJob
    {
        void ExecuteAsync(string input);
    }
}