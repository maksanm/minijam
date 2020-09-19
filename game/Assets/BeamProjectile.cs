using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamProjectile : MonoBehaviour
{
    [HideInInspector]
    public string team;
    [HideInInspector]
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
