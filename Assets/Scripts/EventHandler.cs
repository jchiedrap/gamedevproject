using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventHandler : MonoBehaviour
{
    // set up singleton
    private static EventHandler _instance;
    public static EventHandler Instance { get { return _instance; } }
    
    [FormerlySerializedAs("player")] public PlayerInputController playerInput;

    public void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        
        else
            _instance = this;
    }
    
    public void OnStartEvent()
    {
        playerInput.eventHappening = true;
    }
    
    public void OnEndEvent()
    {
        playerInput.eventHappening = false;
    }
}
