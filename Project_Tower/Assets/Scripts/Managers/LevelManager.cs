using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private RoomManager roomManagerScript;
    [SerializeField] private int RoomCounter;
    private int N;
    private GameObject[,] RoomsGrid;
    private int CurrentRow;
    private int CurrentCol;

    // Only for level generation
    private int RoomLeft;

    public static LevelManager Instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        gameManagerScript = GameManager.Instance;
        roomManagerScript = RoomManager.Instance;
    }

    public void Start()
    {
        N = RoomCounter / 2;
        RoomLeft = RoomCounter;
        CurrentRow = UnityEngine.Random.Range(0, N);
        CurrentCol = UnityEngine.Random.Range(0, N);
        RoomsGrid = new GameObject[N,N];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                RoomsGrid[i,j] = null;
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
        doors[0] = CurrentRow - 1 >= 0 && RoomsGrid[CurrentRow-1,CurrentCol] != null;
        doors[1] = CurrentCol + 1 < N && RoomsGrid[CurrentRow,CurrentCol+1] != null;
        doors[2] = CurrentRow + 1 < N && RoomsGrid[CurrentRow+1,CurrentCol] != null;
        doors[3] = CurrentCol - 1 >= 0 && RoomsGrid[CurrentRow,CurrentCol-1] != null;
        RoomManager.Instance.NewRoom(ref RoomsGrid[CurrentRow, CurrentCol], doors);

    }

    public void OnValidate()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        if(Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private bool RandomizeRooms(int x, int y, int type)
    {
        if(x >= 0 && x < N && y >= 0 && y < N && RoomLeft > 0){
            if(RoomLeft == 1)
                type = 10;
            bool newRoom = false;
            if(RoomsGrid[x,y] == null)
            {
                RoomsGrid[x,y] = GameObject.Instantiate(Resources.Load<GameObject>("Room Layout " + type));;
                RoomsGrid[x,y].SetActive(false);
                RoomLeft--;
                newRoom = true;
            }
            bool reValue = false;
            int tryCount = 4;
            while(RoomLeft > 0 && tryCount > 0){
                switch(UnityEngine.Random.Range(0,4)){
                    case 0: 
                        reValue = RandomizeRooms(x-1,y, Random.Range(2,7));
                        break;
                    case 1: 
                        reValue = RandomizeRooms(x+1,y, Random.Range(2,7));
                        break;
                    case 2: 
                        reValue = RandomizeRooms(x,y-1, Random.Range(2,7));
                        break;
                    case 3: 
                        reValue = RandomizeRooms(x,y+1, Random.Range(2,7));
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
                break;
            case 1:
                NextCol++;
                break;
            case 2:
                NextRow++;
                break;
            case 3:
                NextCol--;
                break;
            default:
                break;
        }

        if(NextRow >=0 && NextCol >= 0 && NextRow < N && NextCol < N && RoomsGrid[NextRow,NextCol] != null){
            CurrentRow = NextRow;
            CurrentCol = NextCol;
            bool[] doors = new bool[4];
            doors[0] = CurrentRow - 1 >= 0 && RoomsGrid[CurrentRow-1,CurrentCol] != null;
            doors[1] = CurrentCol + 1 < N && RoomsGrid[CurrentRow,CurrentCol+1] != null;
            doors[2] = CurrentRow + 1 < N && RoomsGrid[CurrentRow+1,CurrentCol] != null;
            doors[3] = CurrentCol - 1 >= 0 && RoomsGrid[CurrentRow,CurrentCol-1] != null;
            roomManagerScript.NextRoom(ref RoomsGrid[CurrentRow, CurrentCol], doors);
        }

        
        yield return new WaitForSeconds(1.3f);
        GameObject.Find("Game Manager").GetComponent<GameManager>().canClick = true;
    }

}
