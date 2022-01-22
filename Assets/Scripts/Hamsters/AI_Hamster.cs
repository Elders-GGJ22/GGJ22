using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scrips.Criceti
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
            //StartCoroutine(Brain());
        }
        
        private Vector3 AngentDestinationPoint()
        {
            if(goal)
            {
                path = new NavMeshPath();
                agent.CalculatePath(goal.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    /*foreach (Vector3 corner in path.corners)
                    {
                        if ((corner - this.transform.position).magnitude > walkRadius)
                        {
                            Debug.Log("vado a punto");   
                            return (corner - this.transform.position).normalized * walkRadius;
                        }
                    }*/

                    Debug.Log("vado a punto preciso");
                    return goal.position;
                }
            }

            Debug.Log("vado a caso");
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            return hit.position;
        }

        // al posto di update, utilizzo una coroutine in modo da avere molto più controllo su quando e come intervenire
        //private IEnumerator Brain()
        void Update()
        {
            //while (true)
            //{
            if (agent.enabled && waiting < 0)
            {
                waiting = Random.Range(timeToWait.x, timeToWait.y);
                // TODO aggiungere qui le azioni possibili
                switch (Random.Range(1, 3))
                {
                    case 1:
                        agent.destination = AngentDestinationPoint();
                        break;
                    case 2:
                        Debug.Log("scuoto");
                        GFX.transform.DOShakePosition(waiting, 0.1f);
                        //yield return Shake(2f);
                        break;
                }
            }

            //yield return new WaitForSeconds(Random.Range(timeToWait.x, timeToWait.y));
            //}
            waiting -= Time.deltaTime;
        }

        /*private IEnumerator Shake(float duration, float strenght = 0.3f)
        {
            this.transform.DOShakePosition(duration, strenght);
            yield return new WaitForSeconds(duration);
        }*/
        
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
    }

}