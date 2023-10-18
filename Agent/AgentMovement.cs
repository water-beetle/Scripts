using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentStat))]
[RequireComponent(typeof(Rigidbody2D))]
public class AgentMovement : MonoBehaviour
{
    private Rigidbody2D rgbd;
    private AgentStat agentStat;

    [SerializeField]
    private Vector2 currentVelocity;

    private bool isRush = false;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
        agentStat = GetComponent<AgentStat>();
        currentVelocity = Vector2.zero;
    }

    private void MoveAgentByVelocity(Vector2 direction)
    {
/*        Vector2 inputDirection = direction;

        if(inputDirection.magnitude == 0 && currentVelocity.magnitude >= 0)
        {
            currentVelocity -= currentVelocity.normalized * Time.deltaTime * Settings.Agent.deacceleration;
            if(currentVelocity.magnitude < 1)
            {
                currentVelocity = Vector2.zero;
            }
        }
        else
        {
            currentVelocity += (Vector2)direction * Time.deltaTime * Settings.Agent.acceleration;
        }
        
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, agentStat.GetStat(StatType.Speed));*/

        currentVelocity = (Vector2)(direction) * agentStat.GetStat(StatType.Speed);
    }

    private void StopAgent()
    {
        currentVelocity = Vector2.zero;
    }

    private IEnumerator RushAgentByImpulse(Vector2 direction)
    {
        isRush = true;
        rgbd.AddForce(direction * Settings.Agent.rushPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(Settings.Agent.rushDuration);
        rgbd.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        isRush = false;

    }

    private void FixedUpdate()
    {
        if(!isRush)
            rgbd.velocity = currentVelocity;
    }

    public void Move(Component sender, object data)
    {
        AgentMovementParameter agentMovementData = (AgentMovementParameter)data;

        switch (agentMovementData.moveType)
        {
            case MoveType.Move:
                MoveAgentByVelocity(agentMovementData.direction);
                break;
            case MoveType.Rush:
                if(!isRush)
                    StartCoroutine(RushAgentByImpulse(agentMovementData.direction));
                break;
            case MoveType.Stop:
                StopAgent();
                break;
        }
    }
}
