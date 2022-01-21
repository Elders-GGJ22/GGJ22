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

        private NavMeshAgent agent;
        private IEnumerator mainRoutine;
        private const float DURATA_PAUSA = 1.5f;

        public float walkRadius;
        public GameObject goal;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            StartBrain();
        }

        public void StartBrain()
        {
            agent.enabled = true;
            mainRoutine = Brain();
            StartCoroutine(mainRoutine);
        }

        public void StopBrain()
        {
            agent.enabled = false;
            StopCoroutine(mainRoutine);
            mainRoutine = null;
        }
        

        /// <summary>
        /// Restituisce un punto randomico in un raggio di azione all'interno di una navmesh
        /// </summary>
        /// <param name="center">punto di partenza del raggio di ricerca</param>
        /// <param name="range">raggio di ricerca in u</param>
        /// <param name="result">vero se ha trovato un punto valido</param>
        /// <returns></returns>
        private Vector3 RandomPoint()
        {
            var path = new NavMeshPath();
            
            agent.CalculatePath(goal.transform.position, path);
            
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
                return goal.transform.position;

                
                //yield return MuoviAPunto(goal.transform.position);
            }

            Debug.Log("vado a caso");
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            return hit.position;
            
        }

        // al posto di update, utilizzo una coroutine in modo da avere molto più controllo su quando e come intervenire
        private IEnumerator Brain()
        {
            while (true)
            {
                /*var path = new NavMeshPath();
                bool reachable = agent.CalculatePath(goal.transform.position, path);
                Debug.Log("Raggiungo=" + reachable + path.status);
                if (reachable)
                {
                    Debug.Log("vado a punto");
                    yield return MuoviAPunto(goal.transform.position);
                }
                else
                {*/
                    int rand = Random.Range(1, 3);
                    // TODO aggiungere qui le azioni possibili
                    switch (rand)
                    {
                        case 1:
                            agent.destination = RandomPoint();
                            break;
                        case 2:
                            Debug.Log("scuoto");
                            yield return Scuotiti();
                            break;
                    }
                //}

                
                
                yield return PensaPrimaDiAgire();
            }
        }
        
        /// <summary>
        /// Attendi qualche istante prima di fare una nuova azione
        /// sono pigri criceti dopotutto
        /// </summary>
        /// <returns></returns>
        private IEnumerator PensaPrimaDiAgire()
        {
            yield return new WaitForSeconds(DURATA_PAUSA*Random.Range(1f,1.5f));
        }
        private IEnumerator MuoviAPunto(Vector3 goal)
        {
            agent.destination = goal;
            yield return null;
          /*  while (agent.remainingDistance <= 0)
            {
                yield return new WaitForEndOfFrame();    
            }*/
        }


        private IEnumerator Scuotiti()
        {
            float DurataShake = 2;
            this.transform.DOShakePosition(DurataShake, 0.3f);
            
            yield return new WaitForSeconds(DurataShake);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, walkRadius);
        }
    }

}