using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Annotations;

namespace StandardApi.CrossCutting.BackgroundWorker
{
    public class BackgroundJobWrapper : IBackgroundJobWrapper
    {
        public string Enqueue([NotNull][InstantHandle] Expression<Action> methodCall)
            => BackgroundJob.Enqueue(methodCall);
        public string Enqueue([NotNull][InstantHandle] Expression<Func<Task>> methodCall)
            => BackgroundJob.Enqueue(methodCall);

        public string Enqueue<T>([NotNull][InstantHandle] Expression<Action<T>> methodCall)
            => BackgroundJob.Enqueue(methodCall);

        public string Enqueue<T>([NotNull][InstantHandle] Expression<Func<T, Task>> methodCall)
            => BackgroundJob.Enqueue(methodCall);

        public string Schedule([NotNull][InstantHandle] Expression<Action> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        public string Schedule([NotNull][InstantHandle] Expression<Func<Task>> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        public string Schedule([NotNull][InstantHandle] Expression<Action> methodCall, DateTimeOffset enqueueAt)
            => BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule([NotNull][InstantHandle] Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
            => BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule<T>([NotNull][InstantHandle] Expression<Action<T>> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        public string Schedule<T>([NotNull][InstantHandle] Expression<Func<T, Task>> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        public string Schedule<T>([NotNull][InstantHandle] Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
            => BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule<T>([NotNull][InstantHandle] Expression<Func<T, Task>> methodCall,
            DateTimeOffset enqueueAt) => BackgroundJob.Schedule(methodCall, enqueueAt);

        public bool Delete([NotNull] string jobId) => BackgroundJob.Delete(jobId);

        public bool Delete([NotNull] string jobId, [CanBeNull] string fromState)
            => BackgroundJob.Delete(jobId, fromState);

        public bool Requeue([NotNull] string jobId) => BackgroundJob.Requeue(jobId);

        public bool Requeue([NotNull] string jobId, [CanBeNull] string fromState)
            => BackgroundJob.Requeue(jobId, fromState);

        public string ContinueWith([NotNull] string parentId, [NotNull][InstantHandle] Expression<Action> methodCall)
            => BackgroundJob.ContinueJobWith(parentId, methodCall);

        public string ContinueWith<T>([NotNull] string parentId,
            [NotNull][InstantHandle] Expression<Action<T>> methodCall)
            => BackgroundJob.ContinueJobWith(parentId, methodCall);

        public string ContinueWith([NotNull] string parentId, [NotNull][InstantHandle] Expression<Action> methodCall,
            JobContinuationOptions options) => BackgroundJob.ContinueJobWith(parentId, methodCall, options);

        public string ContinueWith([NotNull] string parentId,
            [NotNull][InstantHandle] Expression<Func<Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
            => BackgroundJob.ContinueJobWith(parentId, methodCall, options);

        public string ContinueWith<T>([NotNull] string parentId,
            [NotNull][InstantHandle] Expression<Action<T>> methodCall, JobContinuationOptions options)
            => BackgroundJob.ContinueJobWith(parentId, methodCall, options);

        public string ContinueWith<T>([NotNull] string parentId,
            [NotNull][InstantHandle] Expression<Func<T, Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
            => BackgroundJob.ContinueJobWith(parentId, methodCall, options);
    }
}