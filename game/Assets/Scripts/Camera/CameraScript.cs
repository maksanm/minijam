using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
        public float krai = 10f;
        public float speed = 10f;
        public float maxScale = 8;
        public float minScale = 4;
        private Vector3 startPos;
        private float mw;
        public float startScale = 6;
        public float scaleSpeed = 1f;
        float height;
 		float width;
        Camera cam;
        void Start () 
        {
        	cam = Camera.main;
            startPos = transform.position;
            cam.orthographicSize = startScale;
        }
       
        void Update () 
        {
            height = 2f * cam.orthographicSize;
            width = height * cam.aspect;
            mw = Input.GetAxis("Mouse ScrollWheel");
        	if(Input.GetKeyDown(KeyCode.Space))
        	{
        		transform.position = startPos;
        	}
            if(Input.mousePosition.x < krai && transform.position.x - 1 - width/2 > -20.67f){
                    transform.position -= new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.x > Screen.width - krai && transform.position.x - 1 + width/2 < 43.88f){
                    transform.position += new Vector3 (speed,0,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y < krai && transform.position.y - 1 - height/2 > -62.85f){
                    transform.position -= new Vector3 (0,speed,0) * Time.deltaTime;
            }
            if(Input.mousePosition.y > Screen.height - krai && transform.position.y - 1 + height/2 < 1.7f){
                    transform.position += new Vector3 (0,speed,0) * Time.deltaTime;
            }
            if (mw < -0.1 && cam.orthographicSize <= maxScale)
            {
                cam.orthographicSize += scaleSpeed;
            }
            if (mw > 0.1 && cam.orthographicSize >= minScale)
            {
                cam.orthographicSize -= scaleSpeed;
            }
        }
}