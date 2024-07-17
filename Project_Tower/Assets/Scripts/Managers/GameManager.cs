using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] public bool isPlayerTurn;
    [SerializeField] public bool isFighting;
    [SerializeField] public GameObject playerObj;
    [SerializeField] public RoomManager roomManagerScript;
    [SerializeField] public TileManager tileManagerScript;
    [SerializeField] public List<SpellBase> spellList;
    [SerializeField] public CombatManager combatManagerScript;

    //Basically the length of the spell bar
    [SerializeField] public static int MaxAbilities = 3;

    public bool canClick = true;
    private int currentX;
    private int currentY;

    public event Action SpellRefreshed;
    public event Action OnGameOver;

    public static GameManager Instance;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        //Debug miatt true, false legyen alapból!
#if DEBUG
        //isFighting = true;
#else
        isFighting = false;
#endif
    }

    void OnValidate(){
        if(Instance == null){
            Instance = this;
        }
        
        if(Instance != this){
            Destroy(this.gameObject);
        }

        spellList.Clear();  
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        combatManagerScript = gameObject.GetComponent<CombatManager>();

        GameObject spellsObj = this.gameObject.transform.Find("Spells").gameObject;
        Component[] components = spellsObj.GetComponents(typeof(Component));
        foreach(Component comp in components){
            if(comp.ToString() != "Spells (UnityEngine.Transform)"){
                spellList.Add((SpellBase)comp);
            }
        }

        currentX = -1;
        currentY = -1;

        isPlayerTurn = false;
        RefreshCurrentSpell();
    }

        void OnGUI(){
        if(GUI.Button(new Rect(10, 150, 100, 50), "Next Level")){
            NextLevel();
        }
    }

    public void TileClicked(int posX, int posY)
    {
        //Debug.Log("TileClicked: " + posX + " " + posY);
        if(!canClick){
            return;
        }

        if(isFighting){

            if(tileManagerScript.IsTileClickable(posX, posY)){
                currentX = posX;
                currentY = posY;
            }
            else
            {
                return;
            }
            //Player köre
            if(isPlayerTurn){
                //Check if the player is moving
                if(Player.Instance.currentSpell.spellName == "Move"){
                    tileManagerScript.RemoveAllHighlight();
                    ((Move)Player.Instance.currentSpell).PerformMove(currentX, currentY);
                    //Refresh to highlighter after movement
                    //TileHighlighter(currentX,currentY);
                }
                else if(Player.Instance.currentSpell != null && Player.Instance.currentSpell.ManaCost <= Player.Instance.currCharacter.mana){
                    roomManagerScript.TileClickedAttack(Player.Instance.currentSpell.GetEffectedTiles(currentX, currentY));

                    Player.Instance.currentSpell.PlayAnimation(currentX, currentY);
                    Player.Instance.currentSpell.PlaySound();
                    tileManagerScript.RemoveAllHighlight();

                    //Reduce mana and end player turn
                    Player.Instance.currCharacter.DecreaseMana(Player.Instance.currentSpell.ManaCost);
                    StartCoroutine(combatManagerScript.PlayerTakeTurn());
                }
            }
        }
        else
        {
            roomManagerScript.TileClicked(posX, posY);
        }
    }

    public void TileHighlighter(int posX, int posY){
        //Debug.Log("Center: " + posX + " " + posY);

        if(!isPlayerTurn){
            return;
        }

        if(!canClick){
            return;
        }

        if(tileManagerScript.IsTileClickable(posX, posY)){
            currentX = posX;
            currentY = posY;
        }
        if(isFighting){
            //Player köre
            if(isPlayerTurn){
                if(Player.Instance.currentSpell != null){
                    //Debug.Log("Redoing highlights with: " + currentX + " " + currentY); 
                    foreach(Vector2Int coord in Player.Instance.currentSpell.GetEffectedTiles(currentX, currentY)){
                        tileManagerScript.highlightSpellPreview(coord.x, coord.y);
                    }
                }
            }
        }
    }

    public void StartFight(){
        this.isFighting = true;
        combatManagerScript.StartCombat();
        Player.Instance.OnCombatStart();
    }

    public void EndFight(){
        this.isFighting = false;
        Player.Instance.OnCombatEnd();
    }

    //Highlight miatt van itt
    public void RefreshCurrentSpell(){

        if(Player.Instance == null){
            return;
        }

        foreach(SpellBase spell in spellList){
            if(spell.spellName == Player.Instance.currCharacter.selectedSpell){
                Player.Instance.currentSpell = spell;
            }
        }

        if(currentX != -1 && currentY != -1){
            if(tileManagerScript != null){
                tileManagerScript.RemoveAllHighlight();
            }
            TileHighlighter(currentX, currentY);
        }

        SpellRefreshed?.Invoke();
    }

    public SpellBase GetSpellByName(string spellName){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == spellName){
                return spell;
            }
        }
        return null;
    }

    public int GetSpellDamage(string spellName){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == spellName){
                return spell.DamageModifier;
            }
        }
        return 0;
    }

    public void GameOver(){
        OnGameOver.Invoke();
    }

    public void Restart(){
        //Végleges Scene név re beirni
        SceneManager.LoadScene(0);
    }

    public Character GetRandomCharacter(){
        System.Random rnd = new System.Random();
        return Player.Instance.characterScritps[rnd.Next(0,Player.Instance.characterScritps.Count)];
    }

    public void NextLevel(){
        SceneManager.LoadScene("Gerha");
        //LevelManager.Instance.NewLevel
    }


//Debug buttons
#if DEBUG
    void Update(){
        if(Input.GetKeyDown(KeyCode.K)){
            Debug.Log("Combat started!");
            isFighting = true;
            combatManagerScript.StartCombat();
        }

        if(Input.GetKeyDown(KeyCode.L)){
            Debug.Log("Combat ended!");
            isFighting = false;
            tileManagerScript.RemoveAllHighlight();
            combatManagerScript.EndCombat();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            Debug.Log("P gombnyomás");
            tileManagerScript.GetPlayableArea(1);
        }

    } 
#endif

}
