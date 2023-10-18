using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : StateAction
{
    public override void Action(EnemyAI enemyAI)
    {
        AgentMovementParameter moveParameter = new AgentMovementParameter(MoveType.Stop);
        enemyAI.Move(moveParameter);
    }
}
