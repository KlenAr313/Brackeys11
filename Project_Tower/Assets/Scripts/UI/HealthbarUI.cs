using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image healthBarImage;

    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        gameManagerScript.playerScript.UpdateStatUI += RefreshHealthUI;
    }

    private void RefreshHealthUI(){
        healthBarImage.fillAmount = (float) gameManagerScript.playerScript.health / (float) gameManagerScript.playerScript.GetBaseHealth();
    }
}
