using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMove120 : MonoBehaviour
{
	Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.localPosition = new Vector3(-11.377f*cam.orthographicSize/6.76f,0, 0);
        transform.position = new Vector2(transform.position.x,-29.064f);
    }
}
