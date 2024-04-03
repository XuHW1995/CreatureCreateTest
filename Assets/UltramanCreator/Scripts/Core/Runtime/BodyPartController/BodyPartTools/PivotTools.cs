using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

public class PivotTools : ToolBase
{
    private Vector3 upVector = Vector3.up;
    
    public void Start()
    {
        base.Start();
    }
    
    public override void OnDragPress()
    {
        CreatureCreator.Instance.CameraOrbit.Freeze();
        
        upVector = _mountedBodyPartController.transform.up;
    }
    
    public override void OnDraging()
    {
        _mountedBodyPartController.transform.LookAt(transform, upVector);
    }
    
    public override void OnDragRelease()
    {
        CreatureCreator.Instance.CameraOrbit.Unfreeze();
    }
}