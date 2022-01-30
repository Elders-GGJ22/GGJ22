using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scrips.Hamsters
{
    public class Hamster : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle_bloodExplosion;
        [SerializeField] private ParticleSystem particle_bloodSteam;
        [SerializeField] private ParticleSystem particle_bloodSplit;

        [SerializeField] private HamsterExplosion _expolodedHamster;

        public enum HamsterState
        {
            Alive,
            // probabilmente attratto etc..
            Dead
        }
        
        public enum HamsterType
        {
            Normal,
            // probabilmente attratto etc..
            Puttana,
            Exploder
        }

        [SerializeField]
        [Tooltip("Stato del criceto")]
        public HamsterState HState;
        
        [SerializeField]
        [Tooltip("Tipo di criceto")]
        private HamsterType hamsterType;

        private void Awake()
        {
            //_expolodedHamster = GetComponent<HamsterExplosion>();
            HState = HamsterState.Alive;
            switch (hamsterType)
            {
                case HamsterType.Puttana:
                    var navmesh = GetComponent<NavMeshAgent>();
                    Destroy(navmesh);
                    break;
            }
        }
        // su cosa ho sbattuto?
        void OnTriggerEnter(Collider collision)
        {
            Debug.Log(collision.name);
            if (HState == HamsterState.Dead)
            {
                Physics.IgnoreCollision(collision, GetComponent<Collider>());
                return;
            }
            
            // raggiunto la casetta.
            if (collision.gameObject.tag == HamsterUtils.TAG_GOAL)
            {
                EventsManager.Instance.OnHamsterReachHouse();
                GetComponent<NavMeshAgent>().isStopped = true;
            }

            if (collision.gameObject.tag == HamsterUtils.TAG_TRAP)
            {
                if (!collision.GetComponent<Trap>().isEnabled)
                    return;
            }

            if (collision.gameObject.tag == HamsterUtils.TAG_PUNES ||
                collision.gameObject.tag == HamsterUtils.TAG_TRAP || 
                collision.gameObject.tag == HamsterUtils.TAG_BITCH)
            {
                HState = HamsterState.Dead;
                
                if (hamsterType != HamsterType.Puttana)
                {
                    StartCoroutine(IePushEventAfterTick());
                }
                particle_bloodExplosion.Play();
                particle_bloodSteam.Play();
                particle_bloodSplit.Play();
                if (_expolodedHamster) _expolodedHamster.Explode();
            }
        }

        IEnumerator IePushEventAfterTick()
        {
            yield return new WaitForEndOfFrame();
            EventsManager.Instance.OnHamsterDie(this.gameObject);
        }
    }
}