using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength, corridorCount;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent;

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
    private Transform altarParent; 



    private void Awake()
    {
        
    }

    private void Start()
    {
        FirstTimeGenerate();

    }

    private void FirstTimeGenerate()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();

        if (dungeonData.Rooms.Count != 15)
        {
            FirstTimeGenerate();
        }
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

#if UNITY_EDITOR

        DestroyImmediate(altarParent.gameObject);

#else
        Destroy(altarParent.gameObject);
#endif


        GameObject NewAltarParent = new GameObject();
        NewAltarParent.name = "AltarParent";

        altarParent = NewAltarParent.transform;
        agentPlacer.altarParent = altarParent;

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

        SpawnEnemies();

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

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private void AnalyzeDungeon()
    {
        roomDataExtractor.ProcessRooms();
    }

    private void PlaceProps()
    {
        propPlacementManager.ProcessRooms();
    }
    private void SpawnEnemies()
    {
        agentPlacer.PlaceAgents();
    }
    
    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPosition] = roomFloor;

        //addition
        Room room = new Room(roomPosition, roomFloor);
        dungeonData.Rooms.Add(room);

    }

}
