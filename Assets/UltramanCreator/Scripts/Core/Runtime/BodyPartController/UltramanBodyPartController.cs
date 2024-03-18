using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using TestGon;
using TestGon.BodyPartController;
using UnityEngine;
using UnityEngine.Events;

public class UltramanBodyPartController : BodyPartController
{
    [SerializeField]
    public SkinnedMeshRenderer sr;
    
    public void Start()
    {
        UnityAction DragOnPress = delegate
        {
            CreatureCreator.Instance.CameraOrbit.Freeze();
            transform.SetParent(Dynamic.Transform);
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"), new List<string> {"Tools"});
        };

        UnityAction DragOnRelease = delegate
        {
            CreatureCreator.Instance.CameraOrbit.Unfreeze();

            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerMask.GetMask("BodyPart"));
            if (isHit && raycastHit.collider.CompareTag("UTM"))
            {
                SkinnedMeshRenderer a = raycastHit.collider.GetComponent<SkinnedMeshRenderer>();
                SkinnedMeshRenderer b = this.sr;
                //todo change
                DebugSkinnMeshBones.ExchangeSR(a, this.sr, UltramanCreature.Instance.BoneRootTransform);
            }

            Destroy(gameObject);
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Body"), new List<string> {"Tools"});
            Drag.Plane = new Plane(Vector3.right, Vector3.zero);
        };

        UnityAction Draging = delegate
        {
            if (Physics.Raycast(
                RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition),
                out RaycastHit raycastHit) && raycastHit.collider.CompareTag("UTM"))
            {
                Drag.Draggable = false;

                transform.position = raycastHit.point;
                transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
            else
            {
                Drag.Draggable = true;
            }
        };
        
        Drag.OnPress.AddListener(DragOnPress);
        Drag.OnRelease.AddListener(DragOnRelease);
        Drag.OnDrag.AddListener(Draging);
    }
}
