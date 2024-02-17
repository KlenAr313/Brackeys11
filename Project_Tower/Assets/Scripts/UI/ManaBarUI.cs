using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image manaBarImage;

    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        gameManagerScript.playerScript.UpdateStatUI += RefreshHealthUI;
    }

    private void RefreshHealthUI(){
        manaBarImage.fillAmount = (float) gameManagerScript.playerScript.mana / (float) gameManagerScript.playerScript.baseMana;
    }
}
