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
        [Header("Variables")] [SerializeField] private Vector2 timeToWait = new Vector2(1f, 3f);
        [SerializeField] private float walkRadius = 3f;

        [Header("Components")] 
        [SerializeField]private Transform goal;
        [SerializeField] private Transform GFX;
        [SerializeField] private Animator anim;
        private NavMeshAgent agent;
        private NavMeshPath path;
        private float waiting = 0;
        private float timer;
        private int currWayPoints;
        private GameObject[] _goal, _bitch, _trap, _seed;
        private List<Transform> _targets;
        private Vector3 targetsDist;
        
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            timer = 0;
            currWayPoints = 0;
            _targets = new List<Transform>();
            path = new NavMeshPath();
            AddTargets();
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
            float minDistance = Mathf.Infinity;
            Transform target = null;
            
            for (int i = 0; i < _targets.Count; i++)
            {
                //targetsDist = Vector3.Distance(_targets[i].position, transform.position);
                targetsDist = transform.position - _targets[i].position;
                float curDistance = targetsDist.sqrMagnitude;

                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    target = _targets[i];
                }
            }

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
                    _targets.RemoveAt(_targets.FindIndex(x => x.position == target.position));
                }
            }
            agent.destination = target.position;
        }
        private void AddTargets()
        {
            if (_goal == null)
            {
                _goal = GameObject.FindGameObjectsWithTag("Goal");
                foreach (var obj in _goal)
                {
                    _targets.Add(obj.transform);
                }
            }
            if (_bitch == null)
            {
                _bitch = GameObject.FindGameObjectsWithTag("Bitch");
                foreach (var obj in _bitch)
                {
                    _targets.Add(obj.transform);
                }
            }
            if (_trap == null)
            {
                _trap = GameObject.FindGameObjectsWithTag("Trap");
                foreach (var obj in _trap)
                {
                    _targets.Add(obj.transform);
                }
            }
            if (_seed == null)
            {
                _seed = GameObject.FindGameObjectsWithTag("Seed");
                foreach (var obj in _seed)
                {
                    _targets.Add(obj.transform);
                }
            }
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