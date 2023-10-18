using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData_", menuName = "ScriptableObjects/Weapons")]
public class WeaponDataSO : ScriptableObject
{

    public GameObject weaponPrefab;
    public Sprite weaponRenderer;
    [Range(0f, 1f)]
    public float attackCoolTime;
    [Range(1, 20)]
    public int weaponDamage;
}
