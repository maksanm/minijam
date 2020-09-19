using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMove120 : MonoBehaviour
{
	Camera cam;
	float height;
    float width;
    void Start()
    {
    	cam = Camera.main;
    	height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        transform.localPosition = new Vector3(-11.377f,0, 0);
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x,-29.064f);
    }
}
