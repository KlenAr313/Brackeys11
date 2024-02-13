using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private TileManager _TileManagerPrefab;

    public int RoomCounter;
    private int RoomLeft;
    // Start is called before the first frame update
    private int N;

    private int[,] RoomsGrid;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;

    private bool isFighting;

    void Start()
    {
        N = RoomCounter / 2;
        RoomLeft = RoomCounter;
        int x = UnityEngine.Random.Range(0, N);
        int y = UnityEngine.Random.Range(0, N);
        RoomsGrid = new int[N,N];
        for(int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                RoomsGrid[i,j] = 0;
            }
        }
        RandomizeRooms(x, y, 2);

        string s = "";

        for(int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                s += RoomsGrid[i,j];
            }
            Debug.Log(s);
            s = "";
        }
        
        //Debug miatt true, false legyen alapból!
        isFighting = true;

        Initialise();
    }

    private bool RandomizeRooms(int x, int y, int type)
    {
        if(x >= 0 && x < N && y >= 0 && y < N && RoomLeft > 0){
            if(RoomLeft == 1)
                type = 3;
            bool newRoom = true;
            if(RoomsGrid[x,y] == 0)
            {
                RoomsGrid[x,y] = type;
                RoomLeft--;
                newRoom = false;
            }
            bool reValue = false;
            int bySide = 2;
            int tryCount = 4;
            while(bySide > 0 && tryCount > 0){
                switch(UnityEngine.Random.Range(0,4)){
                    case 0: 
                        reValue = RandomizeRooms(x-1,y, 1);
                        break;
                    case 1: 
                        reValue = RandomizeRooms(x+1,y, 1);
                        break;
                    case 2: 
                        reValue = RandomizeRooms(x,y-1, 1);
                        break;
                    case 3: 
                        reValue = RandomizeRooms(x,y+1, 1);
                        break;
                }
                tryCount--;
                if(reValue){
                    bySide--;
                    tryCount = 3;
                    reValue = false;
                }
            }
            return newRoom;
        }
        return false;
    }

    //Main click entry point
    public void TileClicked(int posX, int posY){
        if(isFighting){
            foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                if(enemyBaseScript.PosX == posX && enemyBaseScript.PosY == posY){
                    //Majd player damage kell az argumentumba
                    enemyBaseScript.GetDamaged(2);
                }
            }
            updateEnemies();
        }
        else{

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
}
