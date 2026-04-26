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
}