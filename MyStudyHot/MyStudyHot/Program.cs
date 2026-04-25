using MyStudyHot.Boxing;

namespace MyStudyHot;

class Program
{
    static void Main(string[] args)
    {

        BoxingTest boxingTest = new BoxingTest();
        //boxingTest.Boxing();
        boxingTest.UnBoxing();
        UnmanagedMemoryTest unmanagedMemoryTest = new UnmanagedMemoryTest();
        unmanagedMemoryTest.Test();
        Console.WriteLine("Hello, World!");
    }
}