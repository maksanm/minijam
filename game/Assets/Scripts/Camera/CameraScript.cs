using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
        public float krai = 10f;
        public float speed = 10f;
        private Vector3 startPos;
        float height;
 		float width;
        Camera cam;
        void Start () 
        {
        	cam = Camera.main;
            startPos = transform.position;
            height = 2f * cam.orthographicSize;
            width = height * cam.aspect;
        }
       
        void Update () 
        {
        	if(Input.GetKeyDown(KeyCode.Space))
        	{
        		transform.position = startPos;
        	}
            if(Input.mousePosition.x < krai && transform.position.x > -7.3f){
                    transform.position -= new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.x > Screen.width - krai && transform.position.x < 33.11f){
                    transform.position += new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y < krai && transform.position.y > -54.55f){
                    transform.position -= new Vector3 (0,speed,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y > Screen.height - krai && transform.position.y < -3.57f){
                    transform.position += new Vector3 (0,speed,0) * Time.deltaTime;
            }
        }
}