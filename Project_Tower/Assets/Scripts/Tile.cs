using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    private TileManager tileManagerScript;

    private bool isEdge;

    private bool isDoor;

    public void Init(bool isOffset, GameObject tileManager, int posX, int posY)
    {
        this.tileManagerScript = tileManager.gameObject.GetComponent<TileManager>();
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        if (isEdge)
        {
            return;
        }
        _highlight.SetActive(true);
    }

    
    void OnMouseExit()
    {
        if (isEdge)
        {
            return;
        }
        _highlight.SetActive(false);
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            tileManagerScript.Click(this.posX, this.posY);
        }
    }

    public bool IsEdge()
    {
        return isEdge;
    }

    public void SetEdge(bool isEdge)
    {
        this.isEdge = isEdge;
    }

    public bool IsDoor(){
        return isDoor;
    }

    public void SetDoor(bool isDoor){
        this.isDoor = isDoor;
    }

    public int getPosX()
    {
        return posX;
    }

    public void SetPosX(int posX)
    {
        this.posX = posX;
    }

    public int getPosY()
    {
        return posY;
    }

    public void SetPosY(int posY)
    {
        this.posY = posY;
    }

}