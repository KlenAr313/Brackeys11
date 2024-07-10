using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image healthBarImage;

    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        for(int i = 0; i < gameManagerScript.characterScritps.Count; i++){
            gameManagerScript.characterScritps[i].UpdateStatUI += RefreshHealthUI;
        }
    }

    private void RefreshHealthUI(){
        healthBarImage.fillAmount = (float) gameManagerScript.currCharacter.health / (float) gameManagerScript.currCharacter.GetBaseHealth();
    }
}
