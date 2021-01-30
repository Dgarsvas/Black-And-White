using System;
using System.Collections;
using System.Collections.Generic;
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
    Left,
    Up,
    Right,
    Down
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
    private Transform[] obstaclePositions;
    [SerializeField]
    public Transform mainPoint;

    [SerializeField]
    public NavMeshSurface navMeshSurface;

    public List<Transform> entryPoints;

    public RoomType type;

    public static Vector2 roomSize = new Vector2(14.5f, 10f);

    public void GenerateRoom(int staticEnemyAmount, int patrolEnemyAmount, RoomType type, Transform pos = null)
    {
        this.type = type;

        switch (type)
        {
            case RoomType.Standart:
                RotateAndOffsetRoomToMatchPreviousRoomEntry(GetRandomEntry(), pos);
                break;
            case RoomType.Start:
                GenerationController.instance.SpawnEntry(mainPoint);
                break;
        }

        //SpawnObstacles();
        SpawnEnemies(staticEnemyAmount, patrolEnemyAmount);
    }

    private void RotateAndOffsetRoomToMatchPreviousRoomEntry(Transform thisRoom, Transform otherRoom)
    {
        Rotate(thisRoom, otherRoom);
        Offset(thisRoom, otherRoom);
    }

    private void Offset(Transform thisRoom, Transform otherRoom)
    {
        transform.position = transform.position - (thisRoom.position - otherRoom.position);
    }

    private void Rotate(Transform thisRoom, Transform otherRoom)
    {
        Dir thisDir = GetDirection(thisRoom);
        Dir otherDir = GetDirection(otherRoom);
        float angle = (((int)thisDir - (int)otherDir + 3) * 90) - 90;
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
        Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GenerationController.instance.guardSpawnList.Add(new GuardSpawn(pos, Quaternion.FromToRotation(pos, mainPoint.position).eulerAngles.y));

    }

    private void AddPatrolEnemyToSpawnList()
    {
        PatrolRoute route = routes[Random.Range(0, routes.Length)];
        GenerationController.instance.patrolSpawnList.Add(new PatrolSpawn(route));
    }

    private void SpawnObstacles()
    {
        for (int i = 0; i < obstaclePositions.Length; i++)
        {
            //Instantiate(GenerationController.instance.GetRandomObstacle(), )
        }
    }
}
