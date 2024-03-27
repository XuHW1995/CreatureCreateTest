using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AutoMoveAndRotate : MonoBehaviour
{
    public enum TestAxis
    {
        X,
        Y,
        Z,
    }

    #region move
    [SerializeField]
    public bool moveAble;
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    public float MoveLimit;
    [SerializeField]
    public TestAxis moveAxis;

    private Vector3 realMoveV;
    private Vector3 MoveV
    {
        get
        {
            Vector3 tempV = Vector3.zero;
            switch (moveAxis)
            {
                case TestAxis.X:
                    tempV = Vector3.right;
                    break;
                case TestAxis.Y: 
                    tempV = Vector3.up;
                    break;
                case TestAxis.Z:
                    tempV = Vector3.forward;
                    break;
            }
            
            return tempV;
        }
    }
    public float LimitAxisPositionValue
    {
        get
        {
            switch (moveAxis)
            {
                case TestAxis.X:
                    return transform.position.x;
                case TestAxis.Y:
                    return transform.position.y;
                case TestAxis.Z:
                    return transform.position.z;

            }

            return 0;
        }
    }
    #endregion

    #region rotate
    [SerializeField]
    public TestAxis rotateAxis;
    public Vector3 RotateAxis
    {
        get
        {
            switch (rotateAxis)
            {
                case TestAxis.X:
                    return Vector3.right;
                case TestAxis.Y:
                    return Vector3.up;
                case TestAxis.Z:
                    return Vector3.forward;

            }

            return Vector3.up;
        }
    }
    [SerializeField]
    public Space rotateAxisSpace;
    [SerializeField]
    public float rotateSpeed;
    [SerializeField]
    public bool rotateAble;
    #endregion

    #region rotateMove
    [SerializeField]
    public bool rotateMoveAble;
    [SerializeField]
    public Transform rotateMoveTarget;
    [SerializeField]
    public float rotateMoveSpeed;
    [SerializeField]
    public TestAxis rotateMoveAxis;

    private Vector3 rotateMoveTargetVector
    {
        get
        {
            switch (rotateMoveAxis)
            {
                case TestAxis.X:
                    return rotateMoveTarget.right;
                case TestAxis.Y:
                    return rotateMoveTarget.up;
                case TestAxis.Z:
                    return rotateMoveTarget.forward;

            }

            return rotateMoveTarget.up;
        }   
    }
    public 

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        realMoveV = MoveV;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dp = Vector3.zero;
        if (moveAble)
        {
            if (Mathf.Abs(LimitAxisPositionValue) > MoveLimit)
            {
                realMoveV *= -1;
            }
            dp += moveSpeed * realMoveV * Time.deltaTime;
        }

        if (rotateMoveAble)
        {
            Vector3 offset = transform.position + dp - rotateMoveTarget.transform.position;
            Vector3 rotateMoveV = Vector3.Cross(offset, rotateMoveTargetVector).normalized;

            dp += rotateMoveV * rotateMoveSpeed * Time.deltaTime;
            transform.LookAt(rotateMoveTarget);
        }
        
        if (rotateAble)
        {
            this.transform.Rotate(RotateAxis, rotateSpeed * Time.deltaTime, rotateAxisSpace);
        }
        
        transform.position += dp;
    }
}