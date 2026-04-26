namespace MyStudyHot;

public class Utils
{
    public static void PrintCurrentThreadId(string title)
    {
        Console.WriteLine($"{title} 当前线程ID: {Thread.CurrentThread.ManagedThreadId}");
    }
}