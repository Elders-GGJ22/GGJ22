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
        /*enum AzioniPossibili
        {
            Muovi, 
            Pulisciti, 
            Vibra,
            GiocaAllaRuota,
            CercaCibo,
            Annusa
        }*/

        [Header("Variables")]
        [SerializeField] private Vector2 timeToWait = new Vector2(1f, 3f);
        [SerializeField] private float walkRadius = 3f;
        [SerializeField] private Transform goal;
        [SerializeField] private Transform GFX;

        private NavMeshAgent agent;
        private NavMeshPath path;
        private float waiting = 0;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (agent.enabled && waiting < 0)
            {
                // TODO aggiungere qui le azioni possibili
                switch (Random.Range(1, 3))
                {
                    case 1:
                        SetDestinationPoint();
                        break;
                    case 2:
                        Shake();
                        break;
                }
            }
            waiting -= Time.deltaTime;
            
            // Wwise
            float speed = wwiseFootStepCounter;
            //GetInput(out speed);
            // TODO calcola runtime la velocità del criceto
            ProgressStepCycle(speed);
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
            waiting = Random.Range(timeToWait.x, timeToWait.y);

            if (goal)
            {
                path = new NavMeshPath();
                agent.CalculatePath(goal.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    //TODO (maybe) calculate waiting = distance for goal (speed * distance)

                    /*foreach (Vector3 corner in path.corners)
                    {
                        if ((corner - this.transform.position).magnitude > walkRadius)
                        {
                            Debug.Log("vado a punto calcolato da corner");
                            Vector3 point = (corner - this.transform.position).normalized * walkRadius;
                            #if UNITY_EDITOR
                            path = new NavMeshPath();
                            agent.CalculatePath(point, path);
                            #endif
                            return;
                        }
                    }*/

                    Debug.Log("vado a punto preciso");
                    agent.destination = goal.position;
                    return;
                }
            }

            Debug.Log("vado a caso");
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            agent.destination = hit.position;

            #if UNITY_EDITOR
            path = new NavMeshPath();
            agent.CalculatePath(hit.position, path);
            #endif
        }

        public void Shake(float strenght = 0.3f)
        {
            Debug.Log("scuoto");
            waiting = Random.Range(timeToWait.x, timeToWait.y);
            agent.destination = this.transform.position;
            GFX.transform.DOShakePosition(waiting, strenght);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, walkRadius);

            if(path != null)
            {
                Gizmos.color = Color.magenta;
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
                }
            }
        }
        
        
        #region Wwise audio
        //Wwise Footstep Audio

        [Header("WWise")]
        public float wwiseFootStepCounter = 50;
        
        private void PlayFootstepAudio()
        {
            /*if (!m_CharacterController.isGrounded)
            {
                return;
            }

            // Make a ray-cast from player to ground and return RaycastHit to hit variable
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
            {
                PhysMat_Last = PhysMat;

                // Get the Tag from the collider and send it to PhysMat variable
                PhysMat = hit.collider.tag;

                // Avoid Unity to send Wwise Switch if the PhysMat variable has not changed
                if (PhysMat != PhysMat_Last)
                {
                    //Send the Switch "Material" to Wwise
                    AkSoundEngine.SetSwitch("Materials", PhysMat, gameObject);

                    // debugging purpose : log the PhysMat in Unity console
                    print(PhysMat);
                }
		
                // Post the Wwise AKEvent each time player step onto the ground
                AkSoundEngine.PostEvent("Play_FS_Pawn", gameObject);
	    
            }*/
            // Post the Wwise AKEvent each time player step onto the ground
            AkSoundEngine.PostEvent("Play_Hamster_Footstep", gameObject);
            Debug.Log("suono?");
        }

        private float nextFootstep = 0;
        void ProgressStepCycle(float speed)
        {
            nextFootstep += agent.velocity.magnitude;
            
            Debug.Log("footstep?? " + agent.velocity.magnitude);
            if (nextFootstep <= speed) return;

            nextFootstep = 0;
            PlayFootstepAudio();
        }
        
        
        
        #endregion
    }

}