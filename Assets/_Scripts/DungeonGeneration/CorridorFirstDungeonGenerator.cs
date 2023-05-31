using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = 0.8f;

    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> corridorPositions;

    [SerializeField]
    private DungeonData dungeonData;
    [SerializeField]
    private RoomDataExtractor roomDataExtractor;
    [SerializeField]
    private PropPlacementManager propPlacementManager;
    [SerializeField]
    private AgentPlacer agentPlacer;

    [SerializeField]
    private Transform propParent;
    [SerializeField]
    private Transform enemyParent;

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        //dungeonData = GetComponent<DungeonData>();
        //roomDataExtractor = GetComponent<RoomDataExtractor>();
        //propPlacementManager = GetComponent<PropPlacementManager>();
        //agentPlacer = GetComponent<AgentPlacer>();
    }

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
#if UNITY_EDITOR

        DestroyImmediate(propParent.gameObject);
        
#else
        Destroy(propParent.gameObject);
#endif


        GameObject NewpropParent = new GameObject();
        NewpropParent.name = "PropParent";

        propParent = NewpropParent.transform;
        propPlacementManager.propParent = propParent;

#if UNITY_EDITOR
        DestroyImmediate(enemyParent.gameObject);
#else
        Destroy(enemyParent.gameObject);
#endif



        GameObject NewEnemyParent = new GameObject();
        NewEnemyParent.name = "EnemyParent";

        enemyParent = NewEnemyParent.transform;
        agentPlacer.enemyParent = enemyParent;

        corridorPositions = null;
        
        dungeonData.Rooms.Clear();
        roomsDictionary.Clear();

        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

        AnalyzeDungeon();
        PlaceProps();

        navMeshSurface.BuildNavMeshAsync();

        SpawnEnemies();

    }

    private void SpawnEnemies()
    {
        agentPlacer.PlaceAgents();
    }

    private void PlaceProps()
    {
        propPlacementManager.ProcessRooms();
    }

    private void AnalyzeDungeon()
    {
        roomDataExtractor.ProcessRooms();
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;
                
            }
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            SaveRoomData(roomPosition, roomFloor);

            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPosition] = roomFloor;

        //addition
        Room room = new Room(roomPosition, roomFloor);
        dungeonData.Rooms.Add(room);
        

    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }

        corridorPositions = new HashSet<Vector2Int>(floorPositions);

        dungeonData.CorridorTiles = corridorPositions;
    }
}
