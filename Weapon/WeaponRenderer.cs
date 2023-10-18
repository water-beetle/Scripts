using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WeaponRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer weaponSprite;
    [SerializeField]
    private Transform playerTransform;

    // 공격 중 무기 이동처리 막기
    private bool isAttacking = false;

    private void Awake()
    {
        weaponSprite = GetComponentInChildren<SpriteRenderer>();
        playerTransform = GetComponentInParent<Transform>();
    }

    public void AimWeapon(Component sender, object data)
    {
        if(!isAttacking)
        {
            Vector3 pointerVector = (Vector3)data - playerTransform.position;
            bool isRight = Vector3.Cross(pointerVector, Vector3.up).z > 0;
            float angle = Mathf.Atan2(pointerVector.y, pointerVector.x) * Mathf.Rad2Deg;

            if (isRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                angle -= 180;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }

    public void blockAimWeapon(Component sender, object attackCooltime)
    {
        if(!isAttacking)
        {
            StartCoroutine(WaitAttackCooltime((float)attackCooltime));
        }
    }

    private IEnumerator WaitAttackCooltime(float attackCooltime)
    {
        isAttacking= true;
        yield return new WaitForSeconds(attackCooltime);
        isAttacking= false;
    }

}
