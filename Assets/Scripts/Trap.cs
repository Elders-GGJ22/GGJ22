using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scrips;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Trap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Material bloodMaterial;
    [SerializeField] private Transform leftClamp;
    [SerializeField] private Transform rightClamp;
    [SerializeField] private Transform button;

    [Header("Child components")]
    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private MeshRenderer[] meshRenderers;

    [Header("Variables")]
    //[SerializeField] private Vector3 closingForce = new Vector3(0f, 10f, 0f);
    [SerializeField] private Vector3 angle = new Vector3(-60f, 0f, 0f);
    [SerializeField] private Vector3 buttonPress = new Vector3(0f, -0.23f, 0f);
    [SerializeField] private float choppingSpeed = 5f;
    [SerializeField] private float restartSpeed = 0.2f;
    [SerializeField] private float restartWaitingTime = 2f;
    [SerializeField] private float vanishTime = 2f;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private bool slerp = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onChopEvent;
    [SerializeField] private UnityEvent onDechopEvent;
    [SerializeField] private UnityEvent onExplodeEvent;

#if UNITY_EDITOR
    [Header("Testing")]
    [SerializeField] private bool testing = false;
    [SerializeField] private float chopAfter = 5f;
    [SerializeField] private float explodeAfter = 5f;
#endif

    private Rigidbody rb;
    private BoxCollider boxCollider;

    private Quaternion startLeftRotation;
    private Quaternion startRightRotation;
    private Vector3 startButtonPosition;

    private Quaternion finalLeftRotation;
    private Quaternion finalRightRotation;
    private Vector3 finalButtonPosition;

    private float restartCurrentSpeed = 0;
    private float restartWaitingCurrentTime = 0;
    private float choppingCurrentSpeed = 0;
    private bool blood;
    public bool isEnabled = true;

    public enum TrapState
    {
        Waiting,
        Chopping,
        Exploded,
        Chopped,
        End
    }
    
    public TrapState status;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        boxCollider = this.GetComponent<BoxCollider>();
        isEnabled = true;

        startLeftRotation = leftClamp.rotation;
        startRightRotation = rightClamp.rotation;
        startButtonPosition = button.position;

        finalLeftRotation = leftClamp.rotation * Quaternion.Euler(angle);
        finalRightRotation = rightClamp.rotation * Quaternion.Euler(angle);
        finalButtonPosition = button.position + buttonPress;
    }

    private void Update()
    {
        switch(status) 
        {
            case TrapState.End:
            {
                destroyTime -= Time.deltaTime;
                if(destroyTime <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
            }
            case TrapState.Exploded:
            {
                vanishTime -= Time.deltaTime;
                if(vanishTime <= 0)
                {
                    status = TrapState.End;
                    EnableChildColliders(false);
                }
                break;
            }
            case TrapState.Chopped:
            {
                if(restartCurrentSpeed >= 1f)
                {
                    restartWaitingCurrentTime -= Time.deltaTime;
                    if(restartWaitingCurrentTime <= 0)
                    {
                        onDechopEvent.Invoke();
                        restartCurrentSpeed = 0;
                        status = TrapState.Waiting;
                        isEnabled = true;
                    }
                }
                else {
                    restartCurrentSpeed += Time.deltaTime * restartSpeed;
                    if(slerp) {
                        leftClamp.rotation = Quaternion.Slerp(finalLeftRotation, startLeftRotation, restartCurrentSpeed);
                        rightClamp.rotation = Quaternion.Slerp(finalRightRotation, startRightRotation, restartCurrentSpeed);
                        button.position = Vector3.Slerp(finalButtonPosition, startButtonPosition, restartCurrentSpeed);
                    } else {
                        leftClamp.rotation = Quaternion.Lerp(finalLeftRotation, startLeftRotation, restartCurrentSpeed);
                        rightClamp.rotation = Quaternion.Lerp(finalRightRotation, startRightRotation, restartCurrentSpeed);
                        button.position = Vector3.Lerp(finalButtonPosition, startButtonPosition, restartCurrentSpeed);
                    }
                }
                break;
            }
            case TrapState.Chopping:
            {
                choppingCurrentSpeed += Time.deltaTime * choppingSpeed;
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
                    choppingCurrentSpeed = 0;
                    restartWaitingCurrentTime = restartWaitingTime;
                    status = TrapState.Chopped;
                    SetBloodMaterial();
                }
                break;
            }
#if UNITY_EDITOR
            case TrapState.Waiting:
            {
                if(testing)
                {
                    chopAfter -= Time.deltaTime;
                    explodeAfter -= Time.deltaTime;
                    if(chopAfter <= 0)
                    {
                        Chop();
                    }
                    else if(explodeAfter <= 0)
                    {
                        Explode();
                    }
                }
                break;
            }
#endif
            default: break;
        }
    }

    void OnMouseDown()
    {
        Chop();
    }

    public void Chop(bool _blood = true)
    {
        //EnableChildColliders(false);
        status = TrapState.Chopping;
        isEnabled = false;
        blood = _blood;
        //this.rb.AddForce(closingForce, ForceMode.Impulse);
        onChopEvent.Invoke();
        AkSoundEngine.PostEvent("PLAY_TAGLIOLA_SNAP", this.gameObject);
    }

    private void SetBloodMaterial()
    {
        if(!blood) { return; }
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = bloodMaterial;
        }
    }

    public void Explode()
    {
        status = TrapState.Exploded;

        onExplodeEvent.Invoke();

        boxCollider.enabled = false;
        EnableChildColliders(true);

        rb.isKinematic = true;
        EnableChildRigidbodies(false);
    }

    private void EnableChildColliders(bool _enable)
    {
        foreach (BoxCollider _boxCollider in boxColliders)
        {
            _boxCollider.enabled = _enable;
        }
    }

    private void EnableChildRigidbodies(bool _enable)
    {
        foreach (Rigidbody _rigidbody in rigidbodies)
        {
            _rigidbody.velocity = rb.velocity;
            _rigidbody.isKinematic = _enable;
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (!isEnabled)
        {
            Physics.IgnoreCollision(other, this.GetComponent<Collider>());
            return;
        }
        if (other.gameObject.tag == HamsterUtils.TAG_HAMSTER)
        {
            Chop();
            //TODO: Get component of Hamster and set death
        }
        else if(other.gameObject.tag == HamsterUtils.TAG_MAGNET || other.gameObject.tag == HamsterUtils.TAG_TRAP)
        {
            Explode();
        }
    }
}
