using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameManager gameManagerScript;

    private Dictionary<Vector2, Tile> _tiles;

    private bool[] doors;

    public void NewTiles(int width, int height, bool[] doors)
    {
        this.gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _width = width;
        _height = height;
        this.doors = doors;
        if(_tiles != null)
            _tiles.Clear();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.SetPosX(x);
                spawnedTile.SetPosY(y);
                spawnedTile.transform.parent = this.transform;
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.isHighlightable = true;

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, this.gameObject, x, y);

                
                if((doors[0] && y == _height-1 && x == _width / 2) || (doors[1] && y == _height / 2 && x == _width-1)
                    || (doors[2] && x == _width / 2 && y == 0) || (doors[3] && x == 0 && y == _height / 2))
                {
                    SpriteRenderer spriteRenderer = spawnedTile.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.blue;
                    spawnedTile.isHighlightable = false;
                }
                else if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                {
                    SpriteRenderer spriteRenderer = spawnedTile.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.gray;
                    spawnedTile.isHighlightable = false;
                }

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public void Click(int posX, int posY){
        gameManagerScript.TileClicked(posX, posY);
    }

    public void SetHightlightedTile(int posX, int posY){
        foreach(KeyValuePair<Vector2, Tile> tile in _tiles){
            tile.Value.highlight.SetActive(false);
        }
        gameManagerScript.TileHighlighter(posX, posY);
    }

    public void highlightSpellPreview(int posX, int posY){
        foreach(KeyValuePair<Vector2, Tile> tile in _tiles){
            if(tile.Value.getPosX() == posX && tile.Value.getPosY() == posY && tile.Value.isHighlightable){
                tile.Value.highlight.SetActive(true);
            }
        }
    }
}