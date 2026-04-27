namespace MyStudyHot.ParallelStudy;

interface IMyAsyncInterface
{
    Task<int> GetValueAsync();
}

class  MySynchronousImplementation : IMyAsyncInterface
{
    public Task<int> GetValueAsync()
    {
        //同步实现，直接返回结果
        return Task.FromResult(42);
    }
}
