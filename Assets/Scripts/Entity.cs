using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : SerializedScriptableObject
{
    public Sprite portrait;
    public StatArray baseStats;
    public StatArray currentStats;
    
}

public class StatArray : SerializedScriptableObject
{
    public Stat[] stats;

}

public class Stat : SerializedScriptableObject
{
    public new string name;
    public int value;
}