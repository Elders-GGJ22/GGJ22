using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Cannibal_AI : MonoBehaviour
{
    [SerializeField] private bool casualMove = false;
    private NavMeshAgent agent;

    private bool _waitingNewPosition = false;

    public Transform[] patrolTargets;
    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (casualMove) MoveRandom();

        agent.destination = patrolTargets[Random.Range(0, patrolTargets.Length - 1)].position;
    }

    private bool isWaiting = false;
    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        agent.destination = patrolTargets[Random.Range(0, patrolTargets.Length - 1)].position;
        isWaiting = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 0.1f && !isWaiting)
        {
            StartCoroutine(WaitAndMove());
        }

        if (anim) anim.SetFloat("Velocity", agent.velocity.magnitude);
        
        return;
        if (casualMove)
        {
            if (_waitingNewPosition) return;
            var dist = (transform.position - agent.destination).sqrMagnitude;
                
            if (dist <= 1.5f) {
                StartCoroutine(MoveRandomAgain());
            }

            return;
        }
    }

    void MoveRandom()
    {
        if (!agent.enabled)
        {
            return;
        }

        Vector3 randomDirection = Random.insideUnitSphere * 3;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 2, agent.areaMask);
     
        agent.destination = hit.position;
    }

    IEnumerator MoveRandomAgain()
    {
        _waitingNewPosition = true;
        yield return new WaitForSeconds(1.5f);
        MoveRandom();

        _waitingNewPosition = false;
    }
}