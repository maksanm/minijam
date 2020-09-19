using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripMoveAT : MonoBehaviour
{
    void Start()
    {
        transform.localPosition = new Vector3(0, 6.14f, 0);
    }

    void Update()
    {
    	transform.position =  new Vector2(12.921f, transform.position.y);
    }
}
