using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EncounterType", menuName = "ScriptableObjects/Dungeon/Room Profile", order = 2)]
public class RoomProfileConfig : SerializedScriptableObject
{
    [SerializeField] public bool usesDoors;
    [SerializeField] public int roomSizeX, roomSizeY, roomSizeRandom;
    [FormerlySerializedAs("variance")] [SerializeField] public int roomSizeVariance;
    [SerializeField] public RoomType roomType;

    [SerializeField] public bool usesEnemies;
    [SerializeField] public int lowEncounterRate, highEncounterRate;
    
    [SerializeField] public GameObject floorPrefab, wallPrefab, doorPrefab, stairsPrefab; 
    [SerializeField] public List<EncounterType> encounterTypes;
    [SerializeField] public int minimumRooms, maximumRooms;
    
    [SerializeField] public int mapSizeX, mapSizeY;
}

public enum RoomType
{
    Rectangle,
    Random
}