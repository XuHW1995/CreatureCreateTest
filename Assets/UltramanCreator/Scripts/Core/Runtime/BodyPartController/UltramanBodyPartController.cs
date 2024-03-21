using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using TestGon;
using TestGon.BodyPartController;
using UnityEngine;
using UnityEngine.Events;

public enum EUltramanBodyPartType
{
    exchange,
    mount,
}

public class UltramanBodyPartController : BodyPartController
{
    [SerializeField]
    public SkinnedMeshRenderer sr;
    [SerializeField]
    public EUltramanBodyPartType type = EUltramanBodyPartType.exchange;

    public GameObject flipped;
    
    public void Start()
    {
        base.Start();
    }

    public void InitBPC()
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
                out RaycastHit raycastHit) && (raycastHit.collider.CompareTag("UTM")))
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
        
        if (type == EUltramanBodyPartType.exchange)
        {
            Drag.OnPress.AddListener(DragOnPress);
            Drag.OnRelease.AddListener(DragOnRelease);
            Drag.OnDrag.AddListener(Draging);
        }
        else if (type == EUltramanBodyPartType.mount)
        {
            Drag.OnRelease.AddListener(MountParyDragOnRelease);
            Drag.OnDrag.AddListener(MountParyDraging);
            Drag.OnPress.AddListener(MountParyDragOnPress);
        }
    }
    
    public void MountParyDragOnPress()
    {
        CreatureCreator.Instance.CameraOrbit.Freeze();
        transform.SetParent(Dynamic.Transform);
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"), new List<string> {"Tools"});
        UltramanCreature.Instance.AllDynamicMountBonesSlotsShow(true);
        
        if (type == EUltramanBodyPartType.mount)
        {
            flipped = GameObject.Instantiate(this.gameObject, Dynamic.Transform);
            flipped.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        
        if (flipped)
        {
            flipped.transform.SetParent(Dynamic.Transform);
            flipped.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"), new List<string> {"Tools"});
        }
    }
    
    public void MountParyDraging()
    {
        if (Physics.Raycast(
            RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition),
            out RaycastHit raycastHit) && raycastHit.collider.CompareTag("UTM_DMBL"))
        {
            Drag.Draggable = false;

            transform.position = raycastHit.point;
            transform.rotation = Quaternion.LookRotation(raycastHit.normal);

            //TODO 对称标记，构建对称面，计算对称点（现在的算法是基于挂载部位X严格处于X = 0处，，实际应该取挂载部件 的对称面
            if (Mathf.Abs(transform.position.x) > 0.1f)
            {
                if (flipped)
                {
                    flipped.SetActive(true);
                    flipped.transform.position =
                        new Vector3(-transform.position.x, transform.position.y, transform.position.z);
                    flipped.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.z);
                }
            }
            else
            {
                if (flipped)
                {
                    flipped.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Drag.Draggable = true;
            if (flipped)
            {
                flipped.gameObject.SetActive(false);
            }
        }
    }
    
    public void MountParyDragOnRelease()
    {
        CreatureCreator.Instance.CameraOrbit.Unfreeze();

        Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
        bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerMask.GetMask("BoneSlots"));
         //挂载吸附
         if (isHit && raycastHit.collider.CompareTag("UTM_DMBL"))
         {
             transform.SetParent(raycastHit.collider.transform);
             transform.localPosition = Vector3.zero;
             //transform.rotation =   Quaternion.LookRotation(raycastHit.normal);
             gameObject.SetLayerRecursively(LayerMask.NameToLayer("Body"), new List<string> {"Tools"});
             
             if (flipped)
             {
                 transform.localPosition = Vector3.zero;
                 flipped.transform.SetParent(raycastHit.collider.transform);
                 flipped.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Body"), new List<string> {"Tools"});
             }
         }
         else
         {
             Destroy(gameObject);
             if (flipped)
             {
                Destroy(flipped);
             }
             
             Drag.Plane = new Plane(Vector3.right, Vector3.zero);
         }
        
         UltramanCreature.Instance.AllDynamicMountBonesSlotsShow(false);
        // RaycastHit[] raycastHits = Physics.RaycastAll(checkRay, 100);
        // bool isHit = false;
        // foreach (var oneRaycastHit in raycastHits)
        // {
        //     if (oneRaycastHit.collider.gameObject.layer == LayerMask.NameToLayer("BoneSlots"))
        //     {
        //         transform.SetParent(oneRaycastHit.collider.transform);
        //         transform.localPosition = Vector3.zero;
        //         transform.rotation = Quaternion.LookRotation(oneRaycastHit.normal);
        //         isHit = true;
        //         break;
        //     }
        // }
        //
        // if (!isHit)
        // {
        //     Destroy(gameObject);
        // }
    }
}
