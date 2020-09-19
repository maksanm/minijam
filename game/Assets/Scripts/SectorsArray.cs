using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsArray : MonoBehaviour
{
	Vector2[,] sectors = new Vector2[20, 20];
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
        	for (int j = 0; j < 20; j++)
        	{
        		sectors[i, j] = new Vector2(-20.67f+64.55f/40+64.55f*i/20,-62.65f+64.55f/40+64.55f*j/20);
        	}
        }
    }

    void Update()
    {
        
    }
}
