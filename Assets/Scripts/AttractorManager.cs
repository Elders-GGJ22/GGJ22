using System.Collections;
using System.Collections.Generic;
using Assets.Scrips;
using UnityEngine;
using UnityEngine.Events;

public class AttractorManager : MonoBehaviour
{
    [SerializeField] private int positiveCharges = 1;
    [SerializeField] private int negativeCharges = 1;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        EventsManager.Instance.OnUsableChargesChanged(positiveCharges, negativeCharges);
        EventsManager.Instance.OnPositiveChargeEvent.AddListener(OnPositiveChargeConsumed);
        EventsManager.Instance.OnNegativeChargeEvent.AddListener(OnNegativeChargeConsumed);
    }
    
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
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                //Debug.Log("Tag " + hitInfo.transform.gameObject.tag);
                //if (hitInfo.transform.gameObject.tag == "Attractable")
                //{
                Attractor attractor = hitInfo.transform.GetComponent<Attractor>();
                if (!attractor)
                {
                    return;
                }

                if(leftClick)
                {
                    if(attractor.IsMagnetic())
                    {
                        if (attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            AddCharge(true);
                        }
                        else if(!attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            AddCharge(false);
                        }
                    }
                    else if(positiveCharges > 0)
                    {
                        attractor.SetPositive(true);
                        AddCharge(true, -1);
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
                            AddCharge(false);
                        }
                        else if (attractor.IsPositive())
                        {
                            attractor.SetMagnetic(false);
                            AddCharge(true);
                        }
                    }
                    else if (negativeCharges > 0)
                    {
                        attractor.SetPositive(false);
                        AddCharge(false, -1);
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

    public void AddCharge(bool positive, int quantity = 1)
    {
        if(positive)
        {
            AkSoundEngine.PostEvent("Play_Gun_Positive", gameObject);
            positiveCharges += quantity;
            StartCoroutine(RestorePositive());
        }
        else
        {
            AkSoundEngine.PostEvent("Play_Gun_Negative", gameObject);
            negativeCharges += quantity;
            StartCoroutine(RestoreNegative());
        }
        EventsManager.Instance.OnUsableChargesChanged(positiveCharges, negativeCharges);
    }

    private void OnNegativeChargeConsumed()
    {
        AddCharge(false, -1);
    }

    private void OnPositiveChargeConsumed()
    {
        
        AddCharge(true, -1);
        
    }

    IEnumerator RestorePositive()
    {
        yield return new WaitForSeconds(2);
        positiveCharges += 1;
        EventsManager.Instance.OnUsableChargesChanged(positiveCharges, negativeCharges);
    }

    IEnumerator RestoreNegative()
    {
        yield return new WaitForSeconds(2);
        negativeCharges += 1;
        EventsManager.Instance.OnUsableChargesChanged(positiveCharges, negativeCharges);
    }
}
