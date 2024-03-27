using System;
using UnityEngine;  
  
public class PlaneGizmo : MonoBehaviour  
{  
    public Vector3 normal; // 平面的法线向量  
    public float distance; // 原点到平面的距离  
    public Color gizmoColor = Color.blue; // Gizmos的颜色  

    public Vector3 checkPoint;
    
    private Vector3 GetFlippedVectorWithPlane(Vector3 point, Plane referencePlane)
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
    
    // void OnDrawGizmos()  
    // {  
    //     Gizmos.color = gizmoColor;  
    //
    //     // 计算平面上的四个点，用于绘制一个矩形表示平面  
    //     Vector3 right = Vector3.Cross(normal, Vector3.up).normalized * 1.0f; // 假设平面不是水平的  
    //     Vector3 forward = Vector3.Cross(normal, right).normalized * 1.0f;  
    //
    //     Vector3 centerPoint = transform.position - normal * distance; // 平面的中心点  
    //
    //     // 绘制平面的四个角点  
    //     Gizmos.DrawCube(centerPoint, new Vector3(right.magnitude * 2, forward.magnitude * 2, normal.magnitude * 0.01f));  
    //
    //     // 绘制平面的法线  
    //     Gizmos.DrawLine(centerPoint, centerPoint + normal * 2);  
    // }  
    

        // void OnDrawGizmos()
        // {
        //     // 定义平面大小
        //     Vector3 planeSize = new Vector3(10, 1, 10);
        //
        //     // 定义平面的中心位置（相对于此脚本所附加的游戏对象）
        //     Vector3 planeCenter = Vector3.zero;
        //
        //     // 绘制实心正方体（可选）
        //     // Gizmos.DrawCube(planeCenter, planeSize);
        //
        //     // 绘制正方体的线框（绘制平面）
        //     Gizmos.DrawWireCube(planeCenter, planeSize);
        // }

        // public Vector3 dragPlaneNormal;
        // public Vector3 DragPlanePosition = Vector3.zero;
        // void OnDrawGizmos(){
        //     Quaternion rotation = Quaternion.LookRotation(transform.TransformDirection(dragPlaneNormal.normalized));
        //     Matrix4x4 trs = Matrix4x4.TRS(transform.TransformPoint(DragPlanePosition), rotation, Vector3.one);
        //     Gizmos.matrix = trs;
        //     Color32 color = Color.blue;
        //     color.a = 125;
        //     Gizmos.color = color;
        //     Gizmos.DrawCube(Vector3.zero, new Vector3(1.0f, 1.0f, 0.0001f));
        //     Gizmos.matrix = Matrix4x4.identity;
        //     Gizmos.color = Color.white;
        // }
        
        public void OnDrawGizmos()
        {
            Vector3 planeCenter = normal.normalized * distance;
            DrawPlane(planeCenter, normal.normalized);

            Vector3 flippedPPoint = GetFlippedVectorWithPlane(checkPoint, new Plane(normal.normalized, distance));
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(checkPoint, 0.5f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(flippedPPoint, 0.5f);
        }

        public void DrawPlane(Vector3 position, Vector3 normal)
        {
            Vector3 v3;

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, Color.green);
            Debug.DrawLine(corner1, corner3, Color.green);
            Debug.DrawLine(corner0, corner1, Color.green);
            Debug.DrawLine(corner1, corner2, Color.green);
            Debug.DrawLine(corner2, corner3, Color.green);
            Debug.DrawLine(corner3, corner0, Color.green);
            Debug.DrawRay(position, normal, Color.red);
        }
}