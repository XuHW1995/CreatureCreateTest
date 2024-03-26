using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

namespace TestGon.BodyPartController
{
    public class ReplacementBodyPartController : UltramanBodyPartController
    {
        public void Start()
        {
            base.Start();
        }

        public override void Init()
        {
            Drag.OnPress.AddListener(OnDragPress);
            Drag.OnRelease.AddListener(OnDragRelease);
            Drag.OnDrag.AddListener(OnDraging);
        }

        public override void OnDragPress()
        {
            base.OnDragPress();
            
            CreatureCreator.Instance.CameraOrbit.Freeze();
            transform.SetParent(Dynamic.Transform);
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"), new List<string> {"Tools"});
        }
        
        public override void OnDraging()
        {
            base.OnDraging();
            
            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            if (isHit && raycastHit.collider.CompareTag(TagManager.REPLACE_PART))
            {
                Drag.Draggable = false;
                transform.position = raycastHit.point;
                //transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
            else
            {
                Drag.Draggable = true;
            }
        }
        
        public override void OnDragRelease()
        {
            base.OnDragRelease();
            
            CreatureCreator.Instance.CameraOrbit.Unfreeze();
            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            if (isHit && raycastHit.collider.CompareTag(TagManager.REPLACE_PART))
            {
                if (bodyPart.BodyPartType == raycastHit.collider.GetComponent<ReplcementPartFlag>().BodyPartTypeEnum)
                {
                    SkinnedMeshRenderer a = raycastHit.collider.GetComponent<SkinnedMeshRenderer>();
                    SkinnedMeshRenderer b = this.sr;
                    DebugSkinnMeshBones.ExchangeSR(a, this.sr, UltramanCreature.Instance.BoneRootTransform);
                }
            }
            
            Drag.Plane = new Plane(Vector3.right, Vector3.zero);
            Destroy(gameObject);
            gameObject.SetLayerRecursively(LayerManager.BODY_LAYER, new List<string> {"Tools"});
        }
    }
}