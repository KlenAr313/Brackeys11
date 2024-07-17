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
    private float HorizontalCameraOffset = 7.5f;
    private float VerticalCameraOffset = 3.5f;

    void OnValidate(){
        target = GameObject.Find("Character 1").transform;
    }

    private void FixedUpdate()
    {
        if(isFollowing){
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            //Szoba jobb korlátja
            if(transform.position.x > RoomManager.Instance.RoomBoundries.HorMax - HorizontalCameraOffset){
                transform.position = new Vector3(RoomManager.Instance.RoomBoundries.HorMax - HorizontalCameraOffset, transform.position.y, -10);
            }

            //Szoba felső korlátja
            if(transform.position.y > RoomManager.Instance.RoomBoundries.VertMax - VerticalCameraOffset){
                transform.position = new Vector3(transform.position.x, RoomManager.Instance.RoomBoundries.VertMax - VerticalCameraOffset, -10);
            }

            //Szoba bal korlátja
            if(transform.position.x < RoomManager.Instance.RoomBoundries.HorMin + HorizontalCameraOffset){
                transform.position = new Vector3(RoomManager.Instance.RoomBoundries.HorMin + HorizontalCameraOffset, transform.position.y, -10);
            }

            //Szoba alsó korlátja
            if(transform.position.y < RoomManager.Instance.RoomBoundries.VertMin + VerticalCameraOffset){
                transform.position = new Vector3(transform.position.x, RoomManager.Instance.RoomBoundries.VertMin + VerticalCameraOffset, -10);
            }
        }
        
    }
}
