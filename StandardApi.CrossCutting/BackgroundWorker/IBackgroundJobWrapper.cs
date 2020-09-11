using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Annotations;

namespace StandardApi.CrossCutting.BackgroundWorker
{
    public interface IBackgroundJobWrapper
    {
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        string Enqueue([NotNull] [InstantHandle] Expression<Action> methodCall);
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        string Enqueue([NotNull] [InstantHandle] Expression<Func<Task>> methodCall);
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        string Enqueue<T>([NotNull] [InstantHandle] Expression<Action<T>> methodCall);
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        string Enqueue<T>([NotNull] [InstantHandle] Expression<Func<T, Task>> methodCall);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of the created job.
        string Schedule([NotNull] [InstantHandle] Expression<Action> methodCall, TimeSpan delay);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of the created job.
        string Schedule([NotNull] [InstantHandle] Expression<Func<Task>> methodCall, TimeSpan delay);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of a created job.
        string Schedule([NotNull] [InstantHandle] Expression<Action> methodCall, DateTimeOffset enqueueAt);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of a created job.
        string Schedule([NotNull] [InstantHandle] Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);
        //
        // Summary:
        //     Creates a new background job based on a specified instance method call expression
        //     and schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     Type whose method will be invoked during job processing.
        //
        // Returns:
        //     Unique identifier of the created job.
        string Schedule<T>([NotNull] [InstantHandle] Expression<Action<T>> methodCall, TimeSpan delay);
        //
        // Summary:
        //     Creates a new background job based on a specified instance method call expression
        //     and schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     Type whose method will be invoked during job processing.
        //
        // Returns:
        //     Unique identifier of the created job.
        string Schedule<T>([NotNull] [InstantHandle] Expression<Func<T, Task>> methodCall, TimeSpan delay);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     The type whose method will be invoked during the job processing.
        //
        // Returns:
        //     Unique identifier of a created job.
        string Schedule<T>([NotNull] [InstantHandle] Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     The type whose method will be invoked during the job processing.
        //
        // Returns:
        //     Unique identifier of a created job.
        string Schedule<T>([NotNull] [InstantHandle] Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);
        //
        // Summary:
        //     Changes state of a job with the specified jobId to the Hangfire.States.DeletedState.
        //     Hangfire.BackgroundJobClientExtensions.Delete(Hangfire.IBackgroundJobClient,System.String)
        //
        // Parameters:
        //   jobId:
        //     An identifier, that will be used to find a job.
        //
        // Returns:
        //     True on a successfull state transition, false otherwise.
        bool Delete([NotNull] string jobId);
        //
        // Summary:
        //     Changes state of a job with the specified jobId to the Hangfire.States.DeletedState.
        //     State change is only performed if current job state is equal to the fromState
        //     value. Hangfire.BackgroundJobClientExtensions.Delete(Hangfire.IBackgroundJobClient,System.String,System.String)
        //
        // Parameters:
        //   jobId:
        //     Identifier of job, whose state is being changed.
        //
        //   fromState:
        //     Current state assertion, or null if unneeded.
        //
        // Returns:
        //     True, if state change succeeded, otherwise false.
        bool Delete([NotNull] string jobId, [CanBeNull] string fromState);

        //
        // Summary:
        //     Changes state of a job with the specified jobId to the Hangfire.States.EnqueuedState.
        //
        // Parameters:
        //   jobId:
        //     Identifier of job, whose state is being changed.
        //
        // Returns:
        //     True, if state change succeeded, otherwise false.
        bool Requeue([NotNull] string jobId);
        //
        // Summary:
        //     Changes state of a job with the specified jobId to the Hangfire.States.EnqueuedState.
        //     If fromState value is not null, state change will be performed only if the current
        //     state name of a job equal to the given value.
        //
        // Parameters:
        //   jobId:
        //     Identifier of job, whose state is being changed.
        //
        //   fromState:
        //     Current state assertion, or null if unneeded.
        //
        // Returns:
        //     True, if state change succeeded, otherwise false.
        bool Requeue([NotNull] string jobId, [CanBeNull] string fromState);
        //
        // Summary:
        //     Creates a new background job that will wait for a successful completion of another
        //     background job to be enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a created job.
        string ContinueWith([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Action> methodCall);
        //
        // Summary:
        //     Creates a new background job that will wait for a successful completion of another
        //     background job to be enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a created job.
        string ContinueWith<T>([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Action<T>> methodCall);

        //
        // Summary:
        //     Creates a new background job that will wait for another background job to be
        //     enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        //   options:
        //     Continuation options.
        //
        // Returns:
        //     Unique identifier of a created job.

        string ContinueWith([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Action> methodCall,
            JobContinuationOptions options);

        //
        // Summary:
        //     Creates a new background job that will wait for another background job to be
        //     enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        //   options:
        //     Continuation options. By default, Hangfire.JobContinuationOptions.OnlyOnSucceededState
        //     is used.
        //
        // Returns:
        //     Unique identifier of a created job.
        string ContinueWith([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Func<Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState);

        //
        // Summary:
        //     Creates a new background job that will wait for another background job to be
        //     enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        //   options:
        //     Continuation options.
        //
        // Returns:
        //     Unique identifier of a created job.
        string ContinueWith<T>([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Action<T>> methodCall,
            JobContinuationOptions options);

        //
        // Summary:
        //     Creates a new background job that will wait for another background job to be
        //     enqueued.
        //
        // Parameters:
        //   parentId:
        //     Identifier of a background job to wait completion for.
        //
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        //   options:
        //     Continuation options. By default, Hangfire.JobContinuationOptions.OnlyOnSucceededState
        //     is used.
        //
        // Returns:
        //     Unique identifier of a created job.
        string ContinueWith<T>([NotNull] string parentId, [NotNull] [InstantHandle] Expression<Func<T, Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState);
    }
}