using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scrips;

public class TeslaCoil : MonoBehaviour
{
    [SerializeField] private Transform[] thunders;
    [SerializeField] private bool isOn = true;
    private Collider collider;

    private void Start()
    {
        collider = this.GetComponent<Collider>();
        TurnOn(isOn);
    }

    public bool IsOn()
    {
        return isOn;
    }

    public void TurnOn(bool _on = true)
    {
        collider.enabled = _on;
        foreach (Transform thunder in thunders)
        {
            thunder.gameObject.SetActive(_on);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOn) { return; }
        if (other.gameObject.tag == HamsterUtils.TAG_HAMSTER)
        {
            //TODO: Get component of Hamster and set death
        }
    }
}
