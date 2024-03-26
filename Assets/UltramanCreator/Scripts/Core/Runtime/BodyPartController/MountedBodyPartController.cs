using System;
using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

namespace TestGon.BodyPartController
{
    public class MountedBodyPartController : UltramanBodyPartController
    {
        public GameObject flipped;
        private Ray gizmosRay = new Ray();

        public void Start()
        {
            base.Start();
            
            //Flipped = Instantiate(this.gameObject, Dynamic.Transform).GetComponent<DanielLochner.Assets.CreatureCreator.BodyPartController>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gizmosRay.origin, gizmosRay.origin + gizmosRay.direction * 100f);
        }

        public override void Init()
        {
            Drag.OnPress.AddListener(OnDragPress);
            Drag.OnRelease.AddListener(OnDragRelease);
            Drag.OnDrag.AddListener(OnDraging);
        }

        public override void OnDragPress()
        {
            CreatureCreator.Instance.CameraOrbit.Freeze();
            UltramanCreature.Instance.AllDynamicMountBonesSlotsShow(true);
            
            transform.SetParent(Dynamic.Transform);
            gameObject.SetLayerRecursively(LayerManager.IGNORE_RAYCAST_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});

            if (flipped != null)
            {
                Destroy(flipped);
            }
            
            flipped = GameObject.Instantiate(this.gameObject, Dynamic.Transform);
            
            //TODO 反转
            flipped.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            flipped.transform.SetParent(Dynamic.Transform);
            flipped.gameObject.SetLayerRecursively(LayerManager.IGNORE_RAYCAST_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});
        }
        
        public override void OnDraging()
        {
            base.OnDraging();
            
            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            gizmosRay = checkRay;
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            if (isHit)
            {
                Drag.Draggable = false;

                transform.position = raycastHit.point;
                transform.rotation = Quaternion.LookRotation(raycastHit.normal);

                //TODO 对称标记，构建对称面，计算对称点（现在的算法是基于挂载部位X严格处于X = 0处，，实际应该取挂载部件 的对称面
                if (Mathf.Abs(transform.position.x) > 0.1f)
                {
                    flipped.SetActive(true);
                    flipped.transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
                    flipped.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.z);
                }
                else
                {
                    flipped.gameObject.SetActive(false);
                }
            }
            else
            {
                Drag.Draggable = true;
                flipped.gameObject.SetActive(false);
            }

            CheckRayCastResult();
        }

        public override void OnDragRelease()
        {
            base.OnDragRelease();
            
            CreatureCreator.Instance.CameraOrbit.Unfreeze();

            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            //挂载吸附
            if (isHit)
            {
                Transform mountedParentBone = FindNearestBone(raycastHit.point);
                Transform flippedParent = FindNearestBone(flipped.transform.position);
                
                
                transform.SetParent(mountedParentBone);
                //transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(raycastHit.normal);
                gameObject.SetLayerRecursively(LayerManager.BODY_PART_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});

                if (flipped)
                {
                    //transform.localPosition = Vector3.zero;
                    flipped.transform.SetParent(flippedParent);
                    flipped.gameObject.SetLayerRecursively(LayerManager.BODY_PART_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});
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
        }

        private Transform FindNearestBone(Vector3 point)
        {
            float minDisSqr = float.MaxValue;
            Transform nearestBone = null;
            foreach (var oneBone in  UltramanCreature.Instance.BoneRootTransform.GetComponentsInChildren<Transform>())
            {
                float disSqr = Vector3.SqrMagnitude(point - oneBone.position);
                if (minDisSqr > disSqr)
                {
                    minDisSqr = disSqr;
                    nearestBone = oneBone;
                }
            }

            return nearestBone;
        }
        
        private void CheckRayCastResult()
        {
            Ray checkRay =
                RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerMask.GetMask("BodyPart"));
            if (isHit)
            {
                Debug.Log($"XHW 射线检测结果 {isHit}, 对象 = {raycastHit.collider.name}, ");
            }
            else
            {
                Debug.Log("未检测到");
            }
        }
    }
}