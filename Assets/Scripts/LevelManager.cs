using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance {get { return _instance; } }
    
    [FormerlySerializedAs("controller")] public PlayerInputController inputController;
    public EncounterHandler encounterHandler;
    public PlayerPartyController partyController;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        
        else
            _instance = this;
    }

    public void LoadMap(HashSet<Cell> map)
    {
        throw new NotImplementedException();
    }
}
