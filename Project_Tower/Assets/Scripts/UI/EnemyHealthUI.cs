using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image healthBarImage;

    [SerializeField] private EnemyBase enemyBaseScript;

    void Start(){
        enemyBaseScript.UpdateEnemyHealthUI += RefreshHealthUI;
        this.healthBarImage.color = enemyBaseScript.color;
    }

    private void RefreshHealthUI(){
        //this.healthBarImage.color = enemyBaseScript.color;
        healthBarImage.fillAmount = (float) enemyBaseScript.Health / (float) enemyBaseScript.GetBaseHealth();
    }
}
