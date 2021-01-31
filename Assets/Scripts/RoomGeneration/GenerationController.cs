using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GuardSpawn
{
    public Vector3 pos;
    public float rot;
    public GuardSpawn(Vector3 pos, float rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
}

public class PatrolSpawn
{
    public PatrolRoute route;

    public PatrolSpawn(PatrolRoute route)
    {
        this.route = route;
    }
}

[Serializable]
public class RoomItem
{
    public GameObject prefab;
    public bool canSpawnAsSpecialRoom;
    public int weight;
}

public class WeightedRoomList
{
    List<RoomItem> list;

    public WeightedRoomList()
    {
        list = new List<RoomItem>();
    }

    public void Add(RoomItem item)
    {
        list.Add(item);
    }

    
}

public class GenerationController : MonoBehaviour
{
    public static GenerationController instance;
    public List<RoomItem> roomPrefabs;
    public GameObject[] patrolPrefabs;
    public GameObject[] guardPrefabs;

    public GameObject[] stairsPrefabs;
    public GameObject girlPrefab;
    public GameObject playerPrefab;

    [HideInInspector]
    public List<Room> rooms;
    public int maxRoomCount;

    public List<GuardSpawn> guardSpawnList;
    public List<PatrolSpawn> patrolSpawnList;

    private Room lastSpawnedRoom;
    private Vector3 stairLocation;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GenerateRooms();
        }
    }

    private void GenerateRooms()
    {
        guardSpawnList = new List<GuardSpawn>();
        patrolSpawnList = new List<PatrolSpawn>();
        rooms = new List<Room>();
        int roomCount = 0;
        SpawnFirstRoom();
        while (roomCount < maxRoomCount - 2)
        {
            if (FindAvailablePoint(out Transform point))
            {
                Room room = SpawnRoom(point);
                roomCount++;
                room.name = $"Room {roomCount}";
            }
            else
            {
                break;
            }
        }
        SpawnLastRoom();
    }

    private Room SpawnRoom(Transform point, bool isSpecial = false)
    {
        Room room = Instantiate(GetRandomRoom(isSpecial), point.position, point.rotation).GetComponent<Room>();
        rooms.Add(room);
        room.GenerateRoom(1, 1, RoomType.Standart, point);
        lastSpawnedRoom = room;
        return room;
    }

    public GameObject GetRandomRoom(bool isSpecialRoom)
    {
        if (isSpecialRoom)
        {
            List<RoomItem> specialRooms = roomPrefabs.Where(item => item.canSpawnAsSpecialRoom).ToList();

            int totalWeight = 0;
            foreach (var room in specialRooms)
            {
                totalWeight += room.weight;
            }
            int weight = Random.Range(0, totalWeight);
            int weightSum = 0;
            foreach (var room in specialRooms)
            {
                if (weightSum > weight)
                {
                    return room.prefab;
                }
                weightSum += room.weight;
            }
            return specialRooms[specialRooms.Count-1].prefab;
        }
        else
        {
            int totalWeight = 0;
            foreach (var room in roomPrefabs)
            {
                totalWeight += room.weight;
            }
            int weight = Random.Range(0, totalWeight);
            int weightSum = 0;
            foreach (var room in roomPrefabs)
            {
                if (weightSum > weight)
                {
                    return room.prefab;
                }
                weightSum += room.weight;
            }
            return roomPrefabs[roomPrefabs.Count-1].prefab;
        }
    }

    private void SpawnLastRoom()
    {
        if (FindAvailablePoint(out Transform point))
        {
            SpawnRoom(point, true);
        }

        lastSpawnedRoom.name = "EndRoom";
        lastSpawnedRoom.navMeshSurface.BuildNavMesh();
        GenerationComplete();
    }

    internal void GenerationComplete()
    {
        SpawnAllEntities();
        FadeManager.instance.StartFadeIn();
    }

    private void SpawnAllEntities()
    {
        SpawnPlayer(stairLocation);
        SpawnGirl(lastSpawnedRoom.mainPoint);

        for (int i = 0; i < patrolSpawnList.Count; i++)
        {
            var enemy = Instantiate(GetRandomPatrolEnemy(), patrolSpawnList[i].route.points[0].position, Quaternion.identity);
            enemy.GetComponent<PatrolEnemy>().SetupEnemy(patrolSpawnList[i].route);
        }
        patrolSpawnList.Clear();

        for (int i = 0; i < guardSpawnList.Count; i++)
        {
            var enemy = Instantiate(GetRandomGuardEnemy(), guardSpawnList[i].pos, Quaternion.identity);
            enemy.GetComponent<GuardEnemy>().SetupEnemy(guardSpawnList[i].pos, guardSpawnList[i].rot);
        }
        guardSpawnList.Clear();
    }

    private void SpawnFirstRoom()
    {
        Room room = Instantiate(GetRandomRoom(true), Vector3.zero, Quaternion.identity).GetComponent<Room>();
        lastSpawnedRoom = room;
        rooms.Add(room);
        room.GenerateRoom(1, 1, RoomType.Start);
        room.name = "StartRoom";
    }

    private bool FindAvailablePoint(out Transform point)
    {
        Room room = GetRandomRoom();
        if (room != null)
        {
            point = room.GetRandomEntry();
            return true;
        }
        else
        {
            point = null;
            return false;
        }
    }

    private Room GetRandomRoom()
    {
        return rooms[Random.Range(0, rooms.Count)];
    }

    internal GameObject GetRandomPatrolEnemy()
    {
        return patrolPrefabs[Random.Range(0, patrolPrefabs.Length)];
    }

    internal GameObject GetRandomGuardEnemy()
    {
        return guardPrefabs[Random.Range(0, guardPrefabs.Length)];
    }

    internal void SpawnGirl(Transform mainPoint)
    {
        Girl girl = Instantiate(girlPrefab, mainPoint.position, Quaternion.identity).GetComponent<Girl>();
        girl.Setup(stairLocation);
    }

    internal void SpawnPlayer(Vector3 pos)
    {
        GameObject gameObject = Instantiate(playerPrefab, pos, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().target = gameObject.transform;
        Camera.main.transform.position = gameObject.transform.position + new Vector3(5,10,-5);
    }

    internal void SpawnEntry(Transform mainPoint)
    {
        Instantiate(stairsPrefabs[Random.Range(0, stairsPrefabs.Length)], mainPoint.position, Quaternion.identity);
        stairLocation = mainPoint.position;
    }
}