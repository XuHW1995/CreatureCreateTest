using UnityEngine;

public static class MathHelper
{
    /// <summary>
    /// 获取关于平面的对称点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="referencePlane"></param>
    /// <returns></returns>
    public static Vector3 GetFlippedPointWithPlane(Vector3 point, Plane referencePlane)
    {
        Ray r = new Ray();
        if (referencePlane.GetSide(point))
        {
            r.origin = point;
            r.direction = -referencePlane.normal;
        }
        else
        {
            r.origin = point;
            r.direction = referencePlane.normal;
        }

        if (referencePlane.Raycast(r, out float enter))
        {
            return r.GetPoint(enter * 2);
        }
        else
        {
            Debug.LogError($"计算错误，点{point} 相对 面 {referencePlane} 无对称点");
            return Vector3.negativeInfinity;
        }
    }
    
    /// <summary>
    /// 获取关于平面的对称向量
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="referencePlane"></param>
    /// <returns></returns>
    public static Vector3 GetFlippedVectorWithPlane(Vector3 vector, Plane referencePlane)
    {
        return vector - 2 * Vector3.Dot(vector, referencePlane.normal) * referencePlane.normal;
    } 
}
