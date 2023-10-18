using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveAnimation(Component sender, object data)
    {
        AgentMovementParameter agentMovementData = (AgentMovementParameter)data;
        Vector2 inputVector = agentMovementData.direction;

        if (inputVector.magnitude != 0)
            animator.SetBool(Settings.AnimatorParams.isMove, true);
        else
            animator.SetBool(Settings.AnimatorParams.isMove, false);
        
    }
}
