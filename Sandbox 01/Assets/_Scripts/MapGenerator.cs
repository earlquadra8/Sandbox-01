using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefab;

   
    public Vector2 floorSize;
    [Range(0, 1)]
    public float outlinePercent;
    public float tileSize = 1;
    //public Vector2[] selectedTile;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    Map currentMap;

    void Start ()
    {
		GenerateMap();
	}

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];

        allTileCoords = new List<Coord>();// get all tile Coord.
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), currentMap.seed));// then shuffle them, and put them into the queue.
        //currentMap.mapStart = new Coord((int)currentMap.mapSize.x / 2, (int)currentMap.mapSize.y / 2);// get the center tile Coord.

        #region Create parent object that holder all the map component.
        string mapholderName = "Map Holder";
        if (transform.Find(mapholderName))
        {
            DestroyImmediate(transform.Find(mapholderName).gameObject);
        }

        Transform mapHolder = new GameObject(mapholderName).transform;// Create mapHolder empty object.
        mapHolder.parent = transform;
        #endregion Create parent object that holder all the map component.


        #region Instantiate tiles
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                Vector3 tileSpawnPos = CoordToPosition(x, y);// tile(x, y) to world position.
                Transform newtile = Instantiate(tilePrefab, tileSpawnPos, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newtile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newtile.parent = mapHolder;
            }
        }
        #endregion Instantiate tiles

        #region Instantiate obstacle
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];//for checking whether a tile has an obstacle.

        int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
        int placeObstacleAttemptCount = 0;//number of obstacle placed.
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetShuffledCoord();// draw shuffled Coord here.

            if (SelectedTileIsAccessible(randomCoord))
            {
                obstacleMap[randomCoord.x, randomCoord.y] = true;
                placeObstacleAttemptCount++;
                
                if (MapIsFullyAccessible(obstacleMap, placeObstacleAttemptCount))
                {
                    Vector3 obstaclePostion = CoordToPosition(randomCoord.x, randomCoord.y);//make it world position
                    //accept and instantiate
                    Transform newObstacle = Instantiate(obstaclePrefab, obstaclePostion + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                    newObstacle.parent = mapHolder;
                }
                else
                {
                    //reject
                    obstacleMap[randomCoord.x, randomCoord.y] = false;
                    placeObstacleAttemptCount--;
                }
            }
        }
        #endregion Instantiate obstacle

        #region Navmesh mask for the map

        Transform maskLeft = Instantiate(navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + floorSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((floorSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y) * tileSize;

        Transform maskRight = Instantiate(navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + floorSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((floorSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y) * tileSize;

        Transform maskTop = Instantiate(navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + floorSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(floorSize.x, 1, (floorSize.y - currentMap.mapSize.y) / 2) * tileSize;

        Transform maskBottom = Instantiate(navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + floorSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(floorSize.x, 1, (floorSize.y - currentMap.mapSize.y) / 2) * tileSize;

        navmeshFloor.localScale = new Vector3(floorSize.x, floorSize.y) * tileSize;
        #endregion Navmesh mask for the map
    }

    #region rules for placing an obstacle
    bool SelectedTileIsAccessible(Coord randomCoord)
    {
        bool notOnselected = true;
        for (int i = 0; i < currentMap.selectedTile.Length; i++)
        {
            if (randomCoord == currentMap.selectedTile[i])
            {
                notOnselected = false;
                break;
            }
        }
        return notOnselected;
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int placeObstacleAttemptCount)
    {
        bool[,] mapChecked = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];//size equal obstacle map.
        Queue<Coord> accessibeTileQueue = new Queue<Coord>();
        accessibeTileQueue.Enqueue(currentMap.selectedTile[0]);//load center tile Coord
        mapChecked[currentMap.selectedTile[0].x, currentMap.selectedTile[0].y] = true;

        int accessibleTileCount = 1;//tile can go to, 1 for the start tile is accessible.

        while (accessibeTileQueue.Count > 0)
        {
            Coord tile = accessibeTileQueue.Dequeue();

            for (int x = -1; x <= 1; x++)//x = -1 == left side neighbour.
            {
                for (int y = -1; y <= 1; y++)//y = -1 == lower side neighbour.
                {
                    int neighbourX = tile.x + x;//get the Coord of the neighbour tile, checkingTile Coord + relativeCoord.
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)// only check side, no diagonal, do nothing for diagonal.
                    {   //if neighbour tile not outside the tile map.
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {       //not checked                          has no obstacle is placed or placing on it
                            if (!mapChecked[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY])
                            {
                                mapChecked[neighbourX, neighbourY] = true;
                                accessibeTileQueue.Enqueue(new Coord(neighbourX, neighbourY));// put neighbour tile to path queue for later check.
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - placeObstacleAttemptCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }
    #endregion rules for placing an obstacle

    Vector3 CoordToPosition(int x, int y)//Coord = the coord in the grid, this makes it into the world postion.
    {
        return new Vector3(-currentMap.mapSize.x / 2 + 0.5f + x, 0, -currentMap.mapSize.y / 2 + 0.5f + y) * tileSize;
    }

    public Coord GetShuffledCoord()// get those shuffled Coord from the array which put into the queue.
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);// rotating
        return randomCoord;
    }

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }

    }

    [System.Serializable]
    public class Map
    {
        public Coord mapSize;
        [Range(0,1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foregroungColor;
        public Color backgroundColor;

        public Coord[] selectedTile;
    }

}
