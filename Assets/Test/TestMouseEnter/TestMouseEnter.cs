using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;

public class TestMouseEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hover hover = GetComponent<Hover>();
        hover.OnEnter.AddListener(delegate
        {  
            Debug.Log($"XHW {gameObject.name} hover.onenter ");
        });
        hover.OnExit.AddListener(delegate
        {
            Debug.Log($"XHW {gameObject.name} hover.OnExit ");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 checkpoint = Vector3.zero;
    public float planeDis;
    public void checkFlipped()
    {
        Plane a = new Plane(Vector3.up, planeDis);
        Ray checkRay = new Ray(new Vector3(0, 100, 0), Vector3.down);
        
        Vector3 aM = GetRayMeetpointWithPlane(checkRay, a);
        //Debug.Log($"am = {aM}");
        DrawMeetingPoint(Color.red, aM);

        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(checkpoint, 0.5f);
        
        Vector3 flippedPoint = MathHelper.GetFlippedPointWithPlane(checkpoint, a);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(flippedPoint, 0.5f);
    }
    
    //[ContextMenu("测试相交点")]
    public void OnDrawGizmos()
    {
        checkFlipped();
        return;
        
        Plane a = new Plane(Vector3.up, -1);
        Plane b = new Plane(Vector3.down, -2);
        Plane c = new Plane(Vector3.up, -3);
        Plane d = new Plane(Vector3.down, -4);

        //ay checkRay = new Ray(new Vector3(0, -5, 0), Vector3.up);
        
        Ray checkRay = new Ray(new Vector3(0, 5, 0), Vector3.down);
        
        Vector3 aM = GetRayMeetpointWithPlane(checkRay, a);
        Debug.Log($"am = {aM}");
        DrawMeetingPoint(Color.red, aM);
            
        Vector3 bM = GetRayMeetpointWithPlane(checkRay, b);
        Debug.Log($"bm = {bM}");
        DrawMeetingPoint(Color.blue, bM);
            
        Vector3 cM = GetRayMeetpointWithPlane(checkRay, c);
        Debug.Log($"cm = {cM}");
        DrawMeetingPoint(Color.yellow, cM);
            
        Vector3 dM = GetRayMeetpointWithPlane(checkRay, d);
        Debug.Log($"dm = {dM}");
        DrawMeetingPoint(Color.green, dM);
    }

    private Vector3 GetRayMeetpointWithPlane(Ray ray, Plane plane)
    {
        if (plane.Raycast(ray, out float aEnter))
        {
            return ray.GetPoint(aEnter);
        }
        else
        {
            return Vector3.negativeInfinity;
        }
    }

    private void DrawMeetingPoint(Color color, Vector3 meetPoint)
    {
        if (meetPoint == Vector3.negativeInfinity)
        {
            return;
        }
            
        Gizmos.color = color;
        Gizmos.DrawSphere(meetPoint, 0.2f);
    }
}
