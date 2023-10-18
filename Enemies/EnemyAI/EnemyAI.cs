using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Astar))]

[RequireComponent(typeof(IdleAction))]
[RequireComponent(typeof(PursueAction))]
public class EnemyAI : MonoBehaviour
{
    [Header("Componenents For EnemyState"), Space(10)]
    public Transform target;
    public Astar astarAlgorithm;
    public TilePainter painter;
    public DungeonData dungeonData;

    [Header("Enemy State Graph"), Space(10)]
    [SerializeField] private FSMGraphSO _graph;

    private StateNodeSO currentNode;

    [Header("Events"), Space(10)]
    [SerializeField]
    private GameEventSO enemyMoveEvent;

    private StateAction idleState, pursueState;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        astarAlgorithm = GetComponent<Astar>();
        painter = FindAnyObjectByType<TilePainter>();
        //dungeonData = FindAnyObjectByType<DungeonData>();
        GetStateComponents();
        _graph.enemyAI = this;
        currentNode = _graph.startStateNode;
    }

    private void Start()
    {
        dungeonData = DungeonData.Instance;   
    }

    private void GetStateComponents()
    {
        idleState = GetComponent<IdleAction>();
        pursueState = GetComponent<PursueAction>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayStateActeion();
        PlayStateDecision();
    }

    private void PlayStateActeion()
    {
        State stateType = currentNode.stateType;

        switch (stateType)
        {
            case State.IdleState:
                idleState.Action(this);
                break;
            case State.PursueState:
                pursueState.Action(this);
                break;
        }

    }

    private void PlayStateDecision()
    {
        StateNodeSO nextNode = currentNode.PlayStateDecision();
        if (nextNode)
            currentNode = nextNode;
    }

    public void Move(AgentMovementParameter movementParameter)
    {
        enemyMoveEvent.Raise(this, movementParameter);
    }
}
