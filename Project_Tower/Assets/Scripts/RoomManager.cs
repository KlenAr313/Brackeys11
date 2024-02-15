using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private LevelManager levelManagerScript;

    [SerializeField] private GameObject roomLayout;

    [SerializeField] private bool[] doors = {true, true, false, true};
    [SerializeField] private int height;
    [SerializeField] private int width;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;

    private RoomData roomData;
    private int prevType;

    public void NewRoom(RoomData data)
    {
        this.levelManagerScript = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        this.tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        
        this.gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        generateRoomFromLayout(data);
    }

    //Main click entry point
    public void TileClicked(int posX, int posY, bool isAttack){
        
        if(isAttack){
            foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                if(enemyBaseScript == null){
                    Debug.Log("Enemy script null");
                }
                if(enemyBaseScript.PosX == posX && enemyBaseScript.PosY == posY){
                    //TODO Majd player damage kell az argumentumba
                    enemyBaseScript.GetDamaged(gameManagerScript.playerScript.GetFinalDamage());
                }
            }
            updateEnemies();
        }
        else if(doors[0] && posY == height-1 && posX == width / 2)
                levelManagerScript.OpenDoor(0);
        else if(doors[1] && posY == height / 2 && posX == width-1 )
                levelManagerScript.OpenDoor(1);
        else if(doors[2] && posX == width / 2 && posY == 0)
                levelManagerScript.OpenDoor(2);
        else if(doors[3] && posX == 0 && posY == height / 2)
                levelManagerScript.OpenDoor(3);
        
    }

    

    private void Initialise()
    {
        if(roomData.IsWon)
        {
            GameObject.DestroyImmediate(GameObject.Find("Enemies"), true);
        }
        else
        {
            enemies.Clear();
            //Load enemies to list
            GameObject enemyParentObj = roomLayout.gameObject.transform.Find("Enemies").gameObject;
            foreach(Transform child in enemyParentObj.transform){
                if(child.gameObject.activeSelf){
                    enemies.Add(child.gameObject);
                }
            }
        }

        interactables.Clear();
        //Load interactables to list
        GameObject interactableParentObj = roomLayout.gameObject.transform.Find("Interactables").gameObject;
        foreach(Transform child in interactableParentObj.transform){
            if(child.gameObject.activeSelf){
                interactables.Add(child.gameObject);
            }
        }

        obstacles.Clear();
        //Load obstacles to list
        GameObject obstacleParentObj = roomLayout.gameObject.transform.Find("Obstacles").gameObject;
        foreach(Transform child in obstacleParentObj.transform){
            if(child.gameObject.activeSelf){
                obstacles.Add(child.gameObject);
            }
        }

        floor.Clear();
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

    public void NextRoom(RoomData data){
        GameObject.DestroyImmediate(GameObject.Find("Room Layout " + prevType + "(Clone)"), true);
        generateRoomFromLayout(data);
        Debug.Log(doors[0].ToString() + doors[1].ToString() + doors[2].ToString() + doors[3].ToString());
        //TODO reloading next room
    }

    private void generateRoomFromLayout(RoomData data)
    {
        this.roomData = data;
        this.roomLayout = GameObject.Instantiate(Resources.Load<GameObject>("Room Layout " + data.Type));
        roomLayout.transform.parent = this.transform;

        prevType = data.Type;
        this.doors = data.Doors;
        tileManagerScript.NewTiles(width,height, doors);

        foreach (AbstractInteractable item in data.Interactables)
        {
            if(item is Chest){
                GameObject interactableGameObj = GameObject.Instantiate(Resources.Load<GameObject>("Interactables/ChestObj"));
                interactableGameObj.transform.position = item.Pos;
                interactableGameObj.transform.parent = GameObject.Find("Interactables").transform;
            }
        }
        Initialise();

        

        if(enemies.Count > 0)
            gameManagerScript.StartFight();

    }


    public List<GameObject> GetAllEnemies(){
        List<GameObject> enemiesCopy = new List<GameObject>();

        foreach(GameObject enemy in enemies){
            enemiesCopy.Add(enemy);
        }

        return enemiesCopy;
    }

    internal void WinFight()
    {
        this.roomData.IsWon = true;
    }
}
