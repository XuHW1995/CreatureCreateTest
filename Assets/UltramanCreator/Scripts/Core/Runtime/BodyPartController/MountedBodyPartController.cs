using System;
using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

namespace TestGon.BodyPartController
{
    public class MountedBodyPartController : UltramanBodyPartController
    {
        [SerializeField]
        public MountedBodyPartController flipped;
        [SerializeField]
        public bool flippedable;

        private bool canFlipped
        {
            get
            {
                return flippedable && flipped != null;
            }
        }
#if UNITY_EDITOR
        private Ray gizmosRay = new Ray();
#endif
        public void Start()
        {
            base.Start();

            //拖动面以参考对象的点为基准
            Drag.Plane = new Plane(Camera.main.transform.forward, UltramanCreature.Instance.transform.position);
            if (flippedable && flipped == null)
            {
                flipped = Instantiate(this.gameObject, Dynamic.Transform).GetComponent<MountedBodyPartController>();
                flipped.flipped = this;
                flipped.flippedable = true;
                flipped.Drag.Plane = Drag.Plane;
                
                flipped.Drag.OnPress.AddListener(OnDragPress);
                flipped.Drag.OnRelease.AddListener(OnDragRelease);
                //flipped.Drag.OnDrag.AddListener(OnDraging);
                flipped.Drag.OnDrag.AddListener(OnFlippedDrag);
                flipped.Model.localScale = new Vector3(-Model.localScale.x, Model.localScale.y, Model.localScale.z);
            }
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

            transform.SetParent(Dynamic.Transform);
            gameObject.SetLayerRecursively(LayerManager.IGNORE_RAYCAST_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});

            if (canFlipped)
            {
                flipped.transform.SetParent(Dynamic.Transform);
                flipped.gameObject.SetLayerRecursively(LayerManager.IGNORE_RAYCAST_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});
            }
        }
        
        public override void OnDraging()
        {
            base.OnDraging();
            
            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
#if UNITY_EDITOR
            gizmosRay = checkRay;
#endif
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            if (isHit)
            {
                Drag.Draggable = false;

                transform.SetPositionAndRotation(raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                if (canFlipped)
                {
                    //接触点不在对称平面上则显示flipped
                    if (Mathf.Abs(UltramanCreature.Instance.FlippedPlane.GetDistanceToPoint(raycastHit.point)) > 0.01f)
                    {
                        flipped.gameObject.SetActive(true);
                        Vector3 flippedPos = MathHelper.GetFlippedPointWithPlane(raycastHit.point, UltramanCreature.Instance.FlippedPlane);
                        flipped.transform.position = flippedPos;

                        Vector3 flippedForward = MathHelper.GetFlippedVectorWithPlane(transform.forward, UltramanCreature.Instance.FlippedPlane);
                        
                        flipped.transform.rotation = Quaternion.LookRotation(flippedForward);
                    }
                    else
                    {
                        flipped.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Drag.Draggable = true;
                flipped?.gameObject.SetActive(false);
            }
#if UNITY_EDITOR
            CheckRayCastResult();
#endif
        }

        public void OnFlippedDrag()
        {
            //闭包内flipped持有的是clone,也就是反转对象， gameobject还是源对象
            Debug.Log($"XHW  OnFlippedDrag gameobject = {gameObject.name}, flipped = {flipped.name}");
            Ray checkRay = RectTransformUtility.ScreenPointToRay(CreatureCreator.Instance.CameraOrbit.Camera, Input.mousePosition);
        
            bool isHit = Physics.Raycast(checkRay, out RaycastHit raycastHit, 100, LayerManager.BODY_PART_LAYER_MASK);
            if (isHit)
            {
                flipped.Drag.Draggable = false;
                flipped.transform.SetPositionAndRotation(raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                
                if (canFlipped)
                {
                    //接触点不在对称平面上则显示flipped
                    if (Mathf.Abs(UltramanCreature.Instance.FlippedPlane.GetDistanceToPoint(raycastHit.point)) > 0.01f)
                    {
                        gameObject.SetActive(true);
                        Vector3 flippedPos = MathHelper.GetFlippedPointWithPlane(raycastHit.point, UltramanCreature.Instance.FlippedPlane);
                        transform.position = flippedPos;
                        
                        //Vector3 flippedForward = MathHelper.GetFlippedPointWithPlane(flipped.transform.forward, UltramanCreature.Instance.FlippedPlane);
                        Vector3 flippedForward = MathHelper.GetFlippedVectorWithPlane(flipped.transform.forward, UltramanCreature.Instance.FlippedPlane);
                        transform.rotation = Quaternion.LookRotation(flippedForward);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                flipped.Drag.Draggable = true;
                gameObject.SetActive(false);
            }
        
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
                gameObject.SetLayerRecursively(LayerManager.BODY_PART_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});

                if (canFlipped)
                {
                    flipped.transform.SetParent(flippedParent);
                    flipped.gameObject.SetLayerRecursively(LayerManager.BODY_PART_LAYER, new List<string> {LayerManager.TOOLS_LAYER_NAME});
                }
            }
            else
            {
                Destroy(gameObject);
                if (canFlipped)
                {
                    Destroy(flipped.gameObject);
                }
            }
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
                Debug.Log("射线检测未检测到碰撞体");
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gizmosRay.origin, gizmosRay.origin + gizmosRay.direction * 100f);
        }
#endif
    }
}