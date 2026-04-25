using System.Runtime.InteropServices;

namespace MyStudyHot;

public class UnmanagedMemoryTest
{
    public void Test()
    {
        IntPtr ptr = Marshal.AllocHGlobal(1000); // 分配1000字节的非托管内存
        try
        {
            //向其中写入数据（使用指针）
            unsafe
            {
                byte* p = (byte*)ptr;
                p[0] = 123;
            }
            //或者用Marshal 类读写
            Marshal.WriteByte(ptr,0,123);
        }
        finally
        {
            //必须手动释放，否则内存泄露
            Marshal.FreeHGlobal(ptr);
        }
    }
}