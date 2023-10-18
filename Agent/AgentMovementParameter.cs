using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementParameter
{
    public Vector2 direction;
    public MoveType moveType;

    public AgentMovementParameter(Vector2 direction, MoveType moveType)
    {
        this.direction = direction.normalized.normalized;
        this.moveType = moveType;
    }

    public AgentMovementParameter(MoveType moveType)
    {
        this.moveType = moveType;
    }
}
