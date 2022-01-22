using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttractorManager : MonoBehaviour
{
    [SerializeField] private int positiveCharges = 1;
    [SerializeField] private int negativeCharges = 1;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);
        if (leftClick || rightClick)
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, maxDistance, layerMask);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                //Debug.Log("Tag " + hitInfo.transform.gameObject.tag);
                //if (hitInfo.transform.gameObject.tag == "Attractable")
                //{
                Attractor attractor = hitInfo.transform.GetComponent<Attractor>();
                if(!attractor) { return; }
                if(leftClick)
                {
                    if(attractor.IsMagnetic())
                    {
                        if (attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            positiveCharges++;
                        }
                        else if(!attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            negativeCharges++;
                        }
                    }
                    else if(positiveCharges > 0)
                    {
                        attractor.SetPositive(true);
                        positiveCharges--;
                    }
                    else
                    {
                        //TODO: some animation for understanding no ammo
                    }
                }
                else
                {
                    if (attractor.IsMagnetic())
                    {
                        if (!attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            negativeCharges++;
                        }
                        else if (attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            positiveCharges++;
                        }
                    }
                    else if (negativeCharges > 0)
                    {
                        attractor.SetPositive(false);
                        negativeCharges--;
                    }
                    else
                    {
                        //TODO: some animation for understanding no ammo
                    }
                }
                //}
            }
        }
    }
}
