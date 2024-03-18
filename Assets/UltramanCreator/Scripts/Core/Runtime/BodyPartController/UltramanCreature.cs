using System;
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
        
        public void Start()
        {
            _instance = this;
        }
    }
}