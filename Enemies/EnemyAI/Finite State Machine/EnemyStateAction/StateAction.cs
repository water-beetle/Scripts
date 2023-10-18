using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateAction : MonoBehaviour
{
    public abstract void Action(EnemyAI enemyAI);
}
