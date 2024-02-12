using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private int posX;
    private int posY;

    private bool isEdge;

    public void Init(bool isOffset, int posX, int posY)
    {
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

    public bool IsEdge()
    {
        return isEdge;
    }

    public void SetEdge(bool isEdge)
    {
        this.isEdge = isEdge;
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