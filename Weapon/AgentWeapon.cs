using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    private bool shouldAttack = false;
    private bool isAttackingNow = false;

    public Transform circleOrigin;
    public float radius;

    // ���� weaponData�� ��Ÿ�� ����������� �����ϱ�
    private float attackCooltime = 1f;

    [SerializeField]
    private GameEventSO playerAttackEvent;
    [SerializeField]
    private WeaponDataSO weaponData;
    private GameObject weaponPrefab;



    private void Awake()
    {
        weaponPrefab = weaponData.weaponPrefab;
    }

    private void Start()
    {
        weaponPrefab.GetComponent<SpriteRenderer>().sprite = weaponData.weaponRenderer;
    }

    public void TryAttack()
    {
        shouldAttack = true;
    }

    public void StopAttack()
    {
        shouldAttack = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (shouldAttack && !isAttackingNow)
        {
            Attack();
            StartCoroutine(AttackCoolTimeCoroutine());
        }
        else
        {

        }
        
    }

    private IEnumerator AttackCoolTimeCoroutine()
    {
        isAttackingNow = true;
        yield return new WaitForSeconds(attackCooltime);
        isAttackingNow = false;
    }

    private void Attack()
    {
        // ���� �ִϸ��̼� ����
        // ���� ���� ������ ���߱�
        // ĳ���� �ٶ󺸴� ���� ������ ���߱�
        // ���⿡ ���� �� ã��
        // ���� ������ ������ �ֱ�

        DetectColliders();
        playerAttackEvent.Raise(this, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color= Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            if (collider.CompareTag(Settings.Tags.enemyBodyCollider))
            {
                Debug.Log("Attack!");
            }
        }
    }


}
