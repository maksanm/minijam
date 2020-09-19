using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMoveAT : MonoBehaviour
{
	Camera cam;
    void Start()
    {
    	cam = Camera.main;
    }

    void Update()
    {
    	transform.localPosition = new Vector3(0, 6.14f*cam.orthographicSize/6.76f, 0);
    	transform.position =  new Vector2(12.921f, transform.position.y);
    }
}
