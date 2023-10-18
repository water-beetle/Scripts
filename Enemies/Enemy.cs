using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{

    [SerializeField]
    private GameEventSO OnDeath;
    [SerializeField]
    private GameEventSO OnTakeDamage;

    [SerializeField]
    private EnemyDataSO enemyData;

    private int maxHealth;
    public int health { get; private set;}
    private bool isDead;

    private void Start()
    {
        maxHealth = enemyData.maxHealth;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(GameObject dealer, int damage)
    {
        if (!isDead)
        {
            health -= 1;
            OnTakeDamage.Raise(dealer.transform, damage);

            if(health <= 0)
            {
                OnDeath.Raise(this, null);
            }
        }
    }
}
