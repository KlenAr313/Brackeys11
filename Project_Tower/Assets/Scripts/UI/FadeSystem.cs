using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSystem : MonoBehaviour
{
        [SerializeField] private GameObject blocker;
        [SerializeField] private Animator animator;

        public void OpenDoor(){
            animator.SetTrigger("Open");
        }
}
