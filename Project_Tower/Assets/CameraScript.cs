using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private bool isFollowing;

    void OnValidate(){
        target = GameObject.Find("Character 1").transform;
    }

    private void FixedUpdate()
    {
        if(isFollowing){
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            //Szoba bal korlátja
            if(transform.position.x < 0){
                transform.position = new Vector3(0f, transform.position.y, -10);
            }

            //Szoba jobb korlátja
            if(transform.position.x > 100){
                transform.position = new Vector3(100f, transform.position.y, -10);
            }

            //Szoba alsó korlátja
            if(transform.position.y < 0){
                transform.position = new Vector3(transform.position.x, 0, -10);
            }

            //Szoba felső korlátja
            if(transform.position.y > 100){
                transform.position = new Vector3(transform.position.x, 100, -10);
            }
        }
        
    }
}
