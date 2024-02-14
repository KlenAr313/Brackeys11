using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private LevelManager levelManagerScript;

    [SerializeField] private GameObject roomLayout;

    [SerializeField] private bool[] doors = {true, true, false, true};
    [SerializeField] private int height;
    [SerializeField] private int width;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;

    private int prevType;

    public void NewRoom(bool[] doors, int type)
    {
        this.levelManagerScript = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        this.tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        this.roomLayout = Resources.Load<GameObject>("Room Layout " + type);
        Instantiate(roomLayout);
        //this.roomLayout = GameObject.Find("Room Layout " + type);
        prevType = type;
        this.doors = doors;
        tileManagerScript.NewTiles(width,height, doors);

        Initialise();
    }

    //Main click entry point
    public void TileClicked(int posX, int posY, bool isAttack){
        
        if(isAttack){
            foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                if(enemyBaseScript.PosX == posX && enemyBaseScript.PosY == posY){
                    //TODO Majd player damage kell az argumentumba
                    enemyBaseScript.GetDamaged(2);
                }
            }
            updateEnemies();
        }
        else if(doors[0] && posY == height-1 && posX == width / 2)
                levelManagerScript.OpenDoor(0);
        else if(doors[1] && posY == height / 2 && posX == width-1)
                levelManagerScript.OpenDoor(1);
        else if(doors[2] && posX == width / 2 && posY == 0)
                levelManagerScript.OpenDoor(2);
        else if(doors[3] && posX == 0 && posY == height / 2)
                levelManagerScript.OpenDoor(3);
        
    }

    

    private void Initialise(){

        //Load enemies to list
        GameObject enemyParentObj = roomLayout.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }

        //Load interactables to list
        GameObject interactableParentObj = roomLayout.gameObject.transform.Find("Interactable").gameObject;
        foreach(Transform child in interactableParentObj.transform){
            if(child.gameObject.activeSelf){
                interactables.Add(child.gameObject);
            }
        }

        //Load obstacles to list
        GameObject obstacleParentObj = roomLayout.gameObject.transform.Find("Obstacles").gameObject;
        foreach(Transform child in obstacleParentObj.transform){
            if(child.gameObject.activeSelf){
                obstacles.Add(child.gameObject);
            }
        }

        //Load floor to list
        GameObject floorParentObj = roomLayout.gameObject.transform.Find("Floor").gameObject;
        foreach(Transform child in floorParentObj.transform){
            if(child.gameObject.activeSelf){
                floor.Add(child.gameObject);
            }
        }
    }

    private void updateEnemies(){
        enemies.Clear();
        GameObject enemyParentObj = roomLayout.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }
    }

    public void NextRoom(bool[] doors, int type){
        this.doors = doors;
        Debug.Log(doors[0].ToString() + doors[1].ToString() + doors[2].ToString() + doors[3].ToString());
        tileManagerScript.NewTiles(width,height, doors);
        GameObject.DestroyImmediate(GameObject.Find("Room Layout " + prevType + "(Clone)"), true);
        this.roomLayout = Resources.Load<GameObject>("Room Layout " + type);
        Instantiate(roomLayout);
        prevType = type;
        //TODO reloading next room
    }
}
