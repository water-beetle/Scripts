using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireEffect : AbstractEffect
{
    private CircleCollider2D circleCollider;

    private float fireSpreadTime = Settings.fireSpreadTime;
    private float fireExtinguishTime = Settings.fireExtinguishTime;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public override void Init(int _nums)
    {
        base.Init(_nums);
        FireSpreads();
    }


    public void FireSpreads()
    {

        Vector3Int validPos = FindNearestValidPosition(transform.position);

        if (!DungeonData.Instance.totalFloorPos.Contains(validPos))
        {
            Destroy(gameObject);
        }

        transform.position = validPos;
        Vector3Int nextFirePos = FindNextPosition(validPos);

        StartCoroutine(MakeFire(nextFirePos));
        StartCoroutine(Extinguish());
    }

    private IEnumerator MakeFire(Vector3Int nextFirePos)
    {
        yield return new WaitForSeconds(fireSpreadTime);
        FireEffect newFire = Instantiate(this);
        newFire.transform.position = nextFirePos;
        newFire.Init(nums - 1);
    }

    private IEnumerator Extinguish()
    {
        yield return new WaitForSeconds(fireExtinguishTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 데미지 주기
    }

    private Vector3Int FindNearestValidPosition(Vector3 position)
    {
        Vector3Int validPosition = Vector3Int.FloorToInt(position);

        if (DungeonData.Instance.totalObstaclePos.Contains(validPosition))
        {
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (!DungeonData.Instance.totalObstaclePos.Contains(validPosition + new Vector3Int(i, j, 0)))
                        return validPosition + new Vector3Int(i, j, 0);
                }
            }
        }

        return validPosition;
    }

    private Vector3Int FindNextPosition(Vector3Int position)
    {

        List<Vector3Int> temp = new List<Vector3Int>();

        for(int i= -1; i < 1; i++)
        {
            for(int j = -1; j < 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                if (!DungeonData.Instance.totalObstaclePos.Contains(position + new Vector3Int(i, j, 0)))
                    temp.Add(position + new Vector3Int(i, j, 0));

            }
        }

        if(temp.Count == 0)
        {
            return position;
        }

        int randNum = Random.Range(0, temp.Count);
        return temp[randNum];
    }
}
