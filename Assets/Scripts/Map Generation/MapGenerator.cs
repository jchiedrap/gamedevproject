using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

public class MapGenerator : SerializedMonoBehaviour
{
    public Vector2Int startPosition;
    public RoomProfileConfig roomProfileConfig;
    [TableList]
    public HashSet<Cell> map = new();
    public List<GameObject> tiles = new();

    public int roomCount, mapCount;

    private void Start()
    {
        GenerateMap();
    }

    [Button("Generate Multiple Map")]
    public void BenchmarkNMaps()
    {
        List<HashSet<Cell>> maps = new();
        
        var current = Time.time;
        for (int i = 0; i < mapCount; i++)
        {
            maps.Add(GenerateMap());
        }

        Debug.Log(Time.time - current);
        Debug.Log(maps.Count);
        Debug.Log(maps.Last().Count);
    }

    public void PlaceMapObjects(HashSet<Cell> map)
    {
        bool alreadyPlacedUpStairs = false;
        
        // way too smart line that I wanted to keep, but basically:
        // for each cell in the map select the cell's prefab depending on the tile type, instantiate the prefab as tile, and add it to the tiles list
        foreach (var cell in map)
        {
            GameObject prefab = cell.type switch
            {
                TileType.Floor => roomProfileConfig.floorPrefab,
                TileType.Wall => roomProfileConfig.wallPrefab,
                TileType.Door => roomProfileConfig.doorPrefab,
                TileType.Stairs => roomProfileConfig.stairsPrefab,
                _ => null
            };

            var tile = Instantiate(prefab, new Vector3(cell.position.x, 0, cell.position.y), Quaternion.identity);
            tile.transform.parent = transform;
            tiles.Add(tile);

            if (cell.type != TileType.Stairs) continue;
            if (alreadyPlacedUpStairs) SetAsDownStairs(tile);
            else
            {
                SetAsUpStairs(tile);
                alreadyPlacedUpStairs = true;
            }
        }
    }

    [Button("Generate And Place Map")]
    public void GenerateAndPlace()
    {
        PlaceMapObjects(GenerateMap());
    }
    
    
    public HashSet<Cell> GenerateMap()
    {
        map = new HashSet<Cell>();
        
        #if UNITY_EDITOR
        foreach (var tile in tiles) DestroyImmediate(tile);
        #else
        foreach (var tile in tiles) Destroy(tile);
        #endif
        
        tiles.Clear();
        
        var floorPositions = GenerateRooms();

        var stairsPositions = PlaceStairs(floorPositions);
        
        foreach (var pos in stairsPositions) floorPositions.Remove(pos);

        var wallablePositions = floorPositions.Union(stairsPositions);
        
        foreach (var position in floorPositions)
        {
            map.Add(new Cell(position, TileType.Floor));
        }

        map.Add(new Cell(stairsPositions[0], TileType.Stairs));
        map.Add(new Cell(stairsPositions[1], TileType.Stairs));

        foreach (var wall in WallNeighbors(wallablePositions))
            map.Add(new Cell(wall, TileType.Wall));

        return map;
    }

    private void SetAsUpStairs(GameObject tile)
    {
        var stairInter = tile.AddComponent<Interactable>();
        
    }

    private void SetAsDownStairs(GameObject tile)
    {
        // add component of event, make it a floor change event going down
    }

    private List<Vector2Int> PlaceStairs(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> stairsPositions = new();
        var floorPosList = floorPositions.ToList();

        Shuffle(floorPosList);

        foreach (var pos in floorPosList)
        {
            int floorNeighbors = GetSquareNeighbors(pos).Count(neighbor => floorPositions.Contains(neighbor));
            if (floorNeighbors == 8)
            {
                switch (stairsPositions.Count)
                {
                    case 0:
                        stairsPositions.Add(pos);
                        break;
                    case 1 when Vector2Int.Distance(pos, stairsPositions.ElementAt(0)) <=
                                Math.Sqrt((roomProfileConfig.mapSizeX ^ 2) + (roomProfileConfig.mapSizeY ^ 2)) * 2:
                        continue;
                    case 1:
                        stairsPositions.Add(pos);
                        break;
                }
            }
            if (stairsPositions.Count == 2) break;
        }

        return stairsPositions;
    }
    
    public static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    private HashSet<Vector2Int> WallNeighbors(IEnumerable<Vector2Int> wallablePositions)
    {
        // return a hash set of all the positions adjacent to the floor positions that are not in the floor positions
        var wallNeighbors = new HashSet<Vector2Int>();
        var vector2Ints = wallablePositions as Vector2Int[] ?? wallablePositions.ToArray();
        foreach (var neighbor in vector2Ints.SelectMany(position => GetNeighbors(position).Where(neighbor => !vector2Ints.Contains(neighbor))))
            wallNeighbors.Add(neighbor);

        return wallNeighbors;
    }

    private static HashSet<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        var neighbors = new Vector2Int[] {
            new (pos.x + 1, pos.y),
            new (pos.x - 1, pos.y),
            new (pos.x, pos.y + 1),
            new (pos.x, pos.y - 1)
        };
        
        return Enumerable.ToHashSet(neighbors);
    }

    private static HashSet<Vector2Int> GetSquareNeighbors(Vector2Int pos)
    {
        var neighbors = new Vector2Int[] {
            new (pos.x + 1, pos.y),
            new (pos.x - 1, pos.y),
            new (pos.x, pos.y + 1),
            new (pos.x, pos.y - 1),
            new (pos.x + 1, pos.y + 1),
            new (pos.x - 1, pos.y - 1),
            new (pos.x - 1, pos.y + 1),
            new (pos.x + 1, pos.y - 1)
        };
        
        return Enumerable.ToHashSet(neighbors);
    }

    public HashSet<Vector2Int> GenerateRoom(RoomType type, BoundsInt room)
    {
        return type switch
        {
            RoomType.Rectangle => CreateRectangularRoom(room),
            RoomType.Random => MapGenerationAlgorithms.RandomWalk(new Vector2Int((int) room.center.x, (int)room.center.y),
                roomProfileConfig.roomSizeRandom + Random.Range(0, roomProfileConfig.roomSizeVariance), false, room),
            _ => null
        };
    }
    
    public HashSet<Vector2Int> GenerateRooms()
    {
        var roomsList = MapGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int) startPosition,
                new Vector3Int(roomProfileConfig.mapSizeX, roomProfileConfig.mapSizeY, 10)),
            roomProfileConfig.roomSizeX,
            roomProfileConfig.roomSizeY,
            roomProfileConfig.minimumRooms,
            roomProfileConfig.maximumRooms);
        
        return CreateRooms(roomsList);
    }

    private HashSet<Vector2Int> CreateRooms(IEnumerable<BoundsInt> roomsList)
    {
        var floorPositions = new HashSet<Vector2Int>();

        foreach (var roomPositions in roomsList.Select(room => GenerateRoom(roomProfileConfig.roomType, room)))
            floorPositions.UnionWith(roomPositions);

        var roomCenters = roomsList.Select(room => (Vector2Int) Vector3Int.RoundToInt(room.center)).ToList();
        
        floorPositions.UnionWith(CreateHallways(roomCenters));
        
        return floorPositions;
    }

    private HashSet<Vector2Int> CreateHallways(List<Vector2Int> roomCenters)
    {
        var corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            var closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            var newHallway = CreateHallway(currentRoomCenter, closest);
            corridors.UnionWith(newHallway);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateHallway(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        var hallway = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        hallway.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y) position += Vector2Int.up;
            else if (destination.y < position.y) position += Vector2Int.down;
            hallway.Add(position);
        }
        
        while (position.x != destination.x)
        {
            if (destination.x > position.x) position += Vector2Int.right;
            else if (destination.x < position.x) position += Vector2Int.left;
            hallway.Add(position);
        }

        hallway.Add(destination);

        return hallway;
    }

    private static Vector2Int FindClosestPointTo(Vector2Int point, List<Vector2Int> otherPoints)
    {
        var closest = Vector2Int.zero;
        var distance = float.MaxValue;

        foreach (var other in otherPoints)
        {
            var currentDistance = Vector2.Distance(other, point);
            if (!(currentDistance < distance)) continue;
            distance = currentDistance;
            closest = other;
        }

        return closest;
    }


    private HashSet<Vector2Int> CreateRectangularRoom(BoundsInt room)
    {
        var floor = new HashSet<Vector2Int>();

        int xStart = Random.Range(1,roomProfileConfig.roomSizeVariance);
        int yStart = Random.Range(1,roomProfileConfig.roomSizeVariance);

        for (int col = xStart; col < roomProfileConfig.roomSizeX - xStart; col++)
        for (int row = yStart; row < roomProfileConfig.roomSizeY - yStart; row++)
        {
            var pos = (Vector2Int) room.min + new Vector2Int(col, row);
            floor.Add(pos);
        }
        
        return floor;
    }
}

public enum TileType
{
    Floor,
    Wall,
    Door, 
    Stairs
}

public class Cell : IComparable
{
    public Vector2Int position;
    public TileType type;
    
    public Cell(Vector2Int position, TileType type)
    {
        this.position = position;
        this.type = type;
    }

    public Cell()
    {
        this.position = new Vector2Int();
        this.type = TileType.Wall;
    }

    public override string ToString()
    {
        return $"{position} {type}";
    }

    public int CompareTo(object obj)
    {
        var other = obj as Cell;
        if (other == null)
            throw new ArgumentException("Object is not a Cell");
        
        return (position.x == other.position.x) 
            ? 
            ((position.y == other.position.y) 
                ? 0 
                : position.y - other.position.y) 
            : position.x - other.position.x;
    }

    public override bool Equals(object obj)
    {
        return obj is Cell cell &&
               position == cell.position &&
               type == cell.type;
    }
    
    public override int GetHashCode()
    {
        var hashCode = -1789735981;
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector2Int>.Default.GetHashCode(position);
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        return hashCode;
    }
    
    public static bool operator ==(Cell left, Cell right)
    {
        return EqualityComparer<Cell>.Default.Equals(left, right);
    }
    
    public static bool operator !=(Cell left, Cell right)
    {
        return !(left == right);
    }
}