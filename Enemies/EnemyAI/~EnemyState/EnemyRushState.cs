using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRushState : EnemyState
{

    protected override void Action()
    {
        Vector2 direction = (_enemyAI.target.transform.position - _enemyAI.transform.position).normalized;
        AgentMovementParameter moveParameter = new AgentMovementParameter(direction, MoveType.Rush);
        _enemyAI.Move(moveParameter);
    }

    protected override void Decision()
    {
        //_enemyAI.Transition(State.IdleState);
    }
}
