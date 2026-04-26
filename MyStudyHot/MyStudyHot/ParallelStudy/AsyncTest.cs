namespace MyStudyHot;

public class AsyncTest2
{
    public async Task DoSomethingAsync()
    {
        //同步执行代码块，这个代码块在调用这个方法的线程中执行
        Utils.PrintCurrentThreadId("DoSomethingAsync 1");//线程ID 1
        int val = 13;

        //异步方式等待1秒
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        //同步执行代码块
        Utils.PrintCurrentThreadId("DoSomethingAsync 2");//线程ID 5 --线程池中的线程
        val *= 2; 

        //异步方式等待1秒 , ConfigureAwait false  没有上下文切换，不会回到之前的线程。
        // 控制台程序下 是没有默认的上下文，所以只会回到默认的线程池
        // 控制台下，不配置ConfigureAwait(false) 也是可以的
        //不需要回到之前线程的情况，就可以ConfigureAwait(false)
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

        //同步执行代码块
        Utils.PrintCurrentThreadId("DoSomethingAsync 3");//线程ID 5 --线程池中的线程
        Console.WriteLine(val);

    }

    #region  处理错误的例子

    /// <summary>
    /// 这里仅仅示例，使用async 和 await 时，要处理错误
    /// </summary>
    async Task TrySomethingAsync()
    {
        //发生异常时，任务结束。不会直接抛出异常
        Task task = PossibleExceptionAsync();
        try
        {
            //Task 对象中的异常，会在这条await 语句中抛出
            await task;
        }
        catch (NotSupportedException ex)
        {
            LogException(ex);
            throw;
        }
    }

    private void LogException(NotSupportedException notSupportedException)
    {
        Console.WriteLine(notSupportedException);
    }

    async Task PossibleExceptionAsync()
    {
        
    }

     #endregion


    #region  死锁的例子

    async Task WaitAsync()
    {
        // 这里 awati 会捕获当前上下文……
        await Task.Delay(TimeSpan.FromSeconds(1));
        // ……这里会试图用上面捕获的上下文继续执行
    }

    void Deadlock()
    {
        // 开始延迟
        Task task = WaitAsync();

        // 同步程序块，正在等待异步方法完成
        task.Wait();
    }
   #endregion
 
}