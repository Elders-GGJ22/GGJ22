using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Material bloodMaterial;
    [SerializeField] private Transform leftClamp;
    [SerializeField] private Transform rightClamp;
    [SerializeField] private Transform button;

    [Header("Variables")]
    [SerializeField] private Vector3 angle = new Vector3(-60f, 0f, 0f);
    [SerializeField] private Vector3 buttonPress = new Vector3(0f, -0.23f, 0f);
    [SerializeField] private float choppingSpeed = .5f;
    [SerializeField] private bool slerp = true;

    [Header("Testing")]
    [SerializeField] private bool testing = false;
    [SerializeField] private float chopAfter = 5f;

    private float choppingCurrentSpeed = 0;
    private bool isChopping = false;
    private bool open = true;
    private bool blood;

    private Quaternion startLeftRotation;
    private Quaternion startRightRotation;
    private Vector3 startButtonPosition;

    private Quaternion finalLeftRotation;
    private Quaternion finalRightRotation;
    private Vector3 finalButtonPosition;

    private void Start()
    {
        startLeftRotation = leftClamp.rotation;
        startRightRotation = rightClamp.rotation;
        startButtonPosition = button.position;

        finalLeftRotation = leftClamp.rotation * Quaternion.Euler(angle);
        finalRightRotation = rightClamp.rotation * Quaternion.Euler(angle);
        finalButtonPosition = button.position + buttonPress;
    }

    private void Update()
    {
        if(!open) { return; }
        if(isChopping)
        {
            Debug.Log("isChopping");

            choppingCurrentSpeed += Time.time * choppingSpeed;
            if(slerp) {
                leftClamp.rotation = Quaternion.Slerp(startLeftRotation, finalLeftRotation, choppingCurrentSpeed);
                rightClamp.rotation = Quaternion.Slerp(startRightRotation, finalRightRotation, choppingCurrentSpeed);
                button.position = Vector3.Slerp(startButtonPosition, finalButtonPosition, choppingCurrentSpeed);
            } else {
                leftClamp.rotation = Quaternion.Lerp(startLeftRotation, finalLeftRotation, choppingCurrentSpeed);
                rightClamp.rotation = Quaternion.Lerp(startRightRotation, finalRightRotation, choppingCurrentSpeed);
                button.position = Vector3.Lerp(startButtonPosition, finalButtonPosition, choppingCurrentSpeed);
            }

            if(choppingCurrentSpeed >= 1f)
            {
                StopChopping();
            }
        }
        else if(testing)
        {
            chopAfter -= Time.deltaTime;
            if(chopAfter <= 0)
            {
                StartChopping();
            }
        }
    }

    public void StartChopping(bool _blood = true)
    {
        Debug.Log("StartChopping");
        isChopping = true;
        blood = _blood;
        //TODO: Play chop sound
    }

    private void StopChopping()
    {
        Debug.Log("StopChopping");
        isChopping = false;
        open = false;
        if(blood)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = bloodMaterial;
            }
        }
    }

    public void Explode()
    {
        //TODO
    }
}
