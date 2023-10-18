using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDecisionSO : ScriptableObject
{
    public StateNodeSO currentState;
    public StateNodeSO nextState;

    public bool distanceCheck = true;
    public bool isDistanceLarger = false;
    public int distance = 3;

    public void SetState(StateNodeSO from, StateNodeSO to)
    {
        this.currentState = from;
        this.nextState = to;
        this.name = "Decision";

    }

    public bool check()
    {
        if (distanceCheck)
        {
            Vector2 agentPos = currentState.fsmGraph.enemyAI.transform.position;
            Vector2 targetPos = currentState.fsmGraph.enemyAI.target.transform.position;

            if(Vector2.Distance(agentPos, targetPos) < distance)
            {
                if (isDistanceLarger)
                    return false;
                return true;
            }

            if (isDistanceLarger)
                return true;
            return false;
        }

        return true;
    }
}
