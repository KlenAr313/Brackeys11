using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image manaBarImage;

    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        gameManagerScript.playerScript.UpdateStatUI += RefreshManaUI;
    }

    private void RefreshManaUI(){
        Debug.Log("Mana updating in UI");
        manaBarImage.fillAmount = (float) gameManagerScript.playerScript.mana / (float) gameManagerScript.playerScript.GetBaseMana();
    }
}
