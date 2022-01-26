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

        private NavMeshAgent agent;
        private NavMeshPath path;
        private float waiting = 0;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent.enabled) SetDestinationPoint();
        }

        public Transform GetGoal()
        {
            return goal;
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
            path = new NavMeshPath();
            if(goal) {
                agent.CalculatePath(goal.position, path);
            }

            agent.path = path;

            #region Old

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