﻿using UnityEngine;

public class Follower : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform follow;
    [SerializeField] private bool fixedUpdate = false;

    [Header("Position")]
    [SerializeField] private bool followPosition = true;
    [SerializeField] private float positionSmoothing = -1f;
    [SerializeField] private Vector3Int followAxes = Vector3Int.one;
    [SerializeField] private Vector3 fixedPosition = Vector3.one * Mathf.Infinity;

    [Header("Rotation")]
    [SerializeField] private bool followRotation = true;
    [SerializeField] private float rotationSmoothing = -1f;

    private Vector3 offsetPosition;
    private Quaternion offsetRotation;
    #endregion

    #region Methods
    private void Start()
    {
        SetFollow(follow);
    }
    private void LateUpdate()
    {
        if (!fixedUpdate) { Follow(); }
    }
    private void FixedUpdate()
    {
        if (fixedUpdate) { Follow(); }
    }

    private void Follow()
    {
        if (!follow) return;

        if (followPosition)
        {
            #region Clamp
            Vector3 targetPosition = follow.position;

            if (fixedPosition.x != Mathf.Infinity) { targetPosition.x = fixedPosition.x; }
            if (fixedPosition.y != Mathf.Infinity) { targetPosition.y = fixedPosition.y; }
            if (fixedPosition.z != Mathf.Infinity) { targetPosition.z = fixedPosition.z; }
            #endregion

            #region Axes
            if (followAxes.x == 0) { targetPosition.x = transform.position.x; }
            if (followAxes.y == 0) { targetPosition.y = transform.position.y; }
            if (followAxes.z == 0) { targetPosition.z = transform.position.z; }
            #endregion

            transform.position = (positionSmoothing == -1f) ?
                transform.position = targetPosition - offsetPosition :
                Vector3.Lerp(transform.position, targetPosition - offsetPosition, Time.deltaTime * positionSmoothing);
        }
        if (followRotation)
        {
            transform.rotation = (rotationSmoothing == -1f) ?
                Quaternion.Euler(follow.rotation.eulerAngles - offsetRotation.eulerAngles) :
                Quaternion.Slerp(transform.rotation, Quaternion.Euler(follow.rotation.eulerAngles - offsetRotation.eulerAngles), Time.deltaTime * rotationSmoothing);
        }
    }

    public void SetFollow(Transform follow)
    {
        if (!follow) return;

        this.follow = follow;

        offsetPosition = follow.position - transform.position;
        offsetRotation = Quaternion.Euler(follow.rotation.eulerAngles - transform.rotation.eulerAngles);
    }
    #endregion
}