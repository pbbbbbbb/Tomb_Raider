using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomGenerator : MonoBehaviour
{
// -------------------------- public variables --------------------------
    public enum Direction { up, down, left, right };
    public Direction direction;

    public GameObject bossRoomPrefab, entryRoomPrefab, enemyRoomPrefab, propRoomPrefab, hidenRoomPrefab;
    public GameObject coin, spikeWeed;
    
    [Header("Room information")]
    public int roomNumber;
    public Color startColor, endColor;

    [Header("Position control")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public List<Room> rooms = new List<Room>();
    public WallType wallType;
    public PropType propType;

    // -------------------------- private variables --------------------------
    private const int INF = 100;
    private const int maxn = 100;
    private const int idxOffset = 35; // Offset of room *index*
    private float minx, miny, maxx, maxy;
    private int[,] RoomPos = new int[100, 100]; // Position of each room (0..roomNumber-1)
    private int[,] Graph = new int[35, 35]; // graph of room 0..roomNumber-1
    private int[,] Dist = new int[35, 35];  // Distance (for floyd)
    private Room startRoom, endRoom;
    private enum RoomType{ boss, entry, enemy, prop, hiden };
    private RoomType roomType;

// -------------------------- Start --------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Initializing roomNo & graph
        for(int i = 0; i < maxn; i ++)
            for(int j = 0; j < maxn; j ++)
                RoomPos[i, j] = -1;
        for(int i = 0; i < idxOffset; i ++)
            for(int j = 0; j < idxOffset; j ++)
            {
                Graph[i, j] = 0;
                Dist[i, j] = INF;
            }

        // Generate room
        for(int i = 0; i < roomNumber; i ++)
        {
            GenerateRoom(i);
        }

        // Set up rooms
        foreach(var room in rooms)
        {
            SetupRoom(room, room.transform.position);
        }

        // Find start & end room (max distance between them)
        Floyd();
        int u, v, maxDist;
        u = v = maxDist = 0;
        for(int i = 0; i < roomNumber; i ++)
            for(int j = 0; j < i; j ++)
            {
                if(Dist[i, j] < INF && Dist[i, j] > maxDist)
                {
                    u = i;  v = j;
                    maxDist = Dist[i, j];
                }
            }
        startRoom = rooms[u];
        endRoom = rooms[v];
        // Change color of start/end rooms
        startRoom.GetComponent<SpriteRenderer>().color = startColor;
        endRoom.GetComponent<SpriteRenderer>().color = endColor;

        // Set player's position to center of start room
        PlayerController.instance.SetPlayerPosition(startRoom.transform.position);
    }

// -------------------------- Update --------------------------
    // Update is called once per frame
    void Update()
    {
        
    }

// -------------------------- other functions --------------------------
    // Generate new room
    public void GenerateRoom(int i)
    {
        // Generate room
        // roomType = (RoomType)UnityEngine.Random.Range(0, 5);
        roomType = RoomType.enemy;  // TEST
        switch(roomType)
        {
            case RoomType.boss:
                rooms.Add(Instantiate(bossRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<BossRoom>());
                break;
            case RoomType.entry:
                rooms.Add(Instantiate(entryRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<EntryRoom>());
                break;
            case RoomType.enemy:
                rooms.Add(Instantiate(enemyRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<EnemyRoom>());
                break;
            case RoomType.prop:
                rooms.Add(Instantiate(propRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<PropRoom>());
                break;
            case RoomType.hiden:
                rooms.Add(Instantiate(hidenRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<HidenRoom>());
                break;
        }

        // Set position & layout of the room
        rooms[i].setPosition(generatorPoint.position);
        rooms[i].GenerateLayout(coin, spikeWeed);

        // Record the position in RoomPos
        int xidx = x2idx(generatorPoint.position.x);
        int yidx = y2idx(generatorPoint.position.y);
        RoomPos[xidx, yidx] = i;

        // Get Graph for floyd
        minx = Math.Min(minx, generatorPoint.position.x);
        miny = Math.Min(miny, generatorPoint.position.y);
        maxx = Math.Max(maxx, generatorPoint.position.x);
        maxy = Math.Max(maxy, generatorPoint.position.y);

        // Change position for generating next room (no repetition)
        do {
            ChangePointPos();
            xidx = x2idx(generatorPoint.position.x);
            yidx = y2idx(generatorPoint.position.y);
        } while(RoomPos[xidx, yidx] >= 0);
    }

    // Change point position (UDLR randomly)
    public void ChangePointPos()
    {
        direction = (Direction)UnityEngine.Random.Range(0, 4);

        switch(direction)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0, yOffset, 0);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0, -yOffset, 0);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0, 0);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0, 0);
                break;
        }
    }

    // Position (float) to index (int)
    private int x2idx(double x)
    {
        return (int)(x / xOffset) + idxOffset;
    }

    private int y2idx(double y)
    {
        return (int)(y / yOffset) + idxOffset;
    }

    // Floyd algorithm
    private void Floyd()
    {
        for(int k = 0; k < roomNumber; k ++)
            for(int i = 0; i < roomNumber; i ++)
                for(int j = 0; j < roomNumber; j ++)
                    if(Dist[i, j] > Dist[i, k] + Dist[k, j])
                        Dist[i, j] = Dist[i, k] + Dist[k, j];
    }

    // Set up room
    //   1) Check adjacent rooms
    //   2) Set wall
    private void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        int xidx = x2idx(newRoom.gameObject.transform.position.x);
        int yidx = y2idx(newRoom.gameObject.transform.position.y);

        // up
        if(RoomPos[xidx, yidx + 1] >= 0)
        {
            newRoom.roomUp = true;
            int u = RoomPos[xidx, yidx], v = RoomPos[xidx, yidx + 1];
            Graph[u, v] = Graph[v, u] = 1;
            Dist[u, v] = Dist[v, u] = 1;
        }
        // down
        if (RoomPos[xidx, yidx - 1] >= 0)
        {
            newRoom.roomDown = true;
            int u = RoomPos[xidx, yidx], v = RoomPos[xidx, yidx - 1];
            Graph[u, v] = Graph[v, u] = 1;
            Dist[u, v] = Dist[v, u] = 1;
        }
        // left
        if (RoomPos[xidx - 1, yidx] >= 0)
        {
            newRoom.roomLeft = true;
            int u = RoomPos[xidx, yidx], v = RoomPos[xidx - 1, yidx];
            Graph[u, v] = Graph[v, u] = 1;
            Dist[u, v] = Dist[v, u] = 1;
        }
        // right
        if (RoomPos[xidx + 1, yidx] >= 0)
        {
            newRoom.roomRight = true;
            int u = RoomPos[xidx, yidx], v = RoomPos[xidx + 1, yidx];
            Graph[u, v] = Graph[v, u] = 1;
            Dist[u, v] = Dist[v, u] = 1;
        }

        newRoom.UpdateRoom();

        // Set walls
        switch(newRoom.getDoorNumber())
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.singleUp, roomPosition, Quaternion.identity);
                else if (newRoom.roomDown)
                    Instantiate(wallType.singleBottom, roomPosition, Quaternion.identity);
                else if (newRoom.roomLeft)
                    Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight)
                    Instantiate(wallType.singleRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomLeft && newRoom.roomUp)
                    Instantiate(wallType.doubleLU, roomPosition, Quaternion.identity);
                else if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                else if (newRoom.roomLeft && newRoom.roomDown)
                    Instantiate(wallType.doubleLB, roomPosition, Quaternion.identity);
                else if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                else if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.doubleRB, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (!newRoom.roomUp)
                    Instantiate(wallType.tripleLRB, roomPosition, Quaternion.identity);
                else if (!newRoom.roomDown)
                    Instantiate(wallType.tripleLUR, roomPosition, Quaternion.identity);
                else if (!newRoom.roomLeft)
                    Instantiate(wallType.tripleURB, roomPosition, Quaternion.identity);
                else if (!newRoom.roomRight)
                    Instantiate(wallType.tripleLUB, roomPosition, Quaternion.identity);
                break;
            case 4:
                Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity);
                break;
        }

        for (int i = 0; i < 6; ++i)
        {
            float c = UnityEngine.Random.value;
            if (c < 0.5) continue;
            float dy = UnityEngine.Random.Range(-3, 4);
            float dx = UnityEngine.Random.value + UnityEngine.Random.Range(-7, 7);
            Instantiate(propType.Heal, roomPosition + new Vector3(dx, dy, 0), Quaternion.identity);
        }

        for (int i = 0; i < 3; ++i)
        {
            float c = UnityEngine.Random.value;
            if (c < 0.5) continue;
            float dy = UnityEngine.Random.Range(-3, 4);
            float dx = UnityEngine.Random.value + UnityEngine.Random.Range(-7, 7);
            Instantiate(propType.bullet, roomPosition + new Vector3(dx, dy, 0), Quaternion.identity);
        }
    }
}

[System.Serializable]
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubleRB,
                      tripleLUR, tripleLUB, tripleURB, tripleLRB,
                      fourDoors;
}

[System.Serializable]
public class PropType
{
    public GameObject Heal,bullet;
}

