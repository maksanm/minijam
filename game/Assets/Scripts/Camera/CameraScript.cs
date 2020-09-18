using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
        public float krai = 10f;
        public float speed = 10f;
       
        void Start () 
        {
               
        }
       
        void Update () {
                if(Input.mousePosition.x < krai){
                        transform.position -= new Vector3 (speed,0,0) * Time.deltaTime;
                }
                if(Input.mousePosition.x > Screen.width - krai){
                        transform.position += new Vector3 (speed,0,0) * Time.deltaTime;
                }
                if(Input.mousePosition.y < krai){
                        transform.position -= new Vector3 (0,0,speed) * Time.deltaTime;
                }
                if(Input.mousePosition.y > Screen.height - krai){
                        transform.position += new Vector3 (0,0,speed) * Time.deltaTime;
                }
        }
}