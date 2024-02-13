using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private TileManager _TileManagerPrefab;

    private bool[] doors;
    private int height;
    private int width;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;

    private bool isFighting;

    void Start()
    {
        doors = new bool[4];

        
        //TODO Debug miatt true, false legyen alapból!
        isFighting = true;

        Initialise();
    }

    //Main click entry point
    public void TileClicked(int posX, int posY){
        if(isFighting){
            foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                if(enemyBaseScript.PosX == posX && enemyBaseScript.PosY == posY){
                    //TODO Majd player damage kell az argumentumba
                    enemyBaseScript.GetDamaged(2);
                }
            }
            updateEnemies();
        }
        else if((doors[0] && posY == height-1 && posX == width / 2) || (doors[1] && posY == height / 2 && posX == width-1)
                    || (doors[2] && posX == width / 2 && posY == 0) || (doors[3] && posX == 0 && posY == height / 2)){
                
        }
    }

    private void Initialise(){

        //Load enemies to list
        GameObject enemyParentObj = this.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }

        //Load interactables to list
        GameObject interactableParentObj = this.gameObject.transform.Find("Interactable").gameObject;
        foreach(Transform child in interactableParentObj.transform){
            if(child.gameObject.activeSelf){
                interactables.Add(child.gameObject);
            }
        }

        //Load obstacles to list
        GameObject obstacleParentObj = this.gameObject.transform.Find("Obstacles").gameObject;
        foreach(Transform child in obstacleParentObj.transform){
            if(child.gameObject.activeSelf){
                obstacles.Add(child.gameObject);
            }
        }

        //Load floor to list
        GameObject floorParentObj = this.gameObject.transform.Find("Floor").gameObject;
        foreach(Transform child in floorParentObj.transform){
            if(child.gameObject.activeSelf){
                floor.Add(child.gameObject);
            }
        }
    }

    private void updateEnemies(){
        enemies.Clear();
        GameObject enemyParentObj = this.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }
    }

    public void StartFight(){
        this.isFighting = true;
    }

    public void EndFight(){
        this.isFighting = false;
    }

    public void NextRoom(bool[] doors){
        this.doors = doors;
        Debug.Log(doors[0].ToString() + doors[1].ToString() + doors[2].ToString() + doors[3].ToString());
        //TODO creating next room
    }
}
