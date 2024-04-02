using System;
using DanielLochner.Assets.CreatureCreator;
using TestGon.BodyPartController;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour
{
    protected MountedBodyPartController _mountedBodyPartController;
    protected Drag drag;
    protected Hover hover;
    public void Start()
    {
        _mountedBodyPartController = GetComponentInParent<MountedBodyPartController>();
        drag = Utility.GetorAddComponent<Drag>(this.gameObject);
        hover = Utility.GetorAddComponent<Hover>(this.gameObject);
        
        drag.OnDrag.AddListener(OnDraging);
        drag.OnPress.AddListener(OnDragPress);
        drag.OnRelease.AddListener(OnDragRelease);
        
        hover.OnEnter.AddListener(OnHoverEnter);
        hover.OnExit.AddListener(OnHoverExit);
    }

    public void OnHoverEnter()
    {
        if (!Input.GetMouseButton(0))
        {
            CreatureCreator.Instance.CameraOrbit.Freeze();
        }
    }
    
    public void OnHoverExit()
    {
        if (!Input.GetMouseButton(0))
        {
            CreatureCreator.Instance.CameraOrbit.Unfreeze();
        }
    }

    public abstract void OnDragPress();
    public abstract void OnDraging();
    public abstract void OnDragRelease();
}