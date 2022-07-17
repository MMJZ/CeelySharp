namespace CeelySharp.MTask;

public static class MTask
{
    private static readonly Task MEmpty = Task.Delay(Timeout.Infinite);
    private static readonly Task MPure = Task.CompletedTask;

    public static Task Guard(bool predicate) =>
        predicate ? MPure : MEmpty;
}
