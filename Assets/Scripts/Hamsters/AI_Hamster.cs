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

        private const float DURATA_PAUSA = 1.5f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            
            StartCoroutine(Brain());
        }

        /// <summary>
        /// Restituisce un punto randomico in un raggio di azione all'interno di una navmesh
        /// </summary>
        /// <param name="center">punto di partenza del raggio di ricerca</param>
        /// <param name="range">raggio di ricerca in u</param>
        /// <param name="result">vero se ha trovato un punto valido</param>
        /// <returns></returns>
        private bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        // al posto di update, utilizzo una coroutine in modo da avere molto più controllo su quando e come intervenire
        private IEnumerator Brain()
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
            StartCoroutine(Brain());
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
            if (RandomPoint(transform.position, 10, out var rand))
            {
                agent.destination = rand;                
            }

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