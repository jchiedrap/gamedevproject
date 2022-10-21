using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Character Creation/Entity", order = 1)]
public class Entity : SerializedScriptableObject
{
    public Sprite portrait;
    public StatArray baseStats;
    public StatArray currentStats;

    public int maxHP;
    public int currentHP;
    
    public List<Skill> skills;

    public int GetStatByName(string n)
    {
        int stat = -1;
        foreach (var s in currentStats.stats)
            if (s.name.ToString() == n)
                stat = s.value;
        return stat;
    }
}

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Character Creation/Skill", order = 2)]
public class Skill : SerializedScriptableObject
{
    public enum SkillType
    {
        Damage,
        Heal,
        Buff,
        Status,
        Special
    }

    public enum DamageType
    {
        Physical,
        Magical,
        True
    }
    
    public enum TargetType
    {
        Self,
        SingleParty,
        SingleAll,
        AllParty,
        AllEnemies, 
        All
    }
    
    public new string name;
    public string description;
    public SkillType type;
    public Sprite icon;
    public int cost;
    public TargetType target;

    [Space]
    [ShowIf("type", SkillType.Damage)]
    public DamageElementType elementType;
    [ShowIf("type", SkillType.Damage)]
    public DamageType damageType;
    [ShowIf("type", SkillType.Damage)]
    public int damageScaling;
    [ShowIf("type", SkillType.Damage)]
    public Stat.StatName stat;
    [ShowIf("type", SkillType.Damage)]
    public float damageAccuracy;
    [ShowIf("type", SkillType.Damage)]
    public Status damageStatus;
    [ShowIf("DamageHasStatus")]
    public float damageStatusAccuracy;

    [Space]
    
    [ShowIf("type", SkillType.Heal)]
    public int healScaling;
    [ShowIf("type", SkillType.Heal)]
    public Stat.StatName healStat;
    
    [Space]
    
    [ShowIf("type", SkillType.Buff)]
    public List<Buff> buffStats = new List<Buff>();
    [ShowIf("type", SkillType.Buff)]
    public float buffAccuracy;
    [ShowIf("type", SkillType.Buff)]

    [Space]
    
    [ShowIf("type", SkillType.Status)]
    public Status status;
    [ShowIf("type", SkillType.Status)]
    public int statusDuration;
    [ShowIf("type", SkillType.Status)]
    public float statusAccuracy;
    
    // _____________
    
    public bool DamageHasStatus()
    {
        return type == SkillType.Damage && damageStatus != Status.None;
    }
}

public class StatArray : SerializedScriptableObject
{
    public Stat[] stats;
}

public class Stat : SerializedScriptableObject
{
    public enum StatName
    {
        Strength,
        Dexterity,
        Intelligence,
        Vitality,
        Wisdom,
        Charisma
    }
    public new StatName name;
    public int value;
}

public enum Status
{
    Poisoned,       // damage scales per turn
    Burned,         // damage procs per time you're hit by physical
    Frostbite,      // damage procs per time you're hit by magical
    Bleeding,       // damage procs per turn they act
    Stunned,        // can't act, comes off after you get hit at all, extra damage of 1.5x
    Confused,       // 66% chance to do the action you chose, 34% random action at random target
    Blinded,        // all accuracy checks are lowered by 50%
    Slowed,         // speed is quartered
    Paralyzed,      // 50% chance to do the action you chose, 50% to do nothing
    Silenced,       // can't use skills
    Invulnerable,   // can't be hit
    Immune,         // can't be affected by anything
    None
}

public enum DamageElementType
{
    Bludgeoning,
    Piercing,
    Slashing,
    Fire,
    Cold,
    Electric,
    Poison,
    Force,
    Psychic,
    Radiant,
    Necrotic,
    None
}

public class Buff
{
    public Stat.StatName stat;
    public int turnDuration;
    public float amount;
    
    public Buff(Stat.StatName stat, int turnDuration, float amount)
    {
        this.stat = stat;
        this.turnDuration = turnDuration;
        this.amount = amount;
    }
    
    public Buff()
    {
        stat = Stat.StatName.Strength;
        turnDuration = 0;
        amount = 0;
    }
    
    public override string ToString()
    {
        return $"{stat} {amount} for {turnDuration} turns";
    }
    
    public static Buff operator +(Buff a, Buff b)
    {
        return new Buff(a.stat, Math.Abs(a.turnDuration + b.turnDuration), a.amount + b.amount);
    }
    
    public static Buff operator -(Buff a, Buff b)
    {
        return new Buff(a.stat, Math.Abs(a.turnDuration - b.turnDuration), a.amount - b.amount);
    }
}