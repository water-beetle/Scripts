using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyAI _enemyAI;

    protected virtual void Awake()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();

    }

    public void Handle()
    {
        Action();
        Decision();
    }

    protected abstract void Action();
    protected abstract void Decision();

}
