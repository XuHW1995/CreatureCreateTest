using System;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

namespace TestGon.BodyPartController
{
    public class UltramanCreature : MonoBehaviour
    {
        private static UltramanCreature _instance;
        public static UltramanCreature Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                return null;
            }
        }

        [SerializeField]
        public Transform BoneRootTransform;
        [SerializeField]
        public Animator animator;
        [SerializeField]
        public Transform[] bodyPartsTransforms;

        //对称平面
        private Plane flippedPlane;
        public Plane FlippedPlane
        {
            get
            {
                return flippedPlane;
            }
        }
        public void Start()
        {
            _instance = this;
            InitBodyPartClick();
            InitDynamicMountBones();

            flippedPlane = new Plane(this.transform.right, this.transform.position);
        }

        [ContextMenu("ChangeflippedPlane")]
        public void ChangeflippedPlane()
        {
            flippedPlane = new Plane(this.transform.right, this.transform.position);
        }
        
        public void DebugTestAnimation(string animationName)
        {
            animator.speed = 1;
            animator.Play(animationName);
        }

        public void StopAnimation()
        {
            animator.speed = 0;
        }
        
        public Transform selectBodyPart;
        public void InitBodyPartClick()
        {
            foreach (Transform bodyPartTransform in bodyPartsTransforms)
            {
                Click click = bodyPartTransform.GetComponent<Click>();
                click.OnClick.AddListener((() =>
                {
                    selectBodyPart = bodyPartTransform;
                    //bodyPartTransform.GetComponent<Outline>().enabled = true;
                }));
            }
        }
        
        [SerializeField] private ColourPicker primaryColourPicker;
        public void OnColorChange()
        {
            if (selectBodyPart != null)
            {
                selectBodyPart.GetComponent<SkinnedMeshRenderer>().material.color = primaryColourPicker.Colour;
            }
        }
        
        [SerializeField] 
        public Transform[] dynamicMountBonesSlots;
        public Mesh dynamicMountBoneMesh;
        
        public void AllDynamicMountBonesSlotsShow(bool show)
        {
            foreach (Transform dynamicMountBonesSlot in dynamicMountBonesSlots)
            {
                dynamicMountBonesSlot.GetComponent<Outline>().enabled = show;
                dynamicMountBonesSlot.GetComponent<MeshRenderer>().enabled = show;
            }
        }
        
        private void InitDynamicMountBones()
        {
            foreach (Transform dynamicMountBonesSlot in dynamicMountBonesSlots)
            {
                dynamicMountBonesSlot.gameObject.tag = "UTM_DMBL";
                dynamicMountBonesSlot.gameObject.AddComponent<SphereCollider>();
                MeshRenderer mr = dynamicMountBonesSlot.gameObject.AddComponent<MeshRenderer>();
                mr.sharedMaterial = new Material(Shader.Find("Standard"));
                mr.enabled = false;
                dynamicMountBonesSlot.gameObject.AddComponent<MeshFilter>().sharedMesh = dynamicMountBoneMesh;
                dynamicMountBonesSlot.gameObject.AddComponent<Outline>().OutlineColor = Color.black;
                dynamicMountBonesSlot.gameObject.layer = LayerMask.NameToLayer("BoneSlots");
                // Hover hover = dynamicMountBonesSlot.gameObject.AddComponent<Hover>();
                // hover.OnEnter.AddListener((() =>
                // {
                //     dynamicMountBonesSlot.GetComponent<Outline>().enabled = true;
                //     mr.enabled = true;
                // }));
                //
                // hover.OnExit.AddListener((() =>
                // {
                //     dynamicMountBonesSlot.GetComponent<Outline>().enabled = false;
                //     mr.enabled = false;
                // }));
            }
        }
    }
}