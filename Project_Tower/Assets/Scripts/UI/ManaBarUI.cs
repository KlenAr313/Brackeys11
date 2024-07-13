using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image manaBarImage;

    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private Player playerScript;

    void Start(){
        playerScript.currCharacter.UpdateStatUI += RefreshManaUI;
    }

    private void RefreshManaUI(){
        Debug.Log("Mana updating in UI");
        manaBarImage.fillAmount = (float) playerScript.currCharacter.mana / (float) playerScript.currCharacter.GetBaseMana();
    }
}
