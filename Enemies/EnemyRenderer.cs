using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Transform target;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag(Settings.Tags.Player).transform;
    }

    private void Update()
    {
        SpriteFlip();
    }

    public void SpriteFlip()
    {
        bool shouldFlip = Vector3.Cross(Vector3.up, target.position - transform.position).z > 0;

        spriteRenderer.flipX = shouldFlip;
    }
}
