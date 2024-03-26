using System;
using UnityEngine;

namespace TestGon
{
    public class DebugSkinnMeshBones : MonoBehaviour
    {
        [Serializable]
        public struct BoneInfo
        {
            public string boneName;
            public Transform boneTransform;
            public Matrix4x4 bindPose;
        }    
     
        [SerializeField]
        public BoneInfo[] boneInfos;
        
        [ContextMenu("输出当前SkinMeshRender关联的骨骼")]
        public void PrintSkinnMeshBones()
        {
            SkinnedMeshRenderer sr = GetComponent<SkinnedMeshRenderer>();
            if (sr == null)
            {
                Debug.LogError("没有SkinnedMeshRenderer组件");
                return;
            }
            
            boneInfos = new BoneInfo[sr.bones.Length];
            for (int i = 0; i < boneInfos.Length; i ++)
            {
                Debug.Log(sr.bones[i].name);
                boneInfos[i].boneName = sr.bones[i].name;
                boneInfos[i].boneTransform = sr.bones[i];
                boneInfos[i].bindPose = sr.sharedMesh.bindposes[i];
            }
        }
        
        [ContextMenu("输出当前SkinMeshRender 中 Mesh的绑定pose")]
        public void PrintBindPose()
        {
            SkinnedMeshRenderer sr = GetComponent<SkinnedMeshRenderer>();
            Mesh targetMesh = sr.sharedMesh;
            for (int i = 0; i < targetMesh.bindposes.Length; i++)
            {
                Matrix4x4 bindPose = targetMesh.bindposes[i];  
                string outputString = $"Bind Pose for Bone {i}  Bone name = {sr.bones[i].name}:" + "\n"
                    + $" Matrix4x4: {bindPose}" + "\n"
                    + $"Position: {bindPose.GetColumn(3)}" + "\n" 
                    + $"Rotation eulerangles: {bindPose.rotation.eulerAngles}" + "\n"
                    + $"Scale: {bindPose.lossyScale}";
                
                Debug.Log(outputString);
            }
        }

        [SerializeField]
        public Transform aBoneRootTransform;
        [SerializeField]
        public SkinnedMeshRenderer aSR;
        [SerializeField]
        public SkinnedMeshRenderer bSR;
        [ContextMenu("切换ASR 改为 BSR, aBoneRootTransform 为 aSR的骨骼根节点")]
        private void ExchangeSR()
        {
            int bBoneCount = bSR.bones.Length;
            Transform[] tempABones = new Transform[bBoneCount];
            
            for (int i = 0; i < bBoneCount; i++)
            {
                //从当前的骨骼中找到B的骨骼
                Transform changeBone = FindBoneFromRoot(aBoneRootTransform, bSR.bones[i].name);
                
                if (changeBone != null)
                {
                    tempABones[i] = changeBone;
                    Debug.Log($"替换后 ASR的骨骼{i} name = {bSR.bones[i].name}");
                }
                else
                {
                    Debug.LogError($"替换ASR的骨骼{i} 为 {bSR.bones[i].name}" + $"但是在aBoneRootTransform {aBoneRootTransform.name} 中没有找到名为 {bSR.bones[i].name}的骨骼");
                }
            }

            aSR.bones = tempABones;
            aSR.sharedMesh = bSR.sharedMesh;
            aSR.sharedMaterials = bSR.sharedMaterials;
        }
        
        private Transform FindBone(Transform[] bones, string name)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                if (bones[i].name == name)
                {
                    Debug.Log($"找到了 {name} 在 {i}");
                    return bones[i];
                }
            }

            return null;
        }

        private static Transform FindBoneFromRoot(Transform rootBone, string findBoneName)
        {
            if (rootBone.name == findBoneName)
            {
                return rootBone;
            }
            
            for (int i = 0; i < rootBone.childCount; i++)
            {
                Transform child = rootBone.GetChild(i);
                Transform findBone = FindBoneFromRoot(child, findBoneName);
                if (findBone != null)
                {
                    return findBone;
                }
            }

            return null;
        }
        
        public Mesh a;
        public Mesh b;
    
        [ContextMenu("检测A B bindpose是否一致")]
        public void CheckABBindPos()
        {
            for (int i = 0; i < a.bindposes.Length; i++)
            {
                if (a.bindposes[i] != b.bindposes[i])
                {
                    Debug.Log($"A 和 B 不一样的 bindpose {i}");
                }
            }
        }
        
        public static void ExchangeSR(SkinnedMeshRenderer aSR, SkinnedMeshRenderer bSR, Transform aBoneRootTransform)
        {
            int bBoneCount = bSR.bones.Length;
            Transform[] tempABones = new Transform[bBoneCount];
            
            for (int i = 0; i < bBoneCount; i++)
            {
                //从当前的骨骼中找到B的骨骼
                Transform changeBone = FindBoneFromRoot(aBoneRootTransform, bSR.bones[i].name);
                
                if (changeBone != null)
                {
                    tempABones[i] = changeBone;
                    Debug.Log($"替换后 ASR的骨骼{i} name = {bSR.bones[i].name}");
                }
                else
                {
                    Debug.LogError($"替换ASR的骨骼{i} 为 {bSR.bones[i].name}" + $"但是在aBoneRootTransform {aBoneRootTransform.name} 中没有找到名为 {bSR.bones[i].name}的骨骼");
                }
            }

            aSR.bones = tempABones;
            aSR.sharedMesh = bSR.sharedMesh;
            aSR.sharedMaterials = bSR.sharedMaterials;
        }
    }
}