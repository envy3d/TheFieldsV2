using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public class MapScript : MonoBehaviour
{
    public Direction startDirection = Direction.N;
    public bool randomLandMines = false;
    public int numOfRandMines = 0;

    private GameManagerScript gms;
    private BunnyManagerScript bms;
    private TileScript[,] tiles;
    private List<TileScript> landmineTiles;
    private int tileOffsetX, tileOffsetZ;
    private int mapHeight, mapWidth;

	void Start()
    {
        gms = GetComponent<GameManagerScript>();
        bms = GetComponent<BunnyManagerScript>();
        InitializeMap();
        if (randomLandMines)
        {
            RandomLandMines();
        }
        InitializeSpecialTiles();
        UpdateTileMineProximity();
	}

    public bool QueryTilePassability(int x, int z)
    {
        if (x - tileOffsetX < 0 || x - tileOffsetX >= tiles.GetLength(0))
            return false;
        if (z - tileOffsetZ < 0 || z - tileOffsetZ >= tiles.GetLength(1))
            return false;
        if (tiles[x - tileOffsetX, z - tileOffsetZ].QueryPassability())
        {
            var tempTile = tiles[x - tileOffsetX, z - tileOffsetZ];
            if ((tempTile.properties & TileProperty.Finish) == TileProperty.Finish)
                gms.PlayerReachedFinish();

            return true;
        }
        else
            return false;
    }

    public TileProperty QueryTileType(int x, int z)
    {
        return tiles[x - tileOffsetX, z - tileOffsetZ].properties;
    }

    public bool DetonateTile(int x, int z)
    {
        bms.StartLocationClear();
        if (tiles[x - tileOffsetX, z - tileOffsetZ].Detonate())
        {
            landmineTiles.Remove(tiles[x - tileOffsetX, z - tileOffsetZ]);
            UpdateTileMineProximity();
            return true;
        }

        return false;
    }

    public bool DigTile(int x, int z)
    {
        return tiles[x - tileOffsetX, z - tileOffsetZ].Dig();
    }

    private void InitializeMap()
    {
        GameObject[] gameTiles = GameObject.FindGameObjectsWithTag("Tile");
        int mapNegX = (int)gameTiles[0].transform.position.x, mapPosX = (int)gameTiles[0].transform.position.x;
        int mapNegZ = (int)gameTiles[0].transform.position.z, mapPosZ = (int)gameTiles[0].transform.position.z;
        foreach (GameObject tile in gameTiles)
        {
            if (tile.transform.position.x < mapNegX)
                mapNegX = (int)tile.transform.position.x;
            else if (tile.transform.position.x > mapPosX)
                mapPosX = (int)tile.transform.position.x;
            if (tile.transform.position.z < mapNegZ)
                mapNegZ = (int)tile.transform.position.z;
            else if (tile.transform.position.z > mapPosZ)
                mapPosZ = (int)tile.transform.position.z;
        }

        this.mapHeight = mapPosZ - mapNegZ + 1;
        this.mapWidth = mapPosX - mapNegX + 1;
        tileOffsetX = mapNegX;
        tileOffsetZ = mapNegZ;


        tiles = new TileScript[mapPosX - mapNegX + 1, mapPosZ - mapNegZ + 1];
        landmineTiles = new List<TileScript>(1);
        TileScript temp;
        foreach (GameObject tile in gameTiles)
        {
            temp = tile.GetComponent<TileScript>();
            if (temp != null)
            {
                tiles[(int)tile.transform.position.x - mapNegX, (int)tile.transform.position.z - mapNegZ] = temp;
            }
        }
    }

    private void InitializeSpecialTiles()
    {
        foreach (TileScript tile in tiles)
        {
            if ((tile.properties & TileProperty.Landmine) == TileProperty.Landmine)
            {
                landmineTiles.Add(tile);
            }
            if ((tile.properties & TileProperty.Start) == TileProperty.Start)
            {
                gms.SetStartLocation(new Vector2((int)tile.transform.position.x, (int)tile.transform.position.z), CardinalDirections.GetVector2(startDirection));
            }
        }
    }

    private void RandomLandMines()
    {
        int xLoc, yLoc;
        for (int numberOfMines = 0; numberOfMines < numOfRandMines; )
        {
            xLoc = Random.Range(0, mapWidth - 1);
            yLoc = Random.Range(0, mapHeight - 1);
            if ((tiles[xLoc, yLoc].properties & TileProperty.Traversable) == TileProperty.Traversable)
            {
                tiles[xLoc, yLoc].properties = tiles[xLoc, yLoc].properties | TileProperty.Landmine;
                ++numberOfMines;
            }
        }
    }

    private void UpdateTileMineProximity()
    {
        int tempDist = 0;
        foreach (TileScript tile in tiles)
        {
            tile.ProximityToMine = 10;
            foreach (TileScript mine in landmineTiles)
            {
                tempDist = Mathf.CeilToInt(Vector3.Distance(mine.transform.position, tile.transform.position));
                if (tile.ProximityToMine > tempDist)
                {
                    tile.ProximityToMine = tempDist;
                }
            }
        }
    }

}
