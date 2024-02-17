using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private CombatManager combatManagerScript;

    [SerializeField] private List<GameObject> turnOrderItems;

    void Start(){
        combatManagerScript.refreshCombatUI += RefreshUI;
        foreach(Transform child in transform.GetChild(0)){
            turnOrderItems.Add(child.gameObject);
            child.gameObject.SetActive(false);
            child.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void RefreshUI(){

        deactivateUIElements();

        int i = 0;
        foreach(IFighter obj in combatManagerScript.combatParticipants){
            turnOrderItems[i].gameObject.SetActive(true);
            turnOrderItems[i].GetComponent<UnityEngine.UI.Image>().sprite = obj.PreviewImage;

            if(i == combatManagerScript.currentTurnIndex){
                turnOrderItems[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            i++;
        }
    }

    void deactivateUIElements(){
        foreach(GameObject obj in turnOrderItems){
            obj.SetActive(false);
            obj.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
