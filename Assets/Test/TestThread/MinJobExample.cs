using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Profiling;

// 定义 Job 结构体
public struct MyJob : IJobParallelFor
{
    public NativeArray<float> input;
    public NativeArray<float> result;
        
    public void Execute(int index)
    {
        Profiler.BeginSample($"XHW JOB {index}");
        // 这里可以编写并行执行的任务
        result[index] = input[index] * 2;
        Debug.Log("Job[" + index + "]: " + input[index] + " * 2 = " + result[index] + "Execute method running on thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        Profiler.EndSample();
    }
}

public class MinJobExample : MonoBehaviour
{
    public int num = 1000;
    public int numThreads = 1;
    private JobHandle syncHandle;

    private JobHandle asyncHandle;
    private NativeArray<float> asyncInputData;
    private NativeArray<float> asyncResultData;
    
    void Start()
    {
        #region syncJob
        Debug.Log($"SyncJob started  curframe:{Time.frameCount}");
        // 创建并初始化输入数据
        NativeArray<float> inputData = new NativeArray<float>(num, Allocator.TempJob);
        for (int i = 0; i < inputData.Length; i++)
        {
            inputData[i] = i;
        }

        // 创建输出数据
        NativeArray<float>resultData = new NativeArray<float>(num, Allocator.TempJob);

        // 创建并调度 Job
        MyJob job = new MyJob
        {
            input = inputData,
            result = resultData
        };
        
        syncHandle = job.Schedule(num, numThreads);
        syncHandle.Complete();
        // 释放 NativeArray 资源
        inputData.Dispose();
        resultData.Dispose();
        Debug.Log($"SyncJob finished curframe:{Time.frameCount}");
        #endregion

        #region AsyncJob
        // 创建并初始化输入数据
        asyncInputData = new NativeArray<float>(num, Allocator.TempJob);
        for (int i = 0; i < asyncInputData.Length; i++)
        {
            asyncInputData[i] = i;
        }

        // 创建输出数据
         asyncResultData = new NativeArray<float>(num, Allocator.TempJob);

        // 创建并调度 Job
        MyJob asyncJob = new MyJob
        {
            input = asyncInputData,
            result = asyncResultData
        };
        
        Debug.Log($"AsyncJob started curframe:{Time.frameCount}");
        asyncHandle = asyncJob.Schedule(asyncInputData.Length, numThreads);
        #endregion
    }
    
    bool isAsyncJobsDone = false;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NativeArray<float> temp = new NativeArray<float>(10, Allocator.TempJob);
            NativeArray<float> tempresult = new NativeArray<float>(10, Allocator.TempJob);
            MyJob job = new MyJob
            {
                input = temp,
                result = tempresult
            };

            Debug.Log($"tempJob created {job}");
            JobHandle handle = job.Schedule(5, 1);
            handle.Complete();
            
            temp.Dispose();
            tempresult.Dispose();
            Debug.Log($"temp Job finished {job}");
        }
        
        if (asyncHandle.IsCompleted && !isAsyncJobsDone)
        {
            asyncHandle.Complete();
            isAsyncJobsDone = true;
            Debug.Log($"AsyncJob finished curframe:{Time.frameCount}");
            asyncInputData.Dispose();
            asyncResultData.Dispose();
        }
    }

    void OnApplicationQuit()
    {
        // 等待 Job 执行完成
        asyncHandle.Complete();

        if (asyncInputData.Length != 0)
        {
            // 释放 NativeArray 资源
            asyncInputData.Dispose();
            asyncResultData.Dispose();
        }
    }
}