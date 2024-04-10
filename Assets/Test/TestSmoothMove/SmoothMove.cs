using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public Transform targetPosition; // 目标位置
    public float dampingCoefficient = 2f; // 阻尼系数

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        // 计算朝向目标位置的方向向量
        Vector3 direction = targetPosition.position - transform.position;

        // 计算朝向目标位置的速度向量，根据临界阻尼模型进行平滑运动
        Vector3 targetVelocity = direction * dampingCoefficient;

        // 使用 SmoothDamp 方法平滑地调整物体的位置
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, dampingCoefficient);

        // 如果物体接近目标位置，直接设置到目标位置并停止运动
        if (direction.magnitude < 0.01f)
        {
            transform.position = targetPosition.position;
            velocity = Vector3.zero;
        }
    }
}