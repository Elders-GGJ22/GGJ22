using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scrips.Hamsters;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Attractor[] hamsters;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetSpawnPoint;
    [SerializeField] private float minHeight = -5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        foreach(Attractor hamster in hamsters)
        {
            if(hamster.transform.position.y < minHeight)
            {
                if(spawnPoint)
                {
                    hamster.transform.position = spawnPoint.position;
                    hamster.SetMagnetic(false);

                    AI_Hamster ai = hamster.GetComponent<AI_Hamster>();
                    if (ai && targetSpawnPoint)
                    {
                        ai.SetDestinationPoint(targetSpawnPoint.position);
                    }
                }
                else
                {
                    //DEATH OF HAMSTER!
                }
            }
        }
    }
}
