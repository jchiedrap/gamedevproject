using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EncounterHandler : MonoBehaviour
{
    public int highEncounterRate, lowEncounterRate, stepsBeforeEncounter;
    
    public List<EncounterType> encounterTypes;

    private void Start()
    {
        CalculateEncounterRate();
    }

    void CalculateEncounterRate()
    { 
        stepsBeforeEncounter = Random.Range(highEncounterRate, lowEncounterRate);
    }

    public void CalculateEncounter()
    {
        stepsBeforeEncounter--;
        if (stepsBeforeEncounter <= 0) StartEncounter();
    }
    
    void StartEncounter()
    {
        int encounterIndex = Random.Range(0, encounterTypes.Count);
        EncounterType encounterType = encounterTypes[encounterIndex];
        
        // TODO: call event handler and tell it it's busy, then enable the UI, then start the encounter logic
        Debug.Log("Starting encounter: " + encounterType.name);
    }

    void EndEncounter()
    {
        // TODO: disable the UI and tell the event handler it's not busy when the encounter ends
        CalculateEncounterRate();
    }
}
