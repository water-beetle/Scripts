using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentThrow : MonoBehaviour
{
    [SerializeField] private Transform trajectoryParent;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject throwItem;

    private Vector3 playerPosition;
    private Vector3 targetPosition;
    private int trajectoryCircleNum = 10;
    private List<GameObject> trajectoryCircles = new List<GameObject>();

    private Vector3 startRelativeCenter;
    private Vector3 endRelativeCenter;
    private Vector3 centerPivot;

    private bool isShowTrajectory = false;

    private void Start()
    {
        for (int i = 0; i < trajectoryCircleNum; i++)
        {
            GameObject circle = Instantiate(circlePrefab, trajectoryParent);
            circle.SetActive(false);
            trajectoryCircles.Add(circle);
        }
    }

    public void SetTrajectory(bool b)
    {
        for (int i = 0; i < trajectoryCircleNum; i++)
        {
            trajectoryCircles[i].SetActive(b);
        }
        isShowTrajectory = b;
    }

    public void HideTrajectory(Component sender, object data)
    {
        SetTrajectory(false);
    }


    public void CalculateTrajectory(Component sender, object data)
    {
        if (!isShowTrajectory)
            return;

        playerPosition = transform.position;
        targetPosition = (Vector3)data;
        targetPosition.z = 0f;
        centerPivot = (playerPosition + targetPosition) * 0.5f;
        centerPivot -= new Vector3(0, Vector3.Distance(playerPosition, targetPosition)/1.5f);

        startRelativeCenter = playerPosition-centerPivot;
        endRelativeCenter = targetPosition - centerPivot;

        for (int i = 0; i < trajectoryCircleNum; i++)
        {
            trajectoryCircles[i].SetActive(true);
            trajectoryCircles[i].transform.position = Vector3.Slerp(startRelativeCenter, endRelativeCenter, (float)i/trajectoryCircleNum) + centerPivot;
        }
    }

    public void ThrowItem(ItemSO itemData)
    {
        StartCoroutine(ThrowCoroutine(itemData));
        SetTrajectory(false);
    }

    private IEnumerator ThrowCoroutine(ItemSO itemData)
    {
        GameObject objToSpawn = new GameObject("throwedItem");
        objToSpawn.AddComponent<SpriteRenderer>().sprite = itemData.itemImage;
        objToSpawn.GetComponent<SpriteRenderer>().sortingLayerName = "Props";
        objToSpawn.transform.position = transform.position;
        

        Vector3 copiedTargetPosition = targetPosition;
        Vector3 copiedStartRelativeCenter = startRelativeCenter;
        Vector3 copiedendRelativeCenter = endRelativeCenter;
        Vector3 copiedcenterPivot = centerPivot;


        float distance = Vector2.Distance(transform.position, copiedTargetPosition);
        // 던지는 속도도 특성으로 추가할지 고민중... 일단 그냥 변수로
        float throwSpeed = 5f;
        int count = 0;

        for (float i = 0; i < 1 + Time.deltaTime; i += (Time.deltaTime * throwSpeed) / Mathf.Sqrt(distance))
        {
            objToSpawn.transform.position = Vector3.Slerp(copiedStartRelativeCenter, copiedendRelativeCenter, i) + copiedcenterPivot;
            count += 1;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);
        itemData.UseItem(objToSpawn.transform);
        Destroy(objToSpawn);
    }

    public void CreateEffects(GameObject toCreate, float createCoolTime, int createSize, int damage)
    {

    }


}
