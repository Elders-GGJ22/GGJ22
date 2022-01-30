using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scrips.Hamsters;

public class HamsterExplosion : MonoBehaviour
{       
    [Header("Components")]
    [SerializeField] private Transform[] toDeactivateTransform;
    [SerializeField] private Collider[] explodedColliders;

    [Header("Variables")]
    [SerializeField] private float vanishTime = 2f;
    [SerializeField] private float destroyTime = 2f;

#if UNITY_EDITOR
    [Header("Testing")]
    [SerializeField] private bool testing = false;
    [SerializeField] private float explodeAfter = 5f;
#endif

    private Attractor attractor;
    private AI_Hamster ai;
    private UnityEngine.AI.NavMeshAgent agent;

    public enum ExplosionState
    {
        Waiting,
        Exploded,
        End
    }
    
    private ExplosionState status;

    private void Start()
    {
        attractor = this.GetComponent<Attractor>();
        ai = this.GetComponent<AI_Hamster>();
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    public void Explode()
    {
        if(status != ExplosionState.Waiting) { return; }
        status = ExplosionState.Exploded;
        //TODO: sound
        SetAllActive(toDeactivateTransform, false);
        SetAllActive(explodedColliders, true);
        if(attractor) { attractor.enabled = false; }
        if(ai) { ai.enabled = false; }
        if(agent) { agent.enabled = false; }
    }

    private void Update()
    {
        switch(status) 
        {
            case ExplosionState.End:
            {
                destroyTime -= Time.deltaTime;
                if(destroyTime <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
            }
            case ExplosionState.Exploded:
            {
                vanishTime -= Time.deltaTime;
                if(vanishTime <= 0)
                {
                    status = ExplosionState.End;
                    EnableAllColliders(explodedColliders, false);
                }
                break;
            }
/*#if UNITY_EDITOR
            case ExplosionState.Waiting:
            {
                if(testing)
                {
                    explodeAfter -= Time.deltaTime;
                    if(explodeAfter <= 0)
                    {
                        Explode();
                    }
                }
                break;
            }
#endif*/
            default: break;
        }
    }

    private void SetAllActive(Transform[] _transforms, bool _active = true)
    {
        foreach (Transform _transform in _transforms)
        {
            _transform.gameObject.SetActive(_active);
        }
    }

    private void SetAllActive(Collider[] _colliders, bool _active = true)
    {
        foreach (Collider _collider in _colliders)
        {
            _collider.gameObject.SetActive(_active);
        }
    }

    private void EnableAllColliders(Collider[] _colliders, bool _enable = true)
    {
        foreach (Collider _collider in _colliders)
        {
            _collider.enabled = _enable;
        }
    }
}