namespace Zaaby.Client.Http.Internal;

internal static class TaskExtensions
{
    internal static Task CastResult(this Task<object?> taskResult, Type resultType)
    {
        var taskSource = new TaskCompletionSource(resultType);
        taskResult.ContinueWith(task =>
        {
            try
            {
                taskSource.SetResult(task.Result);
            }
            catch (AggregateException ex)
            {
                taskSource.SetException(ex.InnerException!);
            }
            catch (Exception ex)
            {
                taskSource.SetException(ex);
            }
        });
        return taskSource.Task;
    }
}