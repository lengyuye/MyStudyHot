namespace MyStudyHot;

public class FinalizerTest
{
    public void Test()
    {
        /*{
            Finalizer finalizer = new Finalizer();
        }// finalizer 在此作用域结束，变为可回收
  
        //finalizer = null; // 取消引用
        GC.Collect(); // 强制垃圾回收
        GC.WaitForPendingFinalizers(); // 等待所有终结器完成
        Console.WriteLine("已等待终结器完成");*/
        using(Finalizer finalizer = new Finalizer())
        {
            
        }
    }
}

public class Finalizer:IDisposable
{
    public void Dispose()
    {
        // 释放非托管资源的代码
        GC.SuppressFinalize(this); // 防止Finalizer再次被调用
    }
    
    ~Finalizer()
    {
        Console.WriteLine("Finalizer对象被垃圾回收了");
    }
}