using System;
using System.Collections;
using System.Collections.Generic;
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


        //Load enemies to list
        GameObject enemyParentObj = this.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in transform){
            enemies.Add(child.gameObject);
        }
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
}
