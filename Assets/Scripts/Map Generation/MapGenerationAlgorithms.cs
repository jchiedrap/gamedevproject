using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerationAlgorithms
{
    public static Vector2Int GetRandomCardinal()
    {
        int randomIndex = Random.Range(0, 4);
        switch (randomIndex)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.down;
            case 2:
                return Vector2Int.left;
            case 3:
                return Vector2Int.right;
            default:
                return Vector2Int.zero;
        }
    }
    
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength, bool countPreviousSteps, BoundsInt roomBounds)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        
        var prevPosition = startPosition;
        while (path.Count < walkLength)
        {
            var nextPosition = prevPosition + GetRandomCardinal();
            var nextPosition_v3 = new Vector3Int(nextPosition.x, nextPosition.y);
            if (!roomBounds.Contains(nextPosition_v3)) continue;
            if (!(countPreviousSteps && path.Contains(nextPosition))) path.Add(nextPosition);
            prevPosition = nextPosition;
        }
        
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        var corridor = new List<Vector2Int>();
        var direction = GetRandomCardinal();
        var currentPosition = startPosition;
        
        corridor.Add(currentPosition);
        
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
            
        }

        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minX, int minY, int minimumRooms, int maximumRooms)
    {
        var rooms = new List<BoundsInt>();
        while (rooms.Count < minimumRooms || rooms.Count > maximumRooms)
        {
            var roomsQueue = new Queue<BoundsInt>();
            roomsQueue.Enqueue(spaceToSplit);
            rooms.Clear();
            while (roomsQueue.Count > 0)
            {
                var room = roomsQueue.Dequeue();
                if (room.size.y < minY || room.size.x < minX) continue;
                if (Random.value > 0.5)
                {
                    if (room.size.y >= minY * 2) SplitHorizontally(minY, minX, roomsQueue, room);
                    else if (room.size.x >= minX * 2) SplitVerically(minY, minX, roomsQueue, room);
                    else rooms.Add(room);
                }
                else
                {
                    if (room.size.x >= minX * 2) SplitVerically(minY, minX, roomsQueue, room);
                    else if (room.size.y >= minY * 2) SplitHorizontally(minY, minX, roomsQueue, room);
                    else rooms.Add(room);
                }
            }
        }

        return rooms;
    }

    private static void SplitVerically(int minY, int minX, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        var leftRoom = new BoundsInt(room.min, new Vector3Int(xSplit, minY, room.size.z));
        var rightRoom = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        
        roomsQueue.Enqueue(leftRoom);
        roomsQueue.Enqueue(rightRoom);
    }

    private static void SplitHorizontally(int minY, int minX, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        var topRoom = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        var bottomRoom = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        
        roomsQueue.Enqueue(topRoom);
        roomsQueue.Enqueue(bottomRoom);
    }
}