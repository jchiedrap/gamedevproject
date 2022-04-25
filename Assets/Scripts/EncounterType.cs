using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterType", menuName = "ScriptableObjects/Dungeon/Encounter Type", order = 1)]
public class EncounterType : SerializedScriptableObject
{
    public List<Entity> enemies = new List<Entity>();
}
