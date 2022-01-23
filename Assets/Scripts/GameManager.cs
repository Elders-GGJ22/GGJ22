using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Attractor[] humsters;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minHeight = -5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        foreach(Attractor humster in humsters)
        {
            if(humster.transform.position.y < minHeight)
            {
                humster.transform.position = spawnPoint.position;
                humster.SetMagnetic(false);
            }
        }
    }
}
