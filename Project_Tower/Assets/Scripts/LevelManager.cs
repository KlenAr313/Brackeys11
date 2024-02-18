using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private RoomManager roomManagerScript;
    [SerializeField] private int RoomCounter;
    private int RoomLeft;
    private int N;
    private RoomData[,] RoomsGrid;
    private int CurrentRow;
    private int CurrentCol;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();

        RoomData.cluePool = new List<string>{
        "Behind the left portal there are brain eaters",
        "Don't choose the middle portal",
        "When the moon is full, the nights are cold",
        "Not many people return from the right one",
        "If you are greedy, don't choose the left portal",
        "In all three portals, you will be alone",
        "All three portals are under construction...",
        "Keep your health high for the right portal",
        "Keep your mana high for the middle portal",
        "Choose the left because that's the best",
        "The blind enemies are stuuuupid, don't trust them",
        "Kill the healer for quick success",
        "Maybe the clues can lie?"
        };

        N = RoomCounter / 2;
        RoomLeft = RoomCounter;
        CurrentRow = UnityEngine.Random.Range(0, N);
        CurrentCol = UnityEngine.Random.Range(0, N);
        RoomsGrid = new RoomData[N,N];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                RoomsGrid[i,j] = new RoomData();
            }
        }
        RandomizeRooms(CurrentRow, CurrentCol, 1);

#if DEBUG
        string s = "";

        for(int i = 0; i < N; i++)
        {
            for(int j = 0; j < N; j++)
            {
                s += RoomsGrid[i,j]+ " ";
            }
            //Debug.Log(s);
            s = "";
        }
#endif

        bool[] doors = new bool[4];
        doors[0] = CurrentRow - 1 >= 0 && RoomsGrid[CurrentRow-1,CurrentCol].Type != 0;
        doors[1] = CurrentCol + 1 < N && RoomsGrid[CurrentRow,CurrentCol+1].Type != 0;
        doors[2] = CurrentRow + 1 < N && RoomsGrid[CurrentRow+1,CurrentCol].Type != 0;
        doors[3] = CurrentCol - 1 >= 0 && RoomsGrid[CurrentRow,CurrentCol-1].Type != 0;
        RoomsGrid[CurrentRow, CurrentCol].Doors = doors;
        roomManagerScript.NewRoom(ref RoomsGrid[CurrentRow, CurrentCol]);

    }

    private bool RandomizeRooms(int x, int y, int type)
    {
        if(x >= 0 && x < N && y >= 0 && y < N && RoomLeft > 0){
            if(RoomLeft == 1)
                type = 10;
            bool newRoom = false;
            if(RoomsGrid[x,y].Type == 0)
            {
                RoomsGrid[x,y] = new RoomData(type);
                RoomLeft--;
                newRoom = true;
            }
            bool reValue = false;
            int tryCount = 4;
            while(RoomLeft > 0 && tryCount > 0){
                switch(UnityEngine.Random.Range(0,4)){
                    case 0: 
                        reValue = RandomizeRooms(x-1,y, Random.Range(2,4));
                        break;
                    case 1: 
                        reValue = RandomizeRooms(x+1,y, Random.Range(2,4));
                        break;
                    case 2: 
                        reValue = RandomizeRooms(x,y-1, Random.Range(2,4));
                        break;
                    case 3: 
                        reValue = RandomizeRooms(x,y+1, Random.Range(2,4));
                        break;
                }
                if(reValue){
                    tryCount--;
                    reValue = false;
                }
            }
            return newRoom;
        }
        return false;
    }

    public IEnumerator OpenDoor(int doorI){
        GameObject.Find("Game Manager").GetComponent<GameManager>().canClick = false;
        GameObject.Find("Game Manager").GetComponent<FadeSystem>().OpenDoor();
        yield return new WaitForSeconds(1.7f);
        int NextRow = CurrentRow;
        int NextCol = CurrentCol;
        switch (doorI)
        {
            case 0:
                NextRow--;
                gameManagerScript.playerScript.SetPosition(gameManagerScript.roomManagerScript.width/2,2);
                gameManagerScript.playerScript.transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case 1:
                NextCol++;
                gameManagerScript.playerScript.SetPosition(2,gameManagerScript.roomManagerScript.height/2);
                gameManagerScript.playerScript.transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case 2:
                NextRow++;
                gameManagerScript.playerScript.SetPosition(gameManagerScript.roomManagerScript.width/2,gameManagerScript.roomManagerScript.height-3);
                gameManagerScript.playerScript.transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case 3:
                NextCol--;
                gameManagerScript.playerScript.SetPosition(gameManagerScript.roomManagerScript.width-3,gameManagerScript.roomManagerScript.height/2);
                gameManagerScript.playerScript.transform.rotation = Quaternion.Euler(0,180,0);
                break;
            default:
                break;
        }

        if(NextRow >=0 && NextCol >= 0 && NextRow < N && NextCol < N && RoomsGrid[NextRow,NextCol].Type != 0){
            CurrentRow = NextRow;
            CurrentCol = NextCol;
            bool[] doors = new bool[4];
            doors[0] = CurrentRow - 1 >= 0 && RoomsGrid[CurrentRow-1,CurrentCol].Type != 0;
            doors[1] = CurrentCol + 1 < N && RoomsGrid[CurrentRow,CurrentCol+1].Type != 0;
            doors[2] = CurrentRow + 1 < N && RoomsGrid[CurrentRow+1,CurrentCol].Type != 0;
            doors[3] = CurrentCol - 1 >= 0 && RoomsGrid[CurrentRow,CurrentCol-1].Type != 0;
            RoomsGrid[CurrentRow, CurrentCol].Doors = doors;
            roomManagerScript.NextRoom(ref RoomsGrid[CurrentRow, CurrentCol]);
        }

        
        yield return new WaitForSeconds(1.3f);
        GameObject.Find("Game Manager").GetComponent<GameManager>().canClick = true;
    }

}
