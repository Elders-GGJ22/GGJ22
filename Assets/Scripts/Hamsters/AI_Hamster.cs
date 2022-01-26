using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AI_Hamster : MonoBehaviour
    {
        [Header("Variables")] [SerializeField] private Vector2 timeToWait = new Vector2(1f, 3f);
        [SerializeField] private float walkRadius = 3f;

        [Header("Components")] [SerializeField]
        private Transform goal;

        [SerializeField] private Transform GFX;
        [SerializeField] private Animator anim;
        [SerializeField] private Transform[] targets;
        private NavMeshAgent agent;
        private NavMeshPath path;
        private float waiting = 0;
        private float timer;
        private int currWayPoints;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            timer = 0;
            currWayPoints = 0;
        }

        public Transform GetGoal()
        {
            return goal;
        }

        private void Update()
        {
            if (agent.enabled) SetDestinationPoint();
        }

        public void SetDestinationPoint(Vector3 destination)
        {
#if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(goal.position, path);
#endif
            agent.destination = destination;
            Debug.Log("vado a spawn point");
        }

         void SetDestinationPoint()
        {
            
            if (timer < 5)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (targets[currWayPoints] == null)
                {
                    currWayPoints = (currWayPoints + 1) % targets.Length;
                }

                if (agent.destination != targets[currWayPoints].position)
                {
                    agent.destination = targets[currWayPoints].position;
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    timer = 0;
                    currWayPoints = (currWayPoints + 1) % targets.Length;
                }
            }

            #region NewOldSystem

            // path = new NavMeshPath();
            //
            // agent.CalculatePath(goal.position, path);
            // agent.path = path;

            // float closestTargetDistance = float.MaxValue;
            // NavMeshPath Path = null;
            //NavMeshPath ShortestPath = null;
            //  if (path.status != NavMeshPathStatus.PathComplete)
            //   {
            //     for (int i = 0; i < targets.Length; i++)
            //     {
            //         if (targets[i] == null)
            //         {
            //             continue;
            //         }
            //
            //         Path = new NavMeshPath();
            //
            //         if (NavMesh.CalculatePath(transform.position, targets[i].position, agent.areaMask, Path))
            //         {
            //             float distance = Vector3.Distance(transform.position, Path.corners[0]);
            //
            //             for (int j = 1; j < Path.corners.Length; j++)
            //             {
            //                 distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
            //             }
            //
            //             if (distance < closestTargetDistance)
            //             {
            //                 closestTargetDistance = distance;
            //                 ShortestPath = Path;
            //             }
            //         }
            //     }
            //     Debug.Log(Path.status);
            //
            //     if (ShortestPath != null && ShortestPath.status == NavMeshPathStatus.PathComplete)
            //     {
            //         agent.SetPath(ShortestPath);
            //     }
            // }

            #endregion

            #region Old

            // path = new NavMeshPath();
            // agent.CalculatePath(goal.position, path);
            //
            // agent.path = path;

            // waiting = Random.Range(timeToWait.x, timeToWait.y);

            // if (path.status != NavMeshPathStatus.PathComplete)
            // {
            //     Vector3 randomDirection = transform.position + Random.insideUnitSphere * walkRadius;
            //     NavMeshHit hit;
            //     if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
            //     {
            //         agent.CalculatePath(hit.position, path);
            //         agent.path = path;
            //     }
            // }
            //  if (goal)
            // {
            // if (agent.path.status == NavMeshPathStatus.PathComplete)
            // {
            //     //TODO (maybe) calculate waiting = distance for goal (speed * distance)
            //
            //     /*foreach (Vector3 corner in path.corners)
            //     {
            //         if ((corner - this.transform.position).magnitude > walkRadius)
            //         {
            //             Debug.Log("vado a punto calcolato da corner");
            //             Vector3 point = (corner - this.transform.position).normalized * walkRadius;
            //             #if UNITY_EDITOR
            //             path = new NavMeshPath();
            //             agent.CalculatePath(point, path);
            //             #endif
            //             return;
            //         }
            //     }*/
            //
            //     Debug.Log("vado a punto preciso");
            //     agent.destination = goal.position;
            //     return;
            // }
            // }

//             Debug.Log("vado a caso");
//             Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
//             randomDirection += transform.position;
//             NavMeshHit hit;
//             NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
//             // if(hit.position != Vector3.positiveInfinity) {
//             //     agent.destination = hit.position;
//             // }
//
// #if UNITY_EDITOR
//             path = new NavMeshPath();
//             agent.CalculatePath(hit.position, path);
//             agent.path = path;
// #endif

            #endregion
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, walkRadius);

            if (path != null)
            {
                Gizmos.color = Color.magenta;
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
                }
            }
        }
#endif
    }
}