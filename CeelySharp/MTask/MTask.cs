namespace CeelySharp.MTask;

public static class MTask
{
    private static readonly Task MEmpty = Task.Delay(Timeout.Infinite);
    private static readonly Task MPure = Unit();

    public static Task Guard(bool predicate) =>
        predicate ? MPure : MEmpty;

    public static async Task Unit()
    {
    }
}
