using MyStudyHot.Boxing;

namespace MyStudyHot;

class Program
{
    static void Main(string[] args)
    {

        /*
        BoxingTest boxingTest = new BoxingTest();
        boxingTest.Boxing();
        boxingTest.UnBoxing();
        UnmanagedMemoryTest unmanagedMemoryTest = new UnmanagedMemoryTest();
        unmanagedMemoryTest.Test();
        FinalizerTest finalizerTest = new FinalizerTest();
        finalizerTest.Test();
        Console.WriteLine("Hello, World!");
        // 保证主线程不立即退出，便于在某些环境下观察输出
        Thread.Sleep(2000);*/
        /*ObjectPool<int> pool = new ObjectPool<int>();
        int obj1 = pool.Get(); // 获取一个对象，初始值为0
        obj1 = 2;
        Console.WriteLine("获取对象1: " + obj1); // 输出0
        pool.Return(obj1); // 将对象返回池中
        int obj2 = pool.Get(); // 再次获取对象，应该是之前返回的对象
        Console.WriteLine("获取对象2: " + obj2); // 输出0*/
        
        WeakReferenceTest.Test();
    }
}