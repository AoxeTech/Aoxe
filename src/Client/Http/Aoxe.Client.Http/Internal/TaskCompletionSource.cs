namespace Aoxe.Client.Http.Internal;

internal class TaskCompletionSource
{
    private readonly ITaskCompletionSource _taskSource;

    public Task Task => _taskSource.Task;

    public TaskCompletionSource(Type resultType)
    {
        var type = typeof(TaskCompletionSourceOf<>).MakeGenericType(resultType);
        _taskSource = (Activator.CreateInstance(type) as ITaskCompletionSource)!;
    }

    public bool SetResult(object? result) => _taskSource.SetResult(result);

    public bool SetException(Exception ex) => _taskSource.SetException(ex);

    private interface ITaskCompletionSource
    {
        Task Task { get; }

        bool SetResult(object? result);

        bool SetException(Exception ex);
    }

    private class TaskCompletionSourceOf<TResult> : ITaskCompletionSource
    {
        private readonly TaskCompletionSource<TResult?> _taskSource;

        public Task Task => _taskSource.Task;

        public TaskCompletionSourceOf() => _taskSource = new TaskCompletionSource<TResult?>();

        public bool SetResult(object? result) => _taskSource.TrySetResult((TResult?)result);

        public bool SetException(Exception ex) => _taskSource.TrySetException(ex);
    }
}
