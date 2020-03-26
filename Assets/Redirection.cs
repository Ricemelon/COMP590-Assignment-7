using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirection : MonoBehaviour
{

    public GameObject treasurehunter;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = treasurehunter.GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float whichSide(Vector2 a, Vector2 b, Vector2 c){
        //NOTE: y of vector2 must actually be z value of vector3 
        return (a.x-b.x)*(c.y-b.y) - (a.y-b.y)*(c.x-b.x);
    }
}
