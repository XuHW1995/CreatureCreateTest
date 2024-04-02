using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

public class PivotTools : ToolBase
{
    public void Start()
    {
        base.Start();
    }
    
    public override void OnDragPress()
    {
        CreatureCreator.Instance.CameraOrbit.Freeze();
    }
    
    public override void OnDraging()
    {
        Vector3 forward = transform.position - _mountedBodyPartController.transform.position;
        Quaternion rotation = Quaternion.LookRotation(forward);
        
        _mountedBodyPartController.transform.rotation = rotation;
    }
    
    public override void OnDragRelease()
    {
        CreatureCreator.Instance.CameraOrbit.Unfreeze();
    }
}