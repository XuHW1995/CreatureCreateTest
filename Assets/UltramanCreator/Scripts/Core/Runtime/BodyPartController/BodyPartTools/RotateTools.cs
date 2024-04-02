using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

public class RotateTools : ToolBase
{
    //缓存从中心点到拖动点的方向
    private Vector3 cacheCenterToRayCastPointDir;
    public float rotateSpeed = 10f;
    
    public void Start()
    {
        base.Start();
    }
    
    public override void OnDragPress()
    {
        CreatureCreator.Instance.CameraOrbit.Freeze();
        
        cacheCenterToRayCastPointDir = (drag.CurDragWorldPosition - transform.position).normalized;
    }
    
    public override void OnDraging()
    {
        Vector3 curDir = (drag.CurDragWorldPosition - transform.position).normalized;
        //当前拖动碰撞点和上一次的做对比
        float offsetAngle = Vector3.SignedAngle(curDir, cacheCenterToRayCastPointDir, drag.Plane.normal);
        //停止拖动则不旋转
        if (Mathf.Abs(offsetAngle) > 1f)
        {
            _mountedBodyPartController.transform.Rotate(Vector3.forward,  Mathf.Sign(offsetAngle) * rotateSpeed, Space.Self);
            cacheCenterToRayCastPointDir = curDir;
        }
    }
    
    public override void OnDragRelease()
    {
        CreatureCreator.Instance.CameraOrbit.Unfreeze();
    }
}