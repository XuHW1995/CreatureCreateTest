using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NBezier : MonoBehaviour
{
    public List<Transform> controlPoints;
    public int numberOfPoints = 50; // 确定贝塞尔曲线上的点数
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;
    }

    // 根据t值计算N阶贝塞尔曲线上的点
    public Vector3 CalculateBezierPoint(List<Transform> points, float t)
    {
        int n = points.Count - 1;
        Vector3 point = Vector3.zero;
        for (int i = 0; i < points.Count; i++)
        {
            float basis = BinomialCoefficient(n, i) * Mathf.Pow((1 - t), (n - i)) * Mathf.Pow(t, i);
            point += points[i].position * basis;
        }
        return point;
    }

    private int BinomialCoefficient(int n, int k)
    {
        int result = 1;
        for (int i = 1; i <= k; i++)
        {
            result *= (n - (k - i));
            result /= i;
        }
        return result;
    }

    void Update()
    {
        // 根据t值在曲线上创建点
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            Vector3 point = CalculateBezierPoint(controlPoints, t);
            lineRenderer.SetPosition(i, point);
        }
    }
}