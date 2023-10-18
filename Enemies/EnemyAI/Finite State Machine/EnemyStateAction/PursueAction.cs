using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PursueAction : StateAction
{


    private Stack<Vector3Int> paths = null;
    private bool isRebuilding = false;


    public override void Action(EnemyAI enemyAI)
    {
        if (!isRebuilding)
        {
            paths = BuildPath(enemyAI);
        }

        PursueWithPath(paths, enemyAI);
    }

    private Stack<Vector3Int> BuildPath(EnemyAI enemyAI)
    {
        Vector3Int targetValidPos = FindNearestValidPosition(enemyAI.target.position, enemyAI);
        Vector3Int thisValidPos = FindNearestValidPosition(enemyAI.transform.position, enemyAI);

        Stack<Vector3Int> stack = enemyAI.astarAlgorithm.BuildPath(thisValidPos, targetValidPos);
        enemyAI.painter.ClearTilemap(enemyAI.painter.AstarTilemap);
        foreach (var pos in stack)
        {
            enemyAI.painter.DrawSingleTile(pos, enemyAI.painter.AstarTilemap, enemyAI.painter.testTile);
        }

        StartCoroutine(RebuildCoolTime());

        return stack;
    }

    private Vector3Int FindNearestValidPosition(Vector3 position, EnemyAI enemyAI)
    {
        Vector3Int validPosition = Vector3Int.FloorToInt(position);

        if (enemyAI.dungeonData.totalObstaclePos.Contains(validPosition))
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (!enemyAI.dungeonData.totalObstaclePos.Contains(validPosition + new Vector3Int(i, j, 0)))
                        return validPosition + new Vector3Int(i, j, 0);
                }
            }
        }

        return validPosition;
    }

    private IEnumerator RebuildCoolTime()
    {
        isRebuilding = true;
        yield return new WaitForSeconds(1f);
        isRebuilding = false;
    }

    private void PursueWithPath(Stack<Vector3Int> paths, EnemyAI enemyAI)
    {
        if (paths != null && paths.Count > 1)
        {
            Vector3 targetPos = paths.Peek();
            targetPos += new Vector3(0.5f, 0.5f, 0);

            if (Vector3.Distance(enemyAI.transform.position, targetPos) < 0.5f
                && paths.Count > 1)
            {
                paths.Pop();
                targetPos = paths.Peek();
                targetPos += new Vector3(0.5f, 0.5f, 0);
            }

            Vector2 direction = (Vector2)(targetPos - enemyAI.transform.position).normalized;
            AgentMovementParameter movementParameter = new AgentMovementParameter(direction, MoveType.Move);
            enemyAI.Move(movementParameter);
        }
    }

}
