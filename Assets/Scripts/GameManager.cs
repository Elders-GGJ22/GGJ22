using System.Collections;
using System.Collections.Generic;
using Assets.Scrips;
using UnityEngine;
using Assets.Scrips.Hamsters;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [Header("Managers")]
	[SerializeField] private AttractorManager attractorManager;

    [Header("Hamsters")]
    [SerializeField] private Attractor[] hamsters;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetSpawnPoint;
    [SerializeField] private float minHeight = -5f;

    [Header("Variables")]
    [SerializeField] private bool reloadWhenRespan = true;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        EventsManager.Instance.OnHamsterSpawnEvent.AddListener(AddNewHamsterToCinematicTarget);
        AssignTargetsToTargetGroup();
    }

    void AddNewHamsterToCinematicTarget(GameObject newTarget)
    {
        targetGroup.AddMember(newTarget.transform,1,0);
    }
    

    void AssignTargetsToTargetGroup()
    {
        foreach (Attractor hamster in hamsters)
        {
            AddHamster(hamster.transform);
        }
    }

    public void AddHamster(Transform hamster)
    {
        targetGroup.AddMember(hamster, 1, 1);
        AI_Hamster ai = hamster.GetComponent<AI_Hamster>();
        if (ai)
        {
            Transform goal = ai.GetGoal();
            if(goal != null)
            {
                targetGroup.AddMember(goal, 1, 1);
            }
        }
    }

    public void RemoveHamster(Transform hamster)
    {
        targetGroup.RemoveMember(hamster);
        AI_Hamster ai = hamster.GetComponent<AI_Hamster>();
        if (ai)
        {
            Transform goal = ai.GetGoal();
            if (goal != null)
            {
                targetGroup.RemoveMember(goal);
            }
        }
    }

    void FixedUpdate()
    {
        foreach(Attractor hamster in hamsters)
        {
            Debug.Log(hamster);
            if(hamster.transform.position.y < minHeight)
            {
                Respawn(hamster);
            }
        }
    }

    void Respawn(Attractor hamster)
    {
        if(spawnPoint)
        {
            hamster.transform.position = spawnPoint.position;
            hamster.SetMagnetic(false);

            if(reloadWhenRespan) {
                attractorManager.AddCharge(hamster.IsPositive());
            }

            AI_Hamster ai = hamster.GetComponent<AI_Hamster>();
            if (ai && targetSpawnPoint)
            {
                ai.SetDestinationPoint(targetSpawnPoint.position);
            }
        }
        else
        {
            //TODO: DEATH OF HAMSTER!
        }
    }
}
