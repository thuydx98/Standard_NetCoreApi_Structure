using System;
using System.Threading.Tasks;

namespace StandardApi.BackgroundJob.Queue
{
    public interface IBackgroundQueue
    {
        Task QueueTask(Action action);
        Task<T> QueueTask<T>(Func<T> work);
    }
}
