using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
  
public class TestBezier : MonoBehaviour  
{  
    public Vector3 p0; // 起点  
    public Vector3 p1; // 控制点  
    public Vector3 p2; // 终点  
    public int segments = 100; // 曲线分段数  
    public float lineWidth = 0.1f; // 线条宽度  
    private LineRenderer lineRenderer;  
  
    void Start()  
    {  
        // 创建LineRenderer组件  
        lineRenderer = gameObject.AddComponent<LineRenderer>();  
        lineRenderer.startWidth = lineWidth;  
        lineRenderer.endWidth = lineWidth;  
        lineRenderer.material = new Material(Shader.Find("Standard"));  

        DrawBezier();
    }  
  
    // 计算二次贝塞尔曲线上的点  
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)  
    {  
        float u = 1 - t;  
        float tt = t * t;  
        float uu = u * u;  
  
        // 二次贝塞尔曲线公式  
        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;  
        return point;  
    }

    private void DrawBezier()
    {
        lineRenderer.positionCount = segments - 1;
        // 绘制贝塞尔曲线  
        for (int i = 0; i <= segments; i++)  
        {  
            float t = (float)i / segments;  
            Vector3 pointOnCurve = CalculateBezierPoint(t, p0, p1, p2);  
            lineRenderer.SetPosition(i, pointOnCurve);  
        }  
    }
    
    void Update()  
    {  
        // 可以在这里添加代码以在运行时动态更新贝塞尔曲线的控制点
        DrawBezier();
    }
}
