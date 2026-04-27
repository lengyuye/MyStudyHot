using System.Numerics;
using  System.Threading.Tasks;

namespace MyStudyHot.ParallelStudy;

/// <summary>
/// 并行开发测试类
/// </summary>
public class ParallelTest1
{
    //数据并行示例
    void RotateMatrices(IEnumerable<Matrix3x2> matrices)
    {
        Parallel.ForEach(matrices, matrix => matrix.GetDeterminant());
    }

    //任务并行示例
    void ProcessArray(double[] array)
    {
        Parallel.Invoke(
            () => ProcessPartialArray(array, 0, array.Length / 2),
            () => ProcessPartialArray(array, array.Length / 2, array.Length)
        );
    }
    
    void ProcessPartialArray(double[] array, int begin, int end)
    {
        //CPU密集型任务
    }

    public void TestException()
    {
        try
        {
            Parallel.Invoke(()=>{ throw new Exception(); }, 
                () => {throw new Exception(); });
        }
        catch (AggregateException ex)
        {
           ex.Handle(exception =>
           {
               Console.WriteLine("处理异常: " + exception.Message);
               return true;
           });
        }
    }
    
    
}