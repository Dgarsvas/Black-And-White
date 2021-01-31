using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum RoomType
{
    Standart,
    Start
}

public enum Dir
{
    Up,
    Right,
    Down,
    Left
}

[Serializable]
public class PatrolRoute
{
    public Transform[] points;
}

public class Room : MonoBehaviour
{
    [SerializeField]
    private PatrolRoute[] routes;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    public Transform mainPoint;

    [SerializeField]
    public NavMeshSurface navMeshSurface;

    public List<Transform> entryPoints;

    public static Vector2 roomSize = new Vector2(14.5f, 10f);

    public void GenerateRoom(int staticEnemyAmount, int patrolEnemyAmount, RoomType type, Transform pos = null)
    {
        switch (type)
        {
            case RoomType.Standart:
                RotateAndOffsetRoomToMatchPreviousRoomEntry(GetRandomEntry(), pos);
                SpawnEnemies(staticEnemyAmount, patrolEnemyAmount);
                break;
            case RoomType.Start:
                GenerationController.instance.SpawnEntry(mainPoint);
                break;
        }
    }

    private void RotateAndOffsetRoomToMatchPreviousRoomEntry(Transform thisRoom, Transform otherRoom)
    {  
        Rotate(thisRoom, otherRoom);
        Offset(thisRoom, otherRoom);
    }

    private void Offset(Transform thisRoom, Transform otherRoom)
    {
       

        Vector3 diffA = otherRoom.position - thisRoom.position;
        Vector3 diffB = thisRoom.position - otherRoom.position;

        Debug.Log($"prevEntry:{otherRoom.position} curEntry:{thisRoom.position} moveBy:{(diffB)} roomPos{transform.position}");

        transform.position = otherRoom.position - diffB;

        /*
        if (thisRoom.position.magnitude < otherRoom.position.magnitude)
        {
            transform.position = transform.position - (thisRoom.position - otherRoom.position);
        }
        else
        {
            transform.position = thisRoom.position + moveBy;
        }
        */
    }

    private void Rotate(Transform thisRoom, Transform otherRoom)
    {
        Dir thisDir = GetDirection(thisRoom);
        Dir otherDir = GetDirection(otherRoom);
        int angle = (((int)otherDir) - (int)thisDir);

        switch (angle)
        {
            case -3:
                angle = 90;
                break;
            case -2:
                angle = 0;
                break;
            case -1:
                angle = -90;
                break;
            case 0:
                angle = 180;
                break;
            case 1:
                angle = 90;
                break;
            case 2:
                angle = 0;
                break;
            case 3:
                angle = -90;
                break;
        }

        Debug.Log($"previousRoom:{otherDir} currentRoom:{thisDir} currentRot:{transform.rotation.eulerAngles.y}  upcomingRot:{transform.rotation.eulerAngles.y + angle}");

        transform.rotation = Quaternion.Euler(0, angle + transform.rotation.eulerAngles.y, 0);
    }

    private Dir GetDirection(Transform entryPoint)
    {
        float x = entryPoint.localPosition.x / roomSize.x;
        float y = entryPoint.localPosition.y / roomSize.y;

        Dir dir = Dir.Up;
        Vector2 norm = new Vector2(x, y);

        if (norm.x == 1)
        {
            dir = Dir.Right;
        }
        else if (norm.x == -1)
        {
            dir = Dir.Left;
        }
        else if (norm.y == 1)
        {
            dir = Dir.Up;
        }
        else
        {
            dir = Dir.Down;
        }

        return dir;
    }

    internal Transform GetRandomEntry(bool remove = true)
    {
        if (entryPoints.Count > 0)
        {
            Transform entry = entryPoints[Random.Range(0, entryPoints.Count)];
            if (remove)
            {
                entryPoints.Remove(entry);
            }
            if (entryPoints.Count == 0)
            {
                GenerationController.instance.rooms.Remove(this);
            }

            return entry;
        }
        else
        {
            GenerationController.instance.rooms.Remove(this);
            return null;
        }
    }

    private void SpawnEnemies(int staticEnemyAmount, int patrolEnemyAmount)
    {
        for (int i = 0; i < staticEnemyAmount; i++)
        {
            AddGuardEnemyToSpawnList();
        }

        for (int i = 0; i < patrolEnemyAmount; i++)
        {
            AddPatrolEnemyToSpawnList();
        }
    }

    private void AddGuardEnemyToSpawnList()
    {
        if (spawnPoints.Length == 0)
        {
            return;
        }
        Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GenerationController.instance.guardSpawnList.Add(new GuardSpawn(pos, Quaternion.FromToRotation(pos, mainPoint.position).eulerAngles.y));
    }

    private void AddPatrolEnemyToSpawnList()
    {
        if (routes.Length == 0)
        {
            return;
        }
        PatrolRoute route = routes[Random.Range(0, routes.Length)];
        GenerationController.instance.patrolSpawnList.Add(new PatrolSpawn(route));
    }
}
