using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMove120 : MonoBehaviour
{
	private Transform trans;
    void Start()
    {
    	trans = GetComponentInParent<Transform>();
    	trans.position = new Vector2(trans.position.x + 2.3f, trans.position.y);
    }

    void Update()
    {
        transform.position = new Vector2(trans.position.x,-29.064f);
    }
}
