using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

public class UltramanContorller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hover hover = gameObject.AddComponent<Hover>();
        hover.OnEnter.AddListener(delegate
        {
            if (!Input.GetMouseButton(0))
            {
                CreatureCreator.Instance.CameraOrbit.Freeze();
                //SetBonesVisibility(true);
            }
        });
        hover.OnExit.AddListener(delegate
        {
            if (!Input.GetMouseButton(0))
            {
                CreatureCreator.Instance.CameraOrbit.Unfreeze();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
