using System;
using System.Collections.Generic;
using UnityEngine;
public class DungeonManager : MonoBehaviour
{
    // set up singleton
    public static DungeonManager Instance { get { return _instance; } }
    private static DungeonManager _instance;
    
    // set up variables
    public int currentFloor = 1;
    public LevelManager levelManager;
    public List<HashSet<Cell>> maps = new List<HashSet<Cell>>();
    public GameObject mapPrefab;
    
    // set up prefabs

    private void Awake()
    { 
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    public void LoadFloor()
    {
        // selects current floor as map to be used from the maps list, and sets it up for user
        var currentMapInstance = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        levelManager = currentMapInstance.GetComponent<LevelManager>();
        levelManager.LoadMap(maps[currentFloor - 1]);
    }
    
    
}