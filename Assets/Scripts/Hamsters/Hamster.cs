using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(AI_Hamster))]
    public class Hamster : MonoBehaviour
    {
        public enum HamsterState
        {
            Alive,
            // probabilmente attratto etc..
            Dead
        }
        

        [SerializeField]
        [Tooltip("Stato del criceto")]
        private HamsterState HState;

        // su cosa ho sbattuto?
        void OnTriggerEnter(Collider collision)
        {
            // raggiunto la casetta.
            if (collision.gameObject.tag == HamsterUtils.TAG_GOAL)
            {
                EventsManager.Instance.OnHamsterReachHouse();
            }

            if (collision.gameObject.tag == HamsterUtils.TAG_TRAP)
            {
                 
            }
        }
    }
}