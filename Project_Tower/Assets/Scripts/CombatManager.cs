using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> combatParticipants;
    [SerializeField] private GameManager gameManagerScript;

    private int playerPosition;
    private int currentTurnIndex;

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void StartCombat(){
        combatParticipants = gameManagerScript.roomManagerScript.GetAllEnemies();

        combatParticipants.Add(gameManagerScript.playerObj);
        playerPosition = combatParticipants.Count - 1;

        SortBySpeed();

        currentTurnIndex = 0;

        NextTurn();
    }

    public void EndCombat(){
        combatParticipants.Clear();
    }

    private void SortBySpeed(){
        /*
        int changes = -1;
        while(changes == 0){
            changes = 0;

            for(int i = 0; i < combatParticipants.Count-1; i++){
                //egyik sem a player
                if(i != playerPosition && i+1 != playerPosition){
                    EnemyBase leftEnemy = combatParticipants[i].GetComponent<EnemyBase>();
                    EnemyBase rightEnemy = combatParticipants[i+1].GetComponent<EnemyBase>();

                    if(leftEnemy.Speed > rightEnemy.Speed){
                        GameObject tmp = combatParticipants[i];
                        GameManager 
                    }
                }
            }
        }
        */
    }

    private void NextTurn(){

        if(!gameManagerScript.isFighting){
            return;
        }

        //Enemy turn
        if(currentTurnIndex != playerPosition){
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
        combatParticipants = gameManagerScript.roomManagerScript.GetAllEnemies();

        if(combatParticipants.Count == 0){
            gameManagerScript.isFighting = false;
            Debug.Log("Combat vége!");
            return;
        }

        combatParticipants.Add(gameManagerScript.playerObj);
        playerPosition = combatParticipants.Count - 1;

        SortBySpeed();
    }
}
