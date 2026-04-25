namespace MyStudyHot.Boxing;

/// <summary>
/// 测试装箱、拆箱
/// </summary>
public class BoxingTest
{
    public void Boxing()
    {
        int i = 10;
        object oi = i; // 装箱
        Console.WriteLine("i:{0},oi:{1}",i,oi);//输出 i:10,oi:10
        
        i = 12;
        oi = 15;
        Console.WriteLine("i:{0},oi:{1}", i, oi);//输出    i:12,oi:15
    }
    
    public void UnBoxing()
    {
        int i = 10;
        object oi = i; // 装箱
        int j = (int)oi; // 拆箱
        Console.WriteLine("i:{0},oi:{1},j:{2}", i, oi, j);//输出 i:10,oi:10,j:10
    }
}