namespace Aoxe.Client.Http.Internal;

internal static class AsyncExtensions
{
    private static readonly TaskFactory TaskFactory =
        new(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default
        );

    public static TResult? RunSync<TResult>(this Task<TResult?> func)
    {
        return TaskFactory.StartNew(Func).Unwrap().GetAwaiter().GetResult();
        Task<TResult?> Func() => func;
    }

    public static void RunSync(this Task func)
    {
        TaskFactory.StartNew(Func).Unwrap().GetAwaiter().GetResult();
        return;
        Task Func() => func;
    }
}
