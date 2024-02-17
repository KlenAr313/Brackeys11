using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popupText;

    public void PopUp(string text){
        popUpBox.SetActive(true);
        popupText.text = text;
        animator.SetTrigger("Pop");
    }
}
