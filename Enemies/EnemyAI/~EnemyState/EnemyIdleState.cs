using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected override void Action()
    {
        StopEnemy();
    }

    private void StopEnemy()
    {
        AgentMovementParameter moveParameter = new AgentMovementParameter(MoveType.Stop);
        _enemyAI.Move(moveParameter);
    }

    protected override void Decision()
    {
        float distance = Vector3.Distance(_enemyAI.target.position, transform.position);
        
        if(distance < Settings.Astar.PursueDistance)
        {
            //_enemyAI.Transition(State.PursueState);
        }
    }
}
