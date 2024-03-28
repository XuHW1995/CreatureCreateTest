using Unity.Burst;
using UnityEngine;  
using UnityEngine.Jobs;  
using UnityEngine.Profiling;  
using Unity.Collections;  
using Unity.Jobs;  
  
public class SimpleJobSystemExample : MonoBehaviour  
{  
    private NativeArray<float> inputArray;  
    private NativeArray<float> outputArray;  
  
    void Start()  
    {  
        int length = 1024;  
        inputArray = new NativeArray<float>(length, Allocator.TempJob);  
        outputArray = new NativeArray<float>(length, Allocator.TempJob);  
  
        // 填充 inputArray 的数据...  
  
        var jobHandle = new MyJob(inputArray, outputArray).Schedule(default);  
        
        // 等待 Job 完成  
        jobHandle.Complete();  
  
        // 使用 outputArray 的数据...  
  
        // 释放 NativeArray 的内存  
        inputArray.Dispose();  
        outputArray.Dispose();  
    }  
  
    [BurstCompile]  
    public struct MyJob : IJob 
    {  
        public NativeArray<float> input;  
        public NativeArray<float> output;
        
        public MyJob(NativeArray<float> input, NativeArray<float> output)
        {
            this.input = input;
            this.output = output;
        }
        
        public void Execute()  
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = output[i] * 2;
            }
        }  
    }  
}