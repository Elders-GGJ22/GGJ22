using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttractorManager : MonoBehaviour
{
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private float raycastLenght = 15f;
    [SerializeField] private UnityEvent eventsTriggerEnter;
    [SerializeField] private UnityEvent eventsTriggerExit;
    private bool calledInvoke = false;
    [HideInInspector] public Vector3 hittedPosition; //variables you can call from another script

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, raycastLenght, rayMask))
        {
            if (!calledInvoke)
            {
                hittedPosition = hit.collider.transform.position;
                Debug.Log("invoke enter");
                eventsTriggerEnter.Invoke();
                calledInvoke = true;
            }
        }
        else
        {
            if (calledInvoke)
            {
                Debug.Log("invoke exit");
                eventsTriggerExit.Invoke();
                calledInvoke = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        DrawHelperAtCenter(this.transform.position, this.transform.forward, Color.blue, raycastLenght);
    }

    private void DrawHelperAtCenter(Vector3 origin, Vector3 direction, Color color, float scale)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(origin, origin + direction * scale);
    }
}
