using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMoveAT : MonoBehaviour
{
	private Transform trans;
    void Start()
    {
    	trans = GetComponentInParent<Transform>();
    	trans.position = new Vector2(trans.position.x, trans.position.y);
    }

    void Update()
    {
    	transform.position =  new Vector2(12.921f, trans.position.y);
    }
}
