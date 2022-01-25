using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AgentLocomotionStart : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 velocity;

    //--
    public bool RootM_byAgent, RootM_byAnimations;
    public float VelBoost;
    Vector3 worldDeltaPosition;

    //--
    public bool PullAvatarTowardsAgent, PullAgentTowardsAvatar;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        // agent.updateRotation = false;
    }

    void Update()
    {
        //--- Update BTrees Animations looking at NavAgent movements
        worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dz = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPos = new Vector2(dx * VelBoost, dz * VelBoost);
        velocity = deltaPos;

        bool shouldmove = agent.remainingDistance > agent.stoppingDistance;
        anim.SetBool("move", shouldmove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);

        // Update Animator parameters

        // Advanced - LookAt
        AgentLookAtStart lookAt = GetComponent<AgentLookAtStart>();
        if (lookAt)
            //We add + transform.forward because otherwise while in IDLE the Agent would look at his feet
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void OnAnimatorMove()
    {
        if (RootM_byAgent)
        {
            // Update postion using agent position
            transform.position = agent.nextPosition;
        }
        else if (RootM_byAnimations)
        {
            // Update position based on animations movements, using navigation surface height
            Vector3 pos = anim.rootPosition;
            pos.y = agent.nextPosition.y;
            transform.position = pos;

            if (Vector3.Distance(transform.position, agent.nextPosition) > agent.radius)
            {

                if (PullAvatarTowardsAgent)
                {
                    transform.position += (agent.nextPosition - transform.position) * 0.1f;
                }
                else if (PullAgentTowardsAvatar)
                {
                    agent.nextPosition += (transform.position-agent.nextPosition) * 0.1f;

                }
            }

        }
    }
}
