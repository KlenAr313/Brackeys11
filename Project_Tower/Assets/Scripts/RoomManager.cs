using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private LevelManager levelManagerScript;

    [SerializeField] private GameObject roomLayout;

    [SerializeField] private bool[] doors = {true, true, false, true};
    [SerializeField] public int height;
    [SerializeField] public int width;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;

    private RoomData roomData;
    private int prevType;

    public void NewRoom(ref RoomData data)
    {
        this.levelManagerScript = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        this.tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        
        this.gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        generateRoomFromLayout(ref data);
    }

    //Main click entry point
    public void TileClicked(int posX, int posY, bool isAttack){
        if(isAttack){
            foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                /*
                if(enemyBaseScript == null){
                    Debug.Log("Enemy script null");
                }
                */
                if(enemyBaseScript.PosX == posX && enemyBaseScript.PosY == posY){
                    //Ne sebezzünk ha:
                    if(gameManagerScript.currentSpell.spellName != "Heal" && gameManagerScript.currentSpell.spellName != "Mana"){
                        enemyBaseScript.GetDamaged(gameManagerScript.playerScript.GetFinalDamage(), gameManagerScript.currentSpell.animationTime);
                    }
                }
            }

            if(posX == gameManagerScript.playerScript.PosX && posY == gameManagerScript.playerScript.PosY){
                if(gameManagerScript.currentSpell.spellName == "Heal"){
                    gameManagerScript.playerScript.GetHealed(gameManagerScript.playerScript.GetFinalDamage());
                    //Debug.Log("Healing: " + gameManagerScript.playerScript.GetFinalDamage());
                }
                else if(gameManagerScript.currentSpell.spellName == "Mana"){
                    gameManagerScript.playerScript.GiveMana(gameManagerScript.playerScript.GetFinalDamage());
                }
                else{
                    gameManagerScript.playerScript.GetDamaged(gameManagerScript.playerScript.GetFinalDamage(), gameManagerScript.currentSpell.animationTime);
                }
            }
        }
        else if(doors[0] && posY == height-1 && posX == width / 2)
                StartCoroutine(levelManagerScript.OpenDoor(0));
        else if(doors[1] && posY == height / 2 && posX == width-1 )
                StartCoroutine(levelManagerScript.OpenDoor(1));
        else if(doors[2] && posX == width / 2 && posY == 0)
                StartCoroutine(levelManagerScript.OpenDoor(2));
        else if(doors[3] && posX == 0 && posY == height / 2)
                StartCoroutine(levelManagerScript.OpenDoor(3));
        else{
            foreach(GameObject item in interactables){
                if(item.transform.position.x == posX && item.transform.position.y == posY){
                    item.GetComponent<IInteractable>().Click();
                }
            }
        }
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

        /*interactables.Clear();*/
        //Load interactables to list
        GameObject interactableParentObj = roomLayout.gameObject.transform.Find("Interactables").gameObject;
        foreach(Transform child in interactableParentObj.transform){
            if(child.gameObject.activeSelf && !interactables.Contains(child.gameObject)){
                interactables.Add(child.gameObject);
                Debug.Log("I added one element");
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
            if(child.gameObject.activeSelf)
            {
                if(doors[0] && child.transform.position.y == height-1 && child.transform.position.x == width / 2)
                    child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
                if (doors[1] && child.transform.position.y == height / 2 && child.transform.position.x == width-1)
                    child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da1"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
                if(doors[2] && child.transform.position.x == width / 2 && child.transform.position.y == 0)
                    child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da3"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
                if(doors[3] && child.transform.position.x == 0 && child.transform.position.y == height / 2)
                    child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da2"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
            
                floor.Add(child.gameObject);
            }
        }
    }

    public void RoomUpdateEnemies(){
        enemies.Clear();
        GameObject enemyParentObj = roomLayout.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }
    }

    public void NextRoom(ref RoomData data){
        Debug.Log("New Room Created");
        GameObject.DestroyImmediate(GameObject.Find("Room Layout " + prevType + "(Clone)"), true);
        generateRoomFromLayout(ref data);
        Debug.Log(doors[0].ToString() + doors[1].ToString() + doors[2].ToString() + doors[3].ToString());
        
        if(enemies.Count > 0)
            gameManagerScript.StartFight();
    }

    private void generateRoomFromLayout(ref RoomData data)
    {
        this.roomData = data;
        this.roomLayout = GameObject.Instantiate(Resources.Load<GameObject>("Room Layout " + data.Type));
        roomLayout.transform.parent = this.transform;

        prevType = data.Type;
        this.doors = data.Doors;
        tileManagerScript.NewTiles(width,height, doors);

        interactables.Clear();
        for(int i = 0; i < data.Interactables.Count; ++i){
            if(data.Interactables[i] is Chest){
                GameObject interactableGameObj = Resources.Load<GameObject>("Interactables/ChestObj");
                if(!data.IsWon && data.Type != 1){
                    List<Vector2Int> vects = gameManagerScript.tileManagerScript.GetPlayableArea();
                    ((Chest)data.Interactables[i]).NewPos(vects[Random.Range(0, vects.Count)]);
                }
                interactableGameObj.transform.position = data.Interactables[i].Pos;
                interactableGameObj.GetComponent<Chest>().CopyData((Chest)data.Interactables[i]);
                interactableGameObj = GameObject.Instantiate(interactableGameObj);
                interactableGameObj.transform.parent = GameObject.Find("Interactables").transform;
                interactables.Add(interactableGameObj);
            }
        }
        Initialise();

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


    #region Utilities

    public string GetTileNameByCoord(int posX, int posY){

        foreach(GameObject obj in enemies){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return "enemy";
            }
        }

        foreach(GameObject obj in interactables){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return "interactable";
            }
        }

        foreach(GameObject obj in obstacles){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return "obsatcle";
            }
        }

        foreach(GameObject obj in floor){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return "floor";
            }
        }

        return null;
    }

    public GameObject GetTileGameObjectByCoord(int posX, int posY){
        
        foreach(GameObject obj in enemies){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return obj;
            }
        }

        foreach(GameObject obj in interactables){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return obj;
            }
        }

        foreach(GameObject obj in obstacles){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return obj;
            }
        }

        foreach(GameObject obj in floor){
            if(obj.transform.position.x == posX && obj.transform.position.y == posY){
                return obj;
            }
        }

        return null;
    }


    #endregion
}
