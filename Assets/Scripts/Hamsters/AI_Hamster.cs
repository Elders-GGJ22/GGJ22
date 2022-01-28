using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AI_Hamster : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ParticleSystem heartParticles;

        [Header("Variables")]
        [SerializeField]  private float minDistance = 0.5f;
        [SerializeField]  private bool casualMove = false;

        private NavMeshAgent agent;
        private NavMeshPath path;
        private List<Transform> targetsList;
        private Transform oldTarget;
        private float timer = 0;
        
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            targetsList = new List<Transform>();
            path = new NavMeshPath();
            AddTargets();
            
            if (casualMove) MoveRandom();
        }

        private void AddTargets()
        {
            GameObject[] _goal = GameObject.FindGameObjectsWithTag(HamsterUtils.TAG_GOAL);
            foreach (GameObject obj in _goal)
            {
                targetsList.Add(obj.transform);
            }

            GameObject[] _bitch = GameObject.FindGameObjectsWithTag(HamsterUtils.TAG_BITCH);
            foreach (GameObject obj in _bitch)
            {
                targetsList.Add(obj.transform);
            }

            GameObject[] _trap = GameObject.FindGameObjectsWithTag(HamsterUtils.TAG_TRAP);
            foreach (GameObject obj in _trap)
            {
                targetsList.Add(obj.transform);
            }

            GameObject[] _seed = GameObject.FindGameObjectsWithTag(HamsterUtils.TAG_SEED);
            foreach (GameObject obj in _seed)
            {
                targetsList.Add(obj.transform);
            }
        }

        public Transform GetGoal()
        {
            return closestTarget();
        }

        private bool _waitingNewPosition = false;
        IEnumerator MoveRandomAgain()
        {
            _waitingNewPosition = true;
            yield return new WaitForSeconds(1.5f);
            MoveRandom();

            _waitingNewPosition = false;
        }

        private void Update()
        {
            if (casualMove)
            {
                if (_waitingNewPosition) return;
                var dist = (transform.position - agent.destination).sqrMagnitude;
                
                if (dist <= 1.5f) {
                    StartCoroutine(MoveRandomAgain());
                }

                return;
            }
            
            if (agent.enabled) SetDestinationPoint();
        }

        public void SetDestinationPoint(Vector3 destination)
        {
#if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(destination, path);
#endif
            agent.destination = destination;
        }

        private void SetDestinationPoint()
        {
            Transform target = closestTarget();
            
            // Search closest target
            if(target == null) {
#if UNITY_EDITOR
                path = null;
#endif
                return;
            }
            
            // Play hears particles and check if is alive
            if(target.tag == HamsterUtils.TAG_BITCH)
            {
                if (target.GetComponent<Hamster>().HState == Hamster.HamsterState.Dead)
                {
                    heartParticles.Stop();
                    targetsList.RemoveAt(targetsList.FindIndex(x => x.position == target.position));
                    oldTarget = null;
                    return;
                }
                heartParticles.Play();
            }
            else
            {
                heartParticles.Stop();
            }

            // If target is reached
            //if (agent.remainingDistance <= agent.stoppingDistance && agent.remainingDistance != 0)
            if (TargetDistance(target.position) <= minDistance)
            {
                targetsList.RemoveAt(targetsList.FindIndex(x => x.position == target.position));
                oldTarget = null;
                return;
            }

            // Return if same target
            if(target == oldTarget) { return; }
            oldTarget = target;

           

#if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(target.position, path);
#endif

            agent.destination = target.position;

            /*if (timer < 0.5f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (agent.remainingDistance <= agent.stoppingDistance && agent.remainingDistance != 0)
                {
                    timer = 0; 
                    targetsList.RemoveAt(targetsList.FindIndex(x => x.position == target.position));
                }
            }
            */
        }

        private Transform closestTarget()
        {
            if(targetsList == null) { return null; } 
            float minDistance = Mathf.Infinity;
            Transform target = null;
            foreach(Transform _target in targetsList)
            {
                float curDistance = TargetDistance(_target.position);
                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    target = _target;
                }
            }
            return target;
        }

        private float TargetDistance(Vector3 _target)
        {
                //Vector3 targetsDist = Vector3.Distance(_target.position, transform.position);
            return (transform.position - _target).sqrMagnitude;
        }
        
        void MoveRandom()
        {
            if(!agent.enabled) { return; }
            Vector3 randomDirection = Random.insideUnitSphere * 3;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 3, 1);
            agent.destination = hit.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, minDistance);

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