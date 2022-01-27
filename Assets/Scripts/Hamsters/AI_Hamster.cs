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

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AI_Hamster : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ParticleSystem heartParticles;

        [Header("Variables")]
        [SerializeField]  private float minDistance = 0.5f;

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

        private void Update()
        {
            if (agent.enabled) SetDestinationPoint();
        }

        public void SetDestinationPoint(Vector3 destination)
        {
#if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(destination, path);
#endif
            agent.destination = destination;
            Debug.Log("vado a spawn point");
        }

        private void SetDestinationPoint()
        {
            // Search closest target
            Transform target = closestTarget();
            if(target == null) {
#if UNITY_EDITOR
            path = null;
#endif
                return;
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

            // Play hears particles
            if(target.tag == HamsterUtils.TAG_BITCH)
            {
                heartParticles.Play();
            }
            else
            {
                heartParticles.Stop();
            }

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