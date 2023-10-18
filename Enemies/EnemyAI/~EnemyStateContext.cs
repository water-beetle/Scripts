using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyIdleState))]
[RequireComponent(typeof(EnemyPursueState))]
public class EnemyStateContext : MonoBehaviour
{
    public EnemyState CurrentState { get; set; }


    private EnemyState _IdleState, _AttckState, _PursueState, _AvoidState, _RushState, _WanderState;

    private void AttachStatetoObject()
    {
        _IdleState = gameObject.GetComponent<EnemyIdleState>();
        _PursueState = gameObject.GetComponent<EnemyPursueState>();
        _RushState = gameObject.GetComponent<EnemyRushState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AttachStatetoObject();
        CurrentState = _IdleState;
    }

    public void Transition(State state)
    {
        switch (state)
        {
            case State.IdleState:
                CurrentState = _IdleState;
                break;
            case State.AttckState:
                CurrentState = _AttckState;
                break;
            case State.PursueState:
                CurrentState = _PursueState;
                break;
            case State.AvoidState:
                CurrentState = _AvoidState;
                break;
            case State.RushState:
                CurrentState = _RushState;
                break;
            case State.WanderState:
                CurrentState = _WanderState;
                break;
        }
    }

    public void RunState()
    {
        CurrentState.Handle();
    }
}
