namespace CeelySharp.TaskMonad;

public static class MTask
{
    private static readonly Task MEmpty = Task.Delay(Timeout.Infinite);
    private static readonly Task MPure = Unit();

    public static Task Guard(bool predicate) =>
        predicate ? MPure : MEmpty;

    public static async Task Unit()
    {
    }

    public static async Task<T> Unit<T>(T t) => t;
}
