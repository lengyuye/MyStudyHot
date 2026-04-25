namespace MyStudyHot;

public class WeakReferenceTest
{
    public static void Test()
    {
        // 创建一个大型对象（以便GC容易注意到它）
        var bigObject = new byte[1024 * 1024 * 10]; // 10 MB
        // 创建指向该对象的短弱引用
        WeakReference weakRef = new WeakReference(bigObject);

        Console.WriteLine("弱引用创建后:");
        Console.WriteLine($"  弱引用.IsAlive = {weakRef.IsAlive}");
        Console.WriteLine($"  弱引用.Target != null = {weakRef.Target != null}");

        // 清除强引用（这是关键：使目标对象只剩下弱引用指向它）
        bigObject = null;

        // 强制进行垃圾回收（仅为演示，实际开发中应避免）
        for (int i = 0; i < 10 && weakRef.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.WriteLine("\n执行 GC.Collect() 并清除强引用后:");
        Console.WriteLine($"  弱引用.IsAlive = {weakRef.IsAlive}");
        Console.WriteLine($"  弱引用.Target != null = {weakRef.Target != null}");

        // 第二部分：演示弱引用对象本身也可以被回收
        CreateAndForgetWeakReference();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("\n弱引用对象本身已被回收（因为没有任何变量引用它）");

        Console.ReadKey();
    }
    
    
    static void CreateAndForgetWeakReference()
    {
        var tempObject = new object();
        var localWeakRef = new WeakReference(tempObject);
        // 方法结束后，localWeakRef 和 tempObject 都没有被任何变量保留（除非被捕获）
        // 因此两者在下一次GC时都会被回收。
        Console.WriteLine("\n在 CreateAndForgetWeakReference 方法内部:");
        Console.WriteLine($"  localWeakRef 引用的目标是否存活？ {localWeakRef.IsAlive}");
    }
}