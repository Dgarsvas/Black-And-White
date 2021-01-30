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

public class GenerationController : MonoBehaviour
{
    public static GenerationController instance;
    public GameObject[] roomPrefabs;
    public GameObject[] patrolPrefabs;
    public GameObject[] guardPrefabs;

    public GameObject girlPrefab;
    public GameObject[] stairsPrefabs;

    [HideInInspector]
    public List<Room> rooms;
    public int maxRoomCount;

    public List<GuardSpawn> guardSpawnList;
    public List<PatrolSpawn> patrolSpawnList;

    private Room lastSpawnedRoom;
    private Vector3 stairLocation;

    public delegate void Notify();
    public event Notify GenerationCompleted;

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
        int roomCount = 2;
        SpawnFirstRoom();
        while (roomCount < maxRoomCount)
        {
            if (FindAvailablePoint(out Transform point))
            {
                Room room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], point.position, point.rotation).GetComponent<Room>();
                lastSpawnedRoom = room;
                rooms.Add(room);
                room.GenerateRoom(1, 1, RoomType.Standart, point);
                roomCount++;
            }
            else
            {
                break;
            }
        }

        SpawnLastRoom();
    }

    private void SpawnLastRoom()
    {
        if (FindAvailablePoint(out Transform point))
        {
            Room room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], point.position, point.rotation).GetComponent<Room>();
            lastSpawnedRoom = room;
            rooms.Add(room);
            room.GenerateRoom(1, 1, RoomType.Standart, point);
        }

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
        Room room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], Vector3.zero, Quaternion.identity).GetComponent<Room>();
        lastSpawnedRoom = room;
        rooms.Add(room);
        room.GenerateRoom(1, 1, RoomType.Start);
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

    internal GameObject GetRandomObstacle()
    {
        //TODO
        return null;
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
        Girl girl= Instantiate(girlPrefab, mainPoint.position, Quaternion.identity).GetComponent<Girl>();
        girl.Setup(stairLocation);
    }

    internal void SpawnEntry(Transform mainPoint)
    {
        Instantiate(stairsPrefabs[Random.Range(0, stairsPrefabs.Length)], mainPoint.position, Quaternion.identity);
        stairLocation = mainPoint.position;
    }
}