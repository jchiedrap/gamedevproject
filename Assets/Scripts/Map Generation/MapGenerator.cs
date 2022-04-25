using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : SerializedMonoBehaviour
{
    public Vector2Int startPosition;
    public RoomProfileConfig roomProfileConfig;
    [TableList]
    public HashSet<Cell> map = new();
    public List<GameObject> tiles = new();

    public int roomCount;

    private void Start()
    {
        GenerateMap();
    }
    
    [Button("Generate Map")]
    public void GenerateMap()
    {
        map = new HashSet<Cell>();
        
        #if UNITY_EDITOR
        foreach (var tile in tiles) DestroyImmediate(tile);
        #else
        foreach (var tile in tiles) Destroy(tile);
        #endif
        
        tiles.Clear();
        
        var floorPositions = GenerateRooms();

        foreach (var position in floorPositions)
        {
            map.Add(new Cell(position, TileType.Floor));
        }

        foreach (var wall in WallNeighbors(floorPositions))
        {
            map.Add(new Cell(wall, TileType.Wall));
        }

        // way too smart line that I wanted to keep, but basically:
        // for each cell in the map select the cell's prefab depending on the tile type, instantiate the prefab as tile, and add it to the tiles list
        foreach (var tile in from cell in map let prefab = cell.type switch
                 {
                     TileType.Floor => roomProfileConfig.floorPrefab,
                     TileType.Wall => roomProfileConfig.wallPrefab,
                     TileType.Door => roomProfileConfig.doorPrefab,
                     TileType.Stairs => roomProfileConfig.stairsPrefab,
                     _ => null
                 } select Instantiate(prefab, new Vector3(cell.position.x, 0, cell.position.y), Quaternion.identity))
        {
            tile.transform.parent = transform;
            tiles.Add(tile);
        }
    }

    private static HashSet<Vector2Int> WallNeighbors(ICollection<Vector2Int> floorPositions)
    {
        // return a hash set of all the positions adjacent to the floor positions that are not in the floor positions
        var wallNeighbors = new HashSet<Vector2Int>();
        foreach (var neighbor in floorPositions.SelectMany(position => GetNeighbors(position).Where(neighbor => !floorPositions.Contains(neighbor))))
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
        
        return neighbors.ToHashSet();
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
        {
            floorPositions.UnionWith(roomPositions);
        }
        
        return floorPositions;

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