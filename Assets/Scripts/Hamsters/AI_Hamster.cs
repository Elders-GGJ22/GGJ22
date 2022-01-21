using System.Collections;
using System.Collections.Generic;
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
                int rand = Random.Range(1, 2);

                // TODO aggiungere qui le azioni possibili
                switch (rand)
                {
                    case 1:
                        yield return MuoviACaso();
                        break;
                    case 2:
                        yield return Scuotiti();
                        break;
                }

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
            yield return new WaitForSeconds(DURATA_PAUSA);
        }

        private IEnumerator MuoviACaso()
        {
            agent.destination = RandomPoint();
            while (agent.remainingDistance <= 0)
            {
                yield return new WaitForEndOfFrame();    
            }
        }

        private IEnumerator Scuotiti()
        {
            float DurataShake = 2;
            this.transform.DOShakePosition(DurataShake, 0.3f);
            
            yield return new WaitForSeconds(DurataShake);
        }
    }

}