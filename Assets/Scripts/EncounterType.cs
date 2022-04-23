using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterType", menuName = "ScriptableObjects/Encounters", order = 1)]
public class EncounterType : SerializedScriptableObject
{
    public List<Entity> enemies = new List<Entity>();
}
