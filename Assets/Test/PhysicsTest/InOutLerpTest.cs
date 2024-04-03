using UnityEngine;

public class InOutLerpTest : MonoBehaviour
{
    public enum LerpType
    {
        interpolate,
        extrapolate
    }
    
    public Transform startPoint;
    public Transform endPoint;
    public float duration = 2.0f;
    public float tMax = 0.5f; // 插值参数
    public LerpType lerpType = LerpType.interpolate;

    private float t = 0f;
    void Update()
    {
        t += Time.deltaTime / duration;
        if (t > tMax)
        {
            t = 0f;
            Transform temp = endPoint;
            endPoint = startPoint;
            startPoint = temp;
        }

        Vector3 tempPosition = Vector3.zero;
        if (lerpType == LerpType.interpolate)
        {
            // 内插值算法
            tempPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
            
        }
        else if (lerpType == LerpType.extrapolate)
        {
            // 外插值算法
            tempPosition = Vector3.LerpUnclamped(startPoint.position, endPoint.position, t);
        }
        
        transform.position = tempPosition;
    }
}