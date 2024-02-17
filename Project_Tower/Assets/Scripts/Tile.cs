using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] public GameObject highlight;
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    private TileManager tileManagerScript;

    public bool isHighlightable = true;

    public void Init(bool isOffset, GameObject tileManager, int posX, int posY)
    {
        this.tileManagerScript = tileManager.gameObject.GetComponent<TileManager>();
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        if (!isHighlightable)
        {
            return;
        }
        //highlight.SetActive(true);
        tileManagerScript.SetHightlightedTile(posX, posY);
    }

    
    void OnMouseExit()
    {
        if (!isHighlightable)
        {
            return;
        }
        //highlight.SetActive(false);
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            tileManagerScript.Click(this.posX, this.posY);
        }
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