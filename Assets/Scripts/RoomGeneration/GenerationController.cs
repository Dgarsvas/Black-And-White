using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GenerationController : MonoBehaviour
{
    public static GenerationController instance;
    public GameObject[] roomPrefabs;
    public GameObject[] patrolPrefabs;
    public GameObject[] staticPrefabs;
    [HideInInspector]
    public List<Room> rooms;
    public int maxRoomCount;

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

    private Room lastSpawnedRoom;

    private void GenerateRooms()
    {
        rooms = new List<Room>();
        int roomCount = 1;
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

        lastSpawnedRoom.SetAsLastRoom();
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

    internal GameObject GetRandomStaticEnemy()
    {
        return patrolPrefabs[Random.Range(0, staticPrefabs.Length)];
    }

    internal void SpawnGirl(Transform mainPoint)
    {
        //TODO
    }

    internal void SpawnEntry(Transform mainPoint)
    {
        //TODO
    }
}