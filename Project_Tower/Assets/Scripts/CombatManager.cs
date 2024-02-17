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
    [SerializeField] public List<Color> colors;

    public event Action refreshCombatUI;

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void StartCombat(){
        //UpdateEnemyList();
        combatParticipants = new List<IFighter>();
        /*
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });
        */

        int i = 0;
        foreach(GameObject obj in gameManagerScript.roomManagerScript.GetAllEnemies()){
            EnemyBase enemyBaseScript = obj.GetComponent<EnemyBase>();
            combatParticipants.Add(enemyBaseScript);
            enemyBaseScript.color = colors[i];
            i++;
        }

        combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());

        SortBySpeed();

        currentTurnIndex = 0;

        refreshCombatUI?.Invoke();

        NextTurn();
    }

    public void EndCombat(){
        combatParticipants.Clear();
        refreshCombatUI.Invoke();
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
        ((EnemyBase)combatParticipants[currentTurnIndex]).Highlight();
        yield return new WaitForSeconds(1f);
        Debug.Log("Castoltam a spellt");
        float waitAfterAttack = combatParticipants[currentTurnIndex].Attack();
        yield return new WaitForSeconds(waitAfterAttack + 0.5f);
        ((EnemyBase)combatParticipants[currentTurnIndex]).Lowlight();
        Debug.Log("továbbadás");
        UpdateEnemyList();
        //currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;
        NextTurn();
    }

    public IEnumerator PlayerTakeTurn(){
        gameManagerScript.isPlayerTurn = false;
        yield return new WaitForSeconds(gameManagerScript.currentSpell.animationTime + 0.5f);
        UpdateEnemyList();
        //currentTurnIndex = (currentTurnIndex+1) % combatParticipants.Count;

        Debug.Log("Player körének vége");

        NextTurn();
    }


    private void UpdateEnemyList(){
        gameManagerScript.roomManagerScript.RoomUpdateEnemies();
        List<IFighter> currentFigtingEnemies = new List<IFighter>();
        gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => currentFigtingEnemies.Add(i.GetComponent<EnemyBase>()));
        //combatParticipants.Clear();
        int cnt = -1;
        int deadBefCurInd = 0;
        for(int i = 0; i < combatParticipants.Count;){
            cnt++;
            if(combatParticipants[i] is not Player && !currentFigtingEnemies.Contains(combatParticipants[i])){
                combatParticipants.RemoveAt(i);
                if(cnt <= currentTurnIndex)
                    currentTurnIndex--;
                    deadBefCurInd++;
            }
            else
                i++;
        }
        /*combatParticipants.RemoveAll(i => {
            cnt++;
            if(i is not Player && !currentFigtingEnemies.Contains(i)){
                if(cnt <= currentTurnIndex)
                    deadBefCurInd++;
                return true;
            }
            return false;
        });*/
        /*gameManagerScript.roomManagerScript.GetAllEnemies().ForEach(i => 
        {
            if(combatParticipants.Contains(i.GetComponent<EnemyBase>()))
            combatParticipants.Add(i.GetComponent<EnemyBase>());
        });*/

        //currentTurnIndex -= deadBefCurInd;
        currentTurnIndex++;
        if(currentTurnIndex >= combatParticipants.Count){
            currentTurnIndex = 0;
            SortBySpeed();
        }

        Debug.Log(combatParticipants.Count);

        if(combatParticipants.Count == 1){
            gameManagerScript.EndFight();
            Debug.Log("Combat vége!");
            refreshCombatUI?.Invoke();
            return;
        }

        //combatParticipants.Add(gameManagerScript.playerObj.GetComponent<Player>());

        //SortBySpeed();

        refreshCombatUI?.Invoke();
    }
}
