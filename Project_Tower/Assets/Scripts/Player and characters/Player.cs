using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public bool canMove;
    public float velocity;
    private float timer;
    private Vector2 lastPostion;

    public Character currCharacter;

    [SerializeField] public SpellBase currentSpell;
    [SerializeField] public List<Character> characterScritps;
    public List<Vector2> followPoints;



    void OnValidate()
    {
        if(Instance == null){
            Instance = this;
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }

        characterScritps.Clear();
        

        foreach(Transform child in this.transform){
            if(child.TryGetComponent<Character>(out Character characterScript)){
                characterScritps.Add(characterScript);
            }
        }

        canMove = true;
        timer = 0.0f;

        currCharacter = characterScritps[0];
        if(GameManager.Instance != null){
            currentSpell = GameManager.Instance.GetSpellByName(currCharacter.GetSpells()[0]);
            GameManager.Instance.RefreshCurrentSpell();
        }

        lastPostion = new Vector2(currCharacter.gameObject.transform.position.x, currCharacter.gameObject.transform.position.y);
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        currentSpell = GameManager.Instance.GetSpellByName(currCharacter.GetSpells()[0]);

        followPoints.Add(new Vector2(currCharacter.transform.position.x, currCharacter.transform.position.y));

        OnRoomEnter();
    }

    void Update(){
            if (Input.GetKeyDown(KeyCode.R) 
                    && characterScritps[0].health <= 0 
                    && characterScritps[1].health <= 0 
                    && characterScritps[2].health <= 0
                )
            {
                GameManager.Instance.Restart();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

    }

    void FixedUpdate(){

        //Stop every character
        foreach(Character character in characterScritps){
            if(character != currCharacter){
                character.Move(0, 0);
                
                if(character.frameCount > 0){
                    character.frameCount--;
                }
            }
        }

        if(canMove){
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");
                currCharacter.Move(moveX, moveY);

                //Only increase timer and move others if we are moving
                //Debug.Log((new Vector2(currCharacter.gameObject.transform.position.x, currCharacter.gameObject.transform.position.y) - lastPostion).magnitude);
                if((new Vector2(currCharacter.gameObject.transform.position.x, currCharacter.gameObject.transform.position.y) - lastPostion).magnitude > 0.001f){
                    timer += Time.fixedDeltaTime;
                }

                //Veriable to see how many character have passed the 0th followpoint
                float closeCount = 0;
                foreach(Character character in characterScritps){
                    if(character != currCharacter && character.frameCount <= 0){

                        //If close the the first follow point, start following the second one
                        if(character.followPointIndex < followPoints.Count 
                            && character.Distance(followPoints[character.followPointIndex].x, followPoints[character.followPointIndex].y) <= 0.3f
                            )
                        {
                            character.followPointIndex++;
                        }

                        //Move the character
                        if(character.followPointIndex < followPoints.Count){

                            //If we get close to them, they stop a bit, so they won't flicker
                            if(character.followPointIndex == followPoints.Count-1 && character.Distance(currCharacter.transform.position.x, currCharacter.transform.position.y) <= character.offsetLength){
                               
                                character.frameCount = 10;

                               /*
                                character.frameCount++;
                                if(character.frameCount > 12){

                                }
                                //If the frame count where the player is close by lower then 5 then don't stop moving
                                else{
                                    character.Move(followPoints[character.followPointIndex].x - character.transform.position.x, followPoints[character.followPointIndex].y - character.transform.position.y, 0.9f);
                                }
                                */
                            }
                            else{
                                //character.frameCount = 0;
                                character.Move(followPoints[character.followPointIndex].x - character.transform.position.x, followPoints[character.followPointIndex].y - character.transform.position.y);
                            }
                        }

                        //Check how many character are following the second point
                        if(character.followPointIndex > 1){
                            closeCount++;
                        }
                    }
                }

                //if everyone follows the second point, delete the first
                if(closeCount >= characterScritps.Count-1){
                    foreach(Character character in characterScritps){
                        character.followPointIndex--;
                    }

                    followPoints.RemoveAt(0);
                }


            if(timer >= 0.5f){
                timer = 0.0f;
                followPoints.Add(new Vector2(currCharacter.transform.position.x, currCharacter.transform.position.y));
            }
        }

        lastPostion = new Vector2(currCharacter.gameObject.transform.position.x, currCharacter.gameObject.transform.position.y);

    }

    public void EnableFreeMovement(){
        canMove = true;
    }

    public void DisableFreeMovement(){
        canMove = false;
    }

    public void OnRoomEnter(){

        //Set offset length from main character
        foreach(Character character in characterScritps){
            character.frameCount = 0;
            if(character != currCharacter){
                character.offsetLength = new Vector2(character.transform.position.x - currCharacter.transform.position.x, character.transform.position.y - currCharacter.transform.position.y).magnitude;
            }
        }
    }

    public PlayerSaveData GetSaveInfo(){

        List<int> characterHealths = new List<int>();
        List<int> characterManas = new List<int>();;

        foreach(Character character in characterScritps){
            characterHealths.Add(character.health);
            characterManas.Add(character.mana);
        }

        return new PlayerSaveData(characterHealths, characterManas);
    }

    public void LoadFromData(PlayerSaveData data){

        //Esetleges problémák a karakter object-ek loadolásával
        for(int i = 0; i < characterScritps.Count; i++){
            characterScritps[i].health = data.characterHealths[i];
            characterScritps[i].mana = data.characterManas[i];
        }
    }

}
