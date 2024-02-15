using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private CombatManager combatManagerScript;

    [SerializeField] private GameObject turnOrderItemPrefab;
    [SerializeField] private List<GameObject> turnOrderItems;

    private int firstPosition = 370;
    private int gapSize = 40;

    void Start(){
        combatManagerScript.refreshCombatUI += RefreshUI;
    }

    void RefreshUI(){

        deleteUIElements();

        int i = 0;
        foreach(IFighter obj in combatManagerScript.combatParticipants){
            GameObject turnOrderItem = GameObject.Instantiate(turnOrderItemPrefab, new Vector3(firstPosition + i * gapSize, 500, 0), Quaternion.identity, this.gameObject.transform);
            turnOrderItems.Add(turnOrderItem);
            i++;
        }
    }

    void deleteUIElements(){
        turnOrderItems.Clear();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
