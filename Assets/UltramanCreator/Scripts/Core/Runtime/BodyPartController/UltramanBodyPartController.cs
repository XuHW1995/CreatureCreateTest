using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using TestGon.BodyPartController;
using UnityEngine;

public abstract class UltramanBodyPartController : BodyPartController
{
    [SerializeField]
    public SkinnedMeshRenderer sr;

    public void Start()
    {
        base.Start();
    }

    public abstract void Init();

    public virtual void OnDragPress()
    {
    }

    public virtual void OnDraging()
    {
    }
    
    public virtual void OnDragRelease()
    {
    }
    
    public virtual void SetUnselected()
    {
    }
}
