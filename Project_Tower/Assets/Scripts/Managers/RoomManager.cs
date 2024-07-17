using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private LevelManager levelManagerScript;
    [SerializeField] private Player playerScript;

    [SerializeField] private GameObject roomLayout;

    private GameObject[] doors = {null, null, null, null};
    [SerializeField] public int height;
    [SerializeField] public int width;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<GameObject> interactables;
    [SerializeField] private List<GameObject> floor;
    [SerializeField] private List<GameObject> wall;

    private Boundries roomBoundries;
    public Boundries RoomBoundries{
        get{
            return roomBoundries;
        } }

    public static RoomManager Instance;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        this.levelManagerScript = LevelManager.Instance;
        this.tileManagerScript = TileManager.Instance;
        this.gameManagerScript = GameManager.Instance;
        this.playerScript = Player.Instance;
    }

    public void OnValidate()
    {
        if(Instance == null){
            Instance = this;
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }
    }

    public void NewRoom(ref GameObject roomLayout, bool[] doorWays, int entryWay)
    {
        this.roomLayout = roomLayout;
        roomLayout.SetActive(true);
        Initialise(doorWays, entryWay);
    }

    public void NextRoom(ref GameObject roomLayout, bool[] doorWays, int entryWay){
        this.roomLayout.SetActive(false);
        this.roomLayout = roomLayout;
        //Debug.Log("New Room Created");
        roomLayout.SetActive(true);
        Initialise(doorWays, entryWay);
        //Debug.Log(doors[0].ToString() + doors[1].ToString() + doors[2].ToString() + doors[3].ToString());
        
        if(enemies.Count > 0)
            gameManagerScript.StartFight();
    }

    //Main click entry point
    public void TileClicked(int posX, int posY){
        if(doors[0] != null && doors[0].transform.position.x == posX && doors[0].transform.position.y == posY)
                StartCoroutine(levelManagerScript.OpenDoor(0));
        else if(doors[1] != null && doors[1].transform.position.x == posX && doors[1].transform.position.y == posY)
                StartCoroutine(levelManagerScript.OpenDoor(1));
        else if(doors[2] != null && doors[2].transform.position.x == posX && doors[2].transform.position.y == posY)
                StartCoroutine(levelManagerScript.OpenDoor(2));
        else if(doors[3] != null && doors[3].transform.position.x == posX && doors[3].transform.position.y == posY)
                StartCoroutine(levelManagerScript.OpenDoor(3));
        else{
            foreach(GameObject item in interactables){
                if(item.transform.position.x == posX && item.transform.position.y == posY){
                    item.GetComponent<IInteractable>().Click();
                }
            }
        }
    }

    public void TileClickedAttack(List<Vector2Int> coordList){

        //Looping though enemies in the effectd tiles
        foreach(GameObject enemy in enemies){
                EnemyBase enemyBaseScript = enemy.gameObject.GetComponent<EnemyBase>();
                Vector2Int enemyPos = new Vector2Int(enemyBaseScript.PosX, enemyBaseScript.PosY);
                if(coordList.Contains(enemyPos)){
                    //Ne sebezzünk ha:
                    if(Player.Instance.currentSpell.spellName != "Heal" && Player.Instance.currentSpell.spellName != "Mana"){
                        enemyBaseScript.GetDamaged(Player.Instance.currCharacter.GetFinalDamage(), Player.Instance.currentSpell.animationTime);
                    }
                }
            }
            
        //Looping though players in the effectd tiles
        foreach(Character character in Player.Instance.characterScritps){
            Vector2Int characterPos = new Vector2Int(character.PosX, character.PosY);

            if(coordList.Contains(characterPos)){
                if(Player.Instance.currentSpell.spellName == "Heal"){
                    character.GetHealed(Player.Instance.currCharacter.GetFinalDamage());
                    //Debug.Log("Healing: " + gameManagerScript.playerScript.GetFinalDamage());
                }
                else if(Player.Instance.currentSpell.spellName == "Mana"){
                    character.GiveMana(Player.Instance.currCharacter.GetFinalDamage());
                }
                else{
                    character.GetDamaged(Player.Instance.currCharacter.GetFinalDamage(), Player.Instance.currentSpell.animationTime);
                }
            }
        }
            
    }

    public void TileClickedMove(List<Vector2Int> coordList){

        
            
    }

    private void Initialise(bool[] doorWays, int entryWay)
    {
        enemies.Clear();
        //Load enemies to list
        GameObject enemyParentObj = roomLayout.gameObject.transform.Find("Enemies").gameObject;
        foreach(Transform child in enemyParentObj.transform){
            if(child.gameObject.activeSelf){
                enemies.Add(child.gameObject);
            }
        }

        interactables.Clear();
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
        float vMax = 5, hMax = 5, vMin = 5, hMin = 5;
        GameObject floorParentObj = roomLayout.gameObject.transform.Find("Floor").gameObject;
        foreach(Transform child in floorParentObj.transform){
            if(child.gameObject.activeSelf && child.gameObject.tag == "Floor")
            {
                if(child.gameObject.transform.position.y > vMax){
                    vMax = child.gameObject.transform.position.y;
                }
                else if (child.gameObject.transform.position.y < vMin){
                    vMin = child.gameObject.transform.position.y;
                }

                if(child.gameObject.transform.position.x > hMax){
                    hMax = child.gameObject.transform.position.x;
                }
                else if (child.gameObject.transform.position.x < hMin){
                    hMin = child.gameObject.transform.position.x;
                }

                floor.Add(child.gameObject);
            }
        }

        roomBoundries = new Boundries(vMax,hMax,vMin,hMin);
        Debug.Log(roomBoundries.VertMax + " " + roomBoundries.HorMax +" " + roomBoundries.VertMin + " " +roomBoundries.HorMin);

        for (int i = 0; i < 4; i++)
        {
            doors[i] = null;
        }
        //Debug.Log(doorWays[0].ToString() + doorWays[1].ToString()+doorWays[2].ToString()+doorWays[3].ToString());
        if(doorWays[0])
        {
            doors[0] = floorParentObj.gameObject.transform.Find("TopDoor").gameObject;
            doors[0].gameObject.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }
        if (doorWays[1])
        {
            doors[1] = floorParentObj.gameObject.transform.Find("RightDoor").gameObject;
            doors[1].gameObject.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da1"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }
        if(doorWays[2])
        {
            doors[2] = floorParentObj.gameObject.transform.Find("BottomDoor").gameObject;
            doors[2].gameObject.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da2"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }
        if(doorWays[3])
        {
            doors[3] = floorParentObj.gameObject.transform.Find("LeftDoor").gameObject;
            doors[3].gameObject.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("DoorPic/da3"), new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }

        GameObject entryParentObj = roomLayout.gameObject.transform.Find("EntryPoints").gameObject;
        Vector3 entryPoint;
        switch(entryWay)
        {
            case 0:
                entryPoint = entryParentObj.gameObject.transform.Find("Top").gameObject.transform.position;
                playerScript.characterScritps[0].SetPosition((int)entryPoint.x, (int)entryPoint.y);
                playerScript.characterScritps[1].SetPosition((int)entryPoint.x-1, (int)entryPoint.y+1);
                playerScript.characterScritps[2].SetPosition((int)entryPoint.x+1, (int)entryPoint.y+1);
                break;
            case 1:
                entryPoint = entryParentObj.gameObject.transform.Find("Right").gameObject.transform.position;
                playerScript.characterScritps[0].SetPosition((int)entryPoint.x, (int)entryPoint.y);
                playerScript.characterScritps[1].SetPosition((int)entryPoint.x+1, (int)entryPoint.y-1);
                playerScript.characterScritps[2].SetPosition((int)entryPoint.x+1, (int)entryPoint.y+1);
                break;
            case 2:
                entryPoint = entryParentObj.gameObject.transform.Find("Down").gameObject.transform.position;
                playerScript.characterScritps[0].SetPosition((int)entryPoint.x, (int)entryPoint.y);
                playerScript.characterScritps[1].SetPosition((int)entryPoint.x+1, (int)entryPoint.y-1);
                playerScript.characterScritps[2].SetPosition((int)entryPoint.x-1, (int)entryPoint.y-1);
                break;
            case 3:
                entryPoint = entryParentObj.gameObject.transform.Find("Left").gameObject.transform.position;
                playerScript.characterScritps[0].SetPosition((int)entryPoint.x, (int)entryPoint.y);
                playerScript.characterScritps[1].SetPosition((int)entryPoint.x-1, (int)entryPoint.y+1);
                playerScript.characterScritps[2].SetPosition((int)entryPoint.x-1, (int)entryPoint.y-1);
                break;
        }

        
        tileManagerScript.SetTiles(floor);
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

    public List<GameObject> GetAllEnemies(){
        List<GameObject> enemiesCopy = new List<GameObject>();

        foreach(GameObject enemy in enemies){
            enemiesCopy.Add(enemy);
        }
        
        return enemiesCopy;
    }


    #region Utilities

    // TODO: We are only looking for enemy with this func
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

    // TODO: We are only looking for enemy with this func
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


public struct Boundries
{
    private float vertMax;
    private float horMax; 
    private float vertMin; 
    private float horMin;

    public readonly float VertMax { 
        get{
        return vertMax;
        } }
    public readonly float HorMax { 
        get {
        return horMax;
        } }
    public readonly float VertMin { 
        get{
            return vertMin;
        } }
    public readonly float HorMin { 
        get{
            return horMin;
        } }

    public Boundries(float vertMax, float horMax, float vertMin, float horMin)
    {
        this.vertMax = vertMax;
        this.horMax = horMax;
        this.vertMin = vertMin;
        this.horMin = horMin;
    }
}

