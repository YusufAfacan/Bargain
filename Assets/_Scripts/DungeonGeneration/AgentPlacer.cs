using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class AgentPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject altarPrefab;
    [SerializeField]
    public Transform enemyParent;
    [SerializeField] 
    public Transform altarParent;
    [SerializeField]
    private Transform player;

    private bool placeAltar = false;
    //private GameObject playerPrefab;

    //[SerializeField]
    //private int playerRoomIndex;
    //[SerializeField]
    //private CinemachineVirtualCamera vCamera;

    //[SerializeField]
    //private List<int> roomEnemiesCount;

    [SerializeField]
    DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    public RoomHostilityStructure roomHostilityStructure;

    private void Awake()
    {
        dungeonData = GetComponent<DungeonData>();
        player = FindObjectOfType<Player>().transform;
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;

        Room roomWithPlayer = null;

        //Loop for each room
        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            //TO place eneies we need to analyze the room tiles to find those accesible from the path
            Room room = dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            if (room.FloorTiles.Contains(Vector2Int.zero))
            {
                roomWithPlayer = room;
            }

            //Find the Path inside this specific room
            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(room.FloorTiles);
            //Find the tiles belonging to both the path and the room
            roomFloor.IntersectWith(dungeonData.CorridorTiles);

            //Run the BFS to find all the tiles in the room accessible from the path
            Dictionary<Vector2Int, Vector2Int> roomMap = roomGraph.RunBFS(roomFloor.First(), room.PropPositions);

            //Positions that we can reach + path == positions where we can place enemies
            room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            

            for (int j = 0; j < roomHostilityStructure.enemiesInRoom.Count; j++)
            {
                //MobGroup mobGroup = roomHostilityStructure.mobsInRoom[j];

                GameObject enemyObj = Instantiate(enemyPrefab);
                enemyObj.transform.SetParent(enemyParent);
                enemyObj.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[j] + Vector2.one * 0.5f;
                room.EnemiesInTheRoom.Add(enemyObj);

                enemyObj.GetComponent<Enemy>().data = roomHostilityStructure.enemiesInRoom[j];

            }

            if (placeAltar)
            {
                GameObject altarObj = Instantiate(altarPrefab);
                altarObj.transform.SetParent(altarParent);
                altarObj.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[room.EnemiesInTheRoom.Count] + Vector2.one * 0.5f;
            }
            

        }

        foreach (GameObject enemy in roomWithPlayer.EnemiesInTheRoom)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    

    /// <summary>
    /// Places enemies in the positions accessible from the path
    /// </summary>
    /// <param name="room"></param>
    /// <param name="enemysCount"></param>
    private void PlaceEnemies(Room room, int enemysCount)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in dungeonData.Rooms)
        {
            Color color = Color.green;
            color.a = 0.3f;
            Gizmos.color = color;

            foreach (Vector2Int pos in room.PositionsAccessibleFromPath)
            {
                Gizmos.DrawCube((Vector2)pos + Vector2.one * 0.5f, Vector2.one);
            }
        }
    }
}

public class RoomGraph
{
    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public RoomGraph(HashSet<Vector2Int> roomFloor)
    {
        foreach (Vector2Int pos in roomFloor)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();
            foreach (Vector2Int direction in fourDirections)
            {
                Vector2Int newPos = pos + direction;
                if (roomFloor.Contains(newPos))
                {
                    neighbours.Add(newPos);
                }
            }
            graph.Add(pos, neighbours);
        }
    }

    /// <summary>
    /// Creates a map of reachable tiles in our dungeon.
    /// </summary>
    /// <param name="startPos">Door position or tile position on the path between rooms inside this room</param>
    /// <param name="occupiedNodes"></param>
    /// <returns></returns>
    public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
    {
        //BFS related variuables
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

        //The dictionary that we will return 
        Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();
        map.Add(startPos, startPos);

        while (nodesToVisit.Count > 0)
        {
            //get the data about specific position
            Vector2Int node = nodesToVisit.Dequeue();
            List<Vector2Int> neighbours = graph[node];

            //loop through each neighbour position
            foreach (Vector2Int neighbourPosition in neighbours)
            {
                //add the neighbour position to our map if it is valid
                if (visitedNodes.Contains(neighbourPosition) == false &&
                    occupiedNodes.Contains(neighbourPosition) == false)
                {
                    nodesToVisit.Enqueue(neighbourPosition);
                    visitedNodes.Add(neighbourPosition);
                    map[neighbourPosition] = node;
                }
            }
        }

        return map;
    }
}
