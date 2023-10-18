using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy", fileName = "EnemyData_")]
public class EnemyDataSO : ScriptableObject
{
    public int maxHealth;
    public int attackDamage;
}
