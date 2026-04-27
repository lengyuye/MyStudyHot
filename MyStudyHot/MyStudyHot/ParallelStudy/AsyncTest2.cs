namespace MyStudyHot.ParallelStudy;

//异步编程测试类
public class AsyncTest2
{
    public async Task<int> GetUrlContentLengthAsync()
    {
        using var client = new HttpClient();
        Task<string> getStringTask = client.GetStringAsync("https://learn.microsoft.com/dotnet");
        DoIndependentWork();
        string content = await getStringTask;
        return content.Length;
    }
    
    void DoIndependentWork()
    {
        Console.WriteLine("Working...");
    }
    
    // 模拟的异步方法
    public async Task<string> GetDataAsync()
    {
        // 1. 同步部分开始：在线程 A 上执行
        Console.WriteLine("1. 开始获取数据");

        // 2. 发起一个异步 I/O 操作，比如网络请求
        //    HttpClient 内部使用的是操作系统级别的 I/O 完成端口等技术，
        //    它本身不消耗任何线程去“等待”。
        using var client = new HttpClient();
        Task<string> task = client.GetStringAsync("某个网址");

        // 3. 关键点：遇到 await，操作还未完成
        //    当前线程 A 会被释放，返回给调用方
        string result = await task;

        // 4. I/O 操作完成后，某个线程（可能是线程 B，也可能是刚释放的线程 A）
        //    会回来继续执行这里的代码
        Console.WriteLine($"2. 获取到数据：{result.Length} 个字符");
        return result;
    }

    private static async Task<User> GetUserAsync(int userId)
    {
        return await Task.FromResult(new User { Id = userId});
    }
    
    //Task.WhenAll 可以等待多个异步操作完成，并且在所有操作完成后返回一个包含所有结果的数组。
    private static async Task<IEnumerable<User>> GetUsersAsync(IEnumerable<int> userIds)
    {
        var getUserTasks = new List<Task<User>>();
        foreach (int userId in userIds)
        {
            getUserTasks.Add(GetUserAsync(userId));
        }
        return await Task.WhenAll(getUserTasks);
    }
    
    private static async Task<User[]> GetUsersAsync2(IEnumerable<int> userIds)
    {
       var getUserTasks = userIds.Select(id => GetUserAsync(id));
       return await Task.WhenAll(getUserTasks);
    }
    
    private static async Task ProcessTasksAsTheyCompleteAsync(IEnumerable<int> userIds)
    {
        var getUserTasks = userIds.Select(id => GetUserAsync(id)).ToList();
        while (getUserTasks.Count > 0)
        {
            Task<User> completedTask = await Task.WhenAny(getUserTasks);
            getUserTasks.Remove(completedTask);
            User user = await completedTask;
            Console.WriteLine($"处理用户 {user.Id}");
        }
    }

    //延迟的例子1
    static async Task<T> DelayResult<T>(T result, TimeSpan delay)
    {
        await Task.Delay(delay);
        return result;
    }
    
    //延迟的例子2
    static async Task<string> DownloadStringWithRetries(string url)
    {
        using (var client = new HttpClient())
        {
            var nextDelay = TimeSpan.FromSeconds(1);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    return await client.GetStringAsync(url);
                }
                catch 
                {
                    
                }
                await Task.Delay(nextDelay);
                nextDelay *= 2; // 指数回退
            }
            return await client.GetStringAsync(url);
        }
    }
    
    //延迟的例子3
    static async Task<string> DownloadStringWithTimeout(string url)
    {
        using (var client = new HttpClient())
        {
            var downloadTask = client.GetStringAsync(url);
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(3));

            var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
            if (completedTask == timeoutTask)
                return null;
            return await downloadTask;
        }
    }

    static Task<T> NotImplementedAsync<T>()
    {
        var tcs = new TaskCompletionSource<T>();
        tcs.SetException(new NotImplementedException());
        return tcs.Task;
    }
    
    private static readonly Task<int> _zeroTask = Task.FromResult(0);
    static Task<int> GetValueAsync()
    {
        return _zeroTask;
    }

    #region 报告进度

    static async Task MyMethodAsync(IProgress<double> progress = null)
    {
        double percentComplete = 0;
        bool done = false;
        while (!done)
        {
            if (progress != null)
            {
                progress.Report(percentComplete);
            }
        }
    }

    static async Task CallMyMethodAsync()
    {
        var progress = new Progress<double>();
        progress.ProgressChanged += (s, percent) =>
        {
            Console.WriteLine($"进度: {percent}%");
        };
        await MyMethodAsync(progress);
    }

    #endregion

    #region 等待一组任务完成

    async Task Wait1()
    {
        Task task1 = Task.Delay(TimeSpan.FromSeconds(1));
        Task task2 = Task.Delay(TimeSpan.FromSeconds(2));
        Task task3 = Task.Delay(TimeSpan.FromSeconds(1));
        await Task.WhenAll(task1, task2,task3);
    }

    async Task Wait2()
    {
        Task<int> task1 = Task.FromResult(3);
        Task<int> task2 = Task.FromResult(5);
        Task<int> task3 = Task.FromResult(7);
        int[] results = await Task.WhenAll(task1, task2,task3);
    }
    #endregion

    #region 等待任意一个任务完成

    private static async Task<int> FirstRespondingUrlAsync(string urlA, string urlB)
    {
        var httpClient = new HttpClient();
        
        //并发地开始下载两个任务
        Task<byte[]> downloadA = httpClient.GetByteArrayAsync(urlA);
        Task<byte[]> downloadB = httpClient.GetByteArrayAsync(urlB);
        
        //等待任意一个任务完成
        Task<byte[]> completedTask = await Task.WhenAny(downloadA, downloadB);
        
        //返回从URL下载的内容长度
        byte[] data = await completedTask;
        return data.Length;
    }

    #endregion
}