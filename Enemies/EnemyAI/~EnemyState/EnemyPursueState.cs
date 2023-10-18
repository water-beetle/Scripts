using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Astar))]
public class EnemyPursueState : EnemyState
{
    private Astar astarAlgorithm;
    private TilePainter painter;
    private DungeonData dungeonData;

    private Stack<Vector3Int> paths = null;
    private bool isRebuilding = false;

    protected override void Awake()
    {
        base.Awake();
        astarAlgorithm = GetComponent<Astar>();
        painter = FindAnyObjectByType<TilePainter>();
        dungeonData = FindAnyObjectByType<DungeonData>();
    }

    protected override void Action()
    {
        if (!isRebuilding)
        {
            paths = BuildPath();
        }

        PursueWithPath(paths);
    }

    protected override void Decision()
    {
        float distance = Vector3.Distance(_enemyAI.target.position, transform.position);

        if (distance > Settings.Astar.PursueDistance)
        {
            //_enemyAI.Transition(State.IdleState);
        }

        if (distance < 3f)
        {
            //_enemyAI.Transition(State.RushState);
        }
    }

    private void PursueWithPath(Stack<Vector3Int> paths)
    {
        if (paths != null && paths.Count > 1)
        {
            Vector3 targetPos = paths.Peek();
            targetPos += new Vector3(0.5f, 0.5f, 0);

            if (Vector3.Distance(_enemyAI.transform.position, targetPos) < 0.5f
                && paths.Count > 1)
            {
                paths.Pop();
                targetPos = paths.Peek();
                targetPos += new Vector3(0.5f, 0.5f, 0);
            }

            Vector2 direction = (Vector2)(targetPos - _enemyAI.transform.position).normalized;
            AgentMovementParameter movementParameter = new AgentMovementParameter(direction, MoveType.Move);
            _enemyAI.Move(movementParameter);
        }
    }

    private Stack<Vector3Int> BuildPath()
    {
        Vector3Int targetValidPos = FindNearestValidPosition(_enemyAI.target.position);
        Vector3Int thisValidPos = FindNearestValidPosition(transform.position);

        Stack<Vector3Int> stack = astarAlgorithm.BuildPath(thisValidPos, targetValidPos);
        painter.ClearTilemap(painter.AstarTilemap);
        foreach (var pos in stack)
        {
            painter.DrawSingleTile(pos, painter.AstarTilemap, painter.testTile);
        }

        StartCoroutine(RebuildCoolTime());

        return stack;
    }

    private IEnumerator RebuildCoolTime()
    {
        isRebuilding = true;
        yield return new WaitForSeconds(1f);
        isRebuilding = false;
    }

    private Vector3Int FindNearestValidPosition(Vector3 position)
    {
        Vector3Int validPosition = Vector3Int.FloorToInt(position);

        if (dungeonData.totalObstaclePos.Contains(validPosition))
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (!dungeonData.totalObstaclePos.Contains(validPosition + new Vector3Int(i, j, 0)))
                        return validPosition + new Vector3Int(i, j, 0);
                }
            }
        }

        return validPosition;
    }
}
