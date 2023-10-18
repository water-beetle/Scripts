using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    IdleState,
    AttckState,
    PursueState,
    AvoidState,
    RushState,
    WanderState
}

public enum MoveType
{
    Stop,
    Move,
    Rush
}

public enum PoolType
{
    PotionEffect,
    Enemy
}

public enum StatAddType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMul = 300,
}

public enum StatType
{
    Health,
    Attack,
    Defense,
    Speed,
    Dodge,
    Critical,
    AttackSpeed,
    HealthRegen,
}

public enum ItemType
{
    Potion,
    Equipment,
    Scroll
}

public enum EquipType
{
    Head,
    Body,
    Shoes,
    Accessory,
    Accessory2
}