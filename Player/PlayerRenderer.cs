using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Transform target;
    private Camera mainCamera;

    private bool isAttacking;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        target = GetComponentInParent<Transform>();
    }

    public void SpriteFlip(Component sender, object data)
    {
        if (!isAttacking)
        {
            Vector3 cursorPos = (Vector3)data;
            bool shouldFlip = Vector3.Cross(Vector3.up, cursorPos - target.position).z > 0;

            spriteRenderer.flipX = shouldFlip;
        }

    }

    public void blockSpriteFlip(Component sender, object attackCooltime)
    {
        if (!isAttacking)
        {
            StartCoroutine(WaitAttackCooltime((float)attackCooltime));
        }
    }

    private IEnumerator WaitAttackCooltime(float attackCooltime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooltime);
        isAttacking = false;
    }



}
