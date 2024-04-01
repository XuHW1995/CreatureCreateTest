using System;
using UnityEngine;  
  
public class AABBTest : MonoBehaviour  
{      
    public bool drawAABB = true;
    public bool drawOBB = true;
    public Mesh mesh; // 要计算包围盒的mesh  
    public Quaternion rotation = Quaternion.identity; // 可能的旋转  
  
    Bounds aabb; // 包围盒  
    Bounds obb; // 轴对齐包围盒  
    [ContextMenu("Calculate AABB and OBB")]
    void Start()  
    {  
        if (mesh != null)  
        {  
            aabb = CalculateAABB(mesh);  
            obb = CalculateOBB(mesh, rotation);  
  
            Debug.Log("AABB: Center=" + aabb.center + ", Size=" + aabb.size);  
            Debug.Log("OBB: Center=" + obb.center + ", Size=" + obb.size);  
        }  
        else  
        {  
            Debug.LogError("Mesh is null! Please assign a mesh to calculate its bounding boxes.");  
        }  
    }
    
    public void OnDrawGizmos()
    {
        if (drawAABB)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(aabb.center, aabb.size);
        }

        if (drawOBB)
        {
            Gizmos.color = Color.blue;
         Gizmos.DrawWireCube(obb.center, obb.size);
        }
    }

    Bounds CalculateAABB(Mesh mesh)  
    {  
        // Unity的Mesh类已经提供了bounds属性，直接返回即可  
        return mesh.bounds;  
    }  
  
    Bounds CalculateOBB(Mesh mesh, Quaternion rotation)  
    {  
        // 首先获取mesh的所有顶点  
        Vector3[] vertices = mesh.vertices;  
  
        // 应用旋转  
        Vector3[] rotatedVertices = new Vector3[vertices.Length];  
        for (int i = 0; i < vertices.Length; i++)  
        {  
            rotatedVertices[i] = rotation * vertices[i];  
        }  
  
        // 找到旋转后顶点的最小和最大坐标  
        Vector3 min = rotatedVertices[0];  
        Vector3 max = rotatedVertices[0];  
        for (int i = 1; i < rotatedVertices.Length; i++)  
        {  
            min = Vector3.Min(min, rotatedVertices[i]);  
            max = Vector3.Max(max, rotatedVertices[i]);  
        }  
  
        // 计算OBB的中心和大小  
        Vector3 center = (min + max) / 2;  
        Vector3 size = max - min;  
  
        // 创建一个新的Bounds对象来表示OBB  
        return new Bounds(center, size);  
    }  
}