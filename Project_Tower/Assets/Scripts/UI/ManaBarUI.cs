using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image manaBarImage;

    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        gameManagerScript.currCharacter.UpdateStatUI += RefreshManaUI;
    }

    private void RefreshManaUI(){
        Debug.Log("Mana updating in UI");
        manaBarImage.fillAmount = (float) gameManagerScript.currCharacter.mana / (float) gameManagerScript.currCharacter.GetBaseMana();
    }
}
