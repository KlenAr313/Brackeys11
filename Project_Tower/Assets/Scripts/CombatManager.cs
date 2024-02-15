using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> combatParticipants;
    [SerializeField] private GameManager gameManagerScript;

    public event Action refreshCombatUI;

    private int playerPosition;
    private int currentTurnIndex;

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void StartCombat()
    {
        combatParticipants = new List<IFighter>();
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });

        combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());
        playerPosition = combatParticipants.Count - 1;

        SortBySpeed();

        currentTurnIndex = 0;

        UpdateEnemyList();

        NextTurn();
    }

    public void EndCombat(){
        combatParticipants.Clear();
    }

    private void SortBySpeed(){
        combatParticipants = combatParticipants.OrderByDescending(i => i.Speed).ToList();
    }

    private void NextTurn(){

        if(!gameManagerScript.isFighting){
            return;
        }

        //Enemy turn
        if(combatParticipants[currentTurnIndex] is not Player){
            StartCoroutine(TakeEnemyTurn());
        }

        //Player turn
        else{
            Debug.Log("Player köre");
            gameManagerScript.isPlayerTurn = true;
        }
    }


    IEnumerator TakeEnemyTurn(){
        Debug.Log(currentTurnIndex + ". enemy köre");
        yield return new WaitForSeconds(1f);
        Debug.Log("Castoltam a spellt");
        yield return new WaitForSeconds(1f);
        Debug.Log("továbbadás");
        currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;
        NextTurn();
    }

    public void PlayerTakeTurn(){
        currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;
        gameManagerScript.isPlayerTurn = false;
        Debug.Log("Player körének vége");

        UpdateEnemyList();

        NextTurn();
    }


    private void UpdateEnemyList(){
        combatParticipants.Clear();
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });

        if(combatParticipants.Count == 0){
            gameManagerScript.EndFight();
            Debug.Log("Combat vége!");
            refreshCombatUI?.Invoke();
            return;
        }

        combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());
        playerPosition = combatParticipants.Count - 1;

        SortBySpeed();

        refreshCombatUI?.Invoke();
    }
}
