using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SharedData {  
    public int count = 0;  
}  

public class TestThread : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SharedData data = new SharedData(); 
        
        Thread a = new Thread((o =>
        {
            for (int i = 0; i < 1000; i++)
            {
                data.count++; // 可能导致数据竞争  
            }
        }));  
        
        Thread b = new Thread((o =>
        {
            for (int i = 0; i < 1000; i++)
            {
                data.count++; // 可能导致数据竞争  
            }
        }));  
        
        a.Start();  
        b.Start();

        a.Join(); 
        b.Join();  

        Debug.Log($"XHW Data count = {data.count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}