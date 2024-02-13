using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private RoomManager roomManagerScript;

    private Dictionary<Vector2, Tile> _tiles;

    [SerializeField] private bool[] doors = {true, true, true, true};

    void Start()
    {
        this.roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
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

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, this.gameObject, x, y);

                
                if((doors[0] && y == _height-1 && x == _width / 2) || (doors[1] && y == _height / 2 && x == _width-1)
                    || (doors[2] && x == _width / 2 && y == 0) || (doors[3] && x == 0 && y == _height / 2))
                {
                    SpriteRenderer spriteRenderer = spawnedTile.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.blue;
                    spawnedTile.SetDoor(true);
                }
                else if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                {
                    SpriteRenderer spriteRenderer = spawnedTile.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.gray;
                    spawnedTile.SetEdge(true);
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
        roomManagerScript.TileClicked(posX, posY);
    }
}