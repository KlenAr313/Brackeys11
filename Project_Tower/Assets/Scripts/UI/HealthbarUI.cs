using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image healthBarImage;

    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private Player playerScript;

    void Start(){
        for(int i = 0; i < playerScript.characterScritps.Count; i++){
            playerScript.characterScritps[i].UpdateStatUI += RefreshHealthUI;
        }
    }

    private void RefreshHealthUI(){
        healthBarImage.fillAmount = (float) playerScript.currCharacter.health / (float) playerScript.currCharacter.GetBaseHealth();
    }
}
