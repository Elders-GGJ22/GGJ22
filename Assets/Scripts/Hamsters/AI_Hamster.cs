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

        private NavMeshAgent agent;
        private NavMeshPath path;
        private float timer = 0;
        private List<Transform> targetsList;
        private Transform oldTarget;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            targetsList = new List<Transform>();
            path = new NavMeshPath();
            AddTargets();
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
            Transform target = closestTarget();
            if(target == null) { return; }

            // Play hears particles
            if(target != oldTarget)
            {
                oldTarget = target;
                if(target.tag == HamsterUtils.TAG_BITCH)
                {
                    heartParticles.Play();
                }
                else
                {
                    heartParticles.Stop();
                }
            }

#if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(target.position, path);
#endif

            bool canDo = false;
            if (timer < 0.5f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                canDo = true;
                
                if (agent.remainingDistance <= agent.stoppingDistance && agent.remainingDistance != 0 && canDo)
                {
                    timer = 0; 
                    canDo = false;
                    targetsList.RemoveAt(targetsList.FindIndex(x => x.position == target.position));
                }
            }
            agent.destination = target.position;
        }

        private Transform closestTarget()
        {
            float minDistance = Mathf.Infinity;
            Transform target = null;
            foreach(Transform _target in targetsList)
            {
                //Vector3 targetsDist = Vector3.Distance(_target.position, transform.position);
                Vector3 targetsDist = transform.position - _target.position;
                float curDistance = targetsDist.sqrMagnitude;

                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    target = _target;
                }
            }
            return target;
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
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