using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(Settings.AnimatorParams.isAttack);
    }

}
