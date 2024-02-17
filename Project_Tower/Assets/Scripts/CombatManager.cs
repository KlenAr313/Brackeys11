using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] public List<IFighter> combatParticipants;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] public int currentTurnIndex;

    public event Action refreshCombatUI;

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void StartCombat(){
        //UpdateEnemyList();
        combatParticipants = new List<IFighter>();
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });

        combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());

        SortBySpeed();

        currentTurnIndex = 0;

        refreshCombatUI?.Invoke();

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
        float waitAfterAttack = combatParticipants[currentTurnIndex].Attack();
        yield return new WaitForSeconds(waitAfterAttack + 0.5f);
        Debug.Log("továbbadás");
        currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;
        UpdateEnemyList();
        NextTurn();
    }

    public IEnumerator PlayerTakeTurn(){
        gameManagerScript.isPlayerTurn = false;
        yield return new WaitForSeconds(gameManagerScript.currentSpell.animationTime + 0.5f);
        currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;
        Debug.Log("Player körének vége");

        UpdateEnemyList();

        NextTurn();
    }


    private void UpdateEnemyList(){
        gameManagerScript.roomManagerScript.RoomUpdateEnemies();
        combatParticipants.Clear();
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });

        Debug.Log(combatParticipants.Count);

        if(combatParticipants.Count == 0){
            gameManagerScript.EndFight();
            Debug.Log("Combat vége!");
            refreshCombatUI?.Invoke();
            return;
        }

        combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());

        SortBySpeed();

        refreshCombatUI?.Invoke();
    }
}
