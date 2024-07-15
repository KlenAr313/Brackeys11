using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] public List<IFighter> combatParticipants;
    [SerializeField] public int currentTurnIndex;
    [SerializeField] public List<Color> colors;
    [SerializeField] public int enemyCount;
    [SerializeField] public static CombatManager Instance;
    [SerializeField] public List<IFighter> EnemyList;

    public event Action refreshCombatUI;

    void Start(){
        if(Instance == null){
            Instance = this;
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }
    }

    public void StartCombat(){
        enemyCount = 0;
        Debug.Log("Combat Started");
        combatParticipants = new List<IFighter>();

        foreach(GameObject obj in GameManager.Instance.roomManagerScript.GetAllEnemies()){
            EnemyBase enemyBaseScript = obj.GetComponent<EnemyBase>();
            combatParticipants.Add(enemyBaseScript);
            enemyBaseScript.color = colors[enemyCount];
            enemyCount++;
        }

        foreach(Character charScript in Player.Instance.characterScritps){
            combatParticipants.Add(charScript);
        }

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

        if(!GameManager.Instance.isFighting){
            return;
        }

        //Enemy turn
        if(combatParticipants[currentTurnIndex] is not Character){
            StartCoroutine(TakeEnemyTurn());
        }

        //Player turn
        else{
            //Debug.Log("Player köre");
            Player.Instance.currCharacter = (Character)combatParticipants[currentTurnIndex];
            Player.Instance.currCharacter.moveRange = Player.Instance.currCharacter.baseMoveRange;
            GameManager.Instance.isPlayerTurn = true;
            GameManager.Instance.RefreshCurrentSpell();
            Player.Instance.currCharacter.UpdateUI();
        }
    }


    IEnumerator TakeEnemyTurn(){
        //Debug.Log(currentTurnIndex + ". enemy köre");
        ((EnemyBase)combatParticipants[currentTurnIndex]).Highlight();
        yield return new WaitForSeconds(1f);

        //Debug.Log("Castoltam a spellt");
        float waitAfterAttack = combatParticipants[currentTurnIndex].Attack();
        yield return new WaitForSeconds(waitAfterAttack + 0.5f);
        //UpdateEnemyList();

        //((EnemyBase)combatParticipants[currentTurnIndex]).Lowlight();
        //Debug.Log("továbbadás");
        UpdateEnemyList();
        
        NextTurn();
    }

    public IEnumerator PlayerTakeTurn(){
        GameManager.Instance.isPlayerTurn = false;
        yield return new WaitForSeconds(Player.Instance.currentSpell.animationTime + 0.5f);
        UpdateEnemyList();

        //Debug.Log("Player körének vége");

        NextTurn();
    }


    private void UpdateEnemyList(){
        GameManager.Instance.roomManagerScript.RoomUpdateEnemies();
        EnemyList = new List<IFighter>();
        GameManager.Instance.roomManagerScript.GetAllEnemies().ForEach(i => EnemyList.Add(i.GetComponent<EnemyBase>()));
        //combatParticipants.Clear();
        int cnt = -1;
        int deadBefCurInd = 0;
        for(int i = 0; i < combatParticipants.Count;){
            cnt++;
            if(combatParticipants[i] is not Character && !EnemyList.Contains(combatParticipants[i])){
                combatParticipants.RemoveAt(i);
                if(cnt <= currentTurnIndex)
                    currentTurnIndex--;
                    deadBefCurInd++;
            }
            else
                i++;
        }
        
        currentTurnIndex++;
        if(currentTurnIndex >= combatParticipants.Count){
            currentTurnIndex = 0;
            SortBySpeed();
        }

        Debug.Log(enemyCount);

        if(enemyCount <= 0){
            GameManager.Instance.EndFight();
            //Debug.Log("Combat vége!");
            foreach(Character character in Player.Instance.characterScritps){
                character.GiveMana(20);
            }
            refreshCombatUI?.Invoke();
            StopAllCoroutines();
            return;
        }

        refreshCombatUI?.Invoke();
    }
}
