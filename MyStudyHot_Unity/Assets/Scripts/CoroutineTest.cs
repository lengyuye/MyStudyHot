using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadDataAsync());
        StartCoroutine(HeavyCalculationCoroutine());
    }
    
    IEnumerator LoadDataAsync()
    {
       UnityWebRequest request = UnityWebRequest.Get("http://baidu.com");
        yield return request.SendWebRequest();// 异步等待请求完成
        Debug.Log("请求完成，状态码: " + request.responseCode);
    }

    IEnumerator HeavyCalculationCoroutine()
    {
        bool isDone = false;
        Task.Run(() =>
        {
            HeavyCalculation();
            isDone = true;
        });

        while (isDone == false)
        {
            yield return null; //等待子线程完成
        }

        Debug.Log("耗时操作完成");
    }

    private void HeavyCalculation()
    {
        // 模拟 CPU 密集型工作：大量平方根计算
        double result = 0;
        const long iterations = 50_000_000; // 可根据需要调整
        for (long i = 0; i < iterations; i++)
        {
            result += System.Math.Sqrt(i & 1023); // 保持一些计算量

            // 定期短暂休眠以让出时间片，避免完全占用 CPU（可选）
            if ((i % 5_000_000) == 0 && i != 0)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        // 防止编译器优化掉计算结果
        System.GC.KeepAlive(result);
    }

}
