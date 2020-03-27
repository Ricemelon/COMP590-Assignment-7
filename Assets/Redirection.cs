using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirection : MonoBehaviour
{

    public GameObject treasurehunter;
    public GameObject trackingspace;
    private Vector3 prevforward;
    private float prevyaw;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = treasurehunter.GetComponent<Camera>();
        prevforward = cam.transform.forward;
        Vector2 forward = new Vector2(cam.transform.forward.x, cam.transform.forward.z);
        //need to check if camera position is relative to vr tracking origin, and if that origin is 0,0......
        Vector2 origin = new Vector2(trackingspace.transform.position.x-cam.transform.position.x, trackingspace.transform.position.z-cam.transform.position.z);
        prevyaw = angleBetween(forward, origin);
        
    }

    // Update is called once per frame
    void Update()
    {
        float newrotate = angleBetween(prevforward, cam.transform.forward);
        int rotateside = (whichSide(cam.transform.position+prevforward, cam.transform.position, cam.transform.position+cam.transform.forward)<0)?1:-1;
        Vector2 forward = new Vector2(cam.transform.forward.x, cam.transform.forward.z);
        Vector2 origin = new Vector2(trackingspace.transform.position.x-cam.transform.position.x, trackingspace.transform.position.z-cam.transform.position.z);
        float deltayaw = prevyaw - angleBetween(forward, origin);
        float distance = origin.magnitude;
        float longestdim = 5f;
        float threshhold = 0;
        if(deltayaw<0){
            threshhold = -0.13f;
        }else{
            threshhold = 0.3f;
        }
        float accelby = threshhold*deltayaw*rotateside*Mathf.Clamp(distance/longestdim/2,0,1);

        trackingspace.transform.RotateAround(cam.transform.position,new Vector3(0,1,0),accelby);
        prevforward = cam.transform.forward;
        prevyaw = angleBetween(forward, origin);
    }

    private float whichSide(Vector3 a, Vector3 b, Vector3 c){
        //NOTE: y of vector2 must actually be z value of vector3 
        //https://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
        return (a.x-b.x)*(c.z-b.z) - (a.z-b.z)*(c.x-b.x);
    }

    private float angleBetween(Vector2 a, Vector2 b){
        //Construct Vector2 with x and z or construct a Vector3 with y as 0 which has the same effect
        a.Normalize();
        b.Normalize();
        var dot = Vector2.Dot(a, b);
        return Mathf.Acos(dot);

    }
}
