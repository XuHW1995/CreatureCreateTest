﻿// Creature Creator - https://github.com/daniellochner/SPORE-Creature-Creator
// Version: 1.0.0
// Author: Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartController : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected BodyPart bodyPart;

        [Header("Body Part")]
        [SerializeField] private Transform model;
        [SerializeField] private GameObject pivotPrefab;
        [SerializeField] private GameObject rotatePrefab;

        protected Hover hover;
        protected Scroll scroll;

        private GameObject pivotGO;
        #endregion

        #region Properties
        public AttachedBodyPart AttachedBodyPart { get; set; }
        public BodyPartController Flipped { get; set; }
        public Drag Drag { get; set; }

        public Transform Model { get { return model; } }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Drag = GetComponent<Drag>();

            hover = GetComponent<Hover>();
            scroll = GetComponent<Scroll>();
        }
        protected virtual void Start()
        {
            hover.OnEnter.AddListener(delegate
            {
                Debug.Log($"XHW {gameObject.name} hover.onenter ");
                if (!Input.GetMouseButton(0))
                {
                    CreatureCreator.Instance.CameraOrbit.Freeze();
                }
            });
            hover.OnExit.AddListener(delegate
            {
                Debug.Log($"XHW {gameObject.name} hover.OnExit ");
                if (!Input.GetMouseButton(0))
                {
                    CreatureCreator.Instance.CameraOrbit.Unfreeze();
                }
            });

            scroll.OnScrollUp.AddListener(delegate
            {
                if (transform.localScale.y < bodyPart.MaxScale - bodyPart.ScaleIncrement)
                {
                    transform.localScale += Vector3.one * bodyPart.ScaleIncrement;
                    if (Flipped != null)
                    {
                        Flipped.transform.localScale = transform.localScale;
                    }
                }
            });
            scroll.OnScrollDown.AddListener(delegate
            {
                if (transform.localScale.y > bodyPart.MinScale + bodyPart.ScaleIncrement)
                {
                    transform.localScale -= Vector3.one * bodyPart.ScaleIncrement;
                    if (Flipped != null)
                    {
                        Flipped.transform.localScale = transform.localScale;
                    }
                }
            });
        }
        #endregion
    }
}