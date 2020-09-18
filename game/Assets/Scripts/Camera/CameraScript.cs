using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
        public float krai = 10f;
        public float speed = 10f;
        private Vector3 startPos;
        void Start () 
        {
           startPos = transform.position;
        }
       
        void Update () 
        {
        	if(Input.GetKeyDown(KeyCode.Space))
        	{
        		transform.position = startPos;
        	}
            if(Input.mousePosition.x < krai && transform.position.x > -7.76f){
                    transform.position -= new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.x > Screen.width - krai && transform.position.x < 32.25f){
                    transform.position += new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y < krai && transform.position.y > -55.52f){
                    transform.position -= new Vector3 (0,speed,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y > Screen.height - krai && transform.position.y < -2f){
                    transform.position += new Vector3 (0,speed,0) * Time.deltaTime;
            }
        }
}