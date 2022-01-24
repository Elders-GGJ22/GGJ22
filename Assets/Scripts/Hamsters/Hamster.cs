using System;
using UnityEngine;

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(AI_Hamster))]
    public class Hamster : MonoBehaviour
    {
        public enum HamsterType
        {
            Comune,
            Esplosivo,
            Magnetico,
            Puttana
        }

        public enum HamsterState
        {
            Alive,
            // probabilmente attratto etc..
            Dead
        }
        
        [SerializeField]
        [Tooltip("Tipo di criceto")]
        public HamsterType HType;
        
        [SerializeField]
        [Tooltip("Stato del criceto")]
        public HamsterType HState;

        public void Start()
        {
            
        }

        // su cosa ho sbattuto?
        void OnTriggerEnter(Collider collision)
        {
            // raggiunto la casetta.
            if (collision.gameObject.tag == HamsterUtils.TAG_GOAL)
            {
                EventsManager.Instance.OnHamsterReachHouse();
                
                // ora che fo? lo distruggo? 
            }
        }
    }
}