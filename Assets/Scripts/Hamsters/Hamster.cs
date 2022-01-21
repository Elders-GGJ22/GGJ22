using System;
using UnityEngine;

namespace Assets.Scrips.Criceti
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
            // in base al tipo prepara
        }
    }
}