using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scrips.Hamsters
{
    [RequireComponent(typeof(Animator))]
    public class Hamster_animator : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private NavMeshAgent agent;

        [Header("WWise")]
        [SerializeField] private float wwiseFootStepCounter = 50;

        private Animator anim;
        private float nextFootstep = 0;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            // Wwise
            float speed = wwiseFootStepCounter;
            //GetInput(out speed);
            // TODO calcola runtime la velocità del criceto
            ProgressStepCycle(speed);
        }

        private void OnAnimatorMove()
        {
            if(anim) { anim.SetFloat("Velocity", agent.velocity.magnitude); }
        }

        public void SetMagneticAnimation(bool magnetic)
        {
            if (anim) anim.SetBool("Magnetic", magnetic);
        }

        private void PlayFootstepAudio()
        {
            /*if (!m_CharacterController.isGrounded)
            {
                return;
            }

            // Make a ray-cast from player to ground and return RaycastHit to hit variable
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
            {
                PhysMat_Last = PhysMat;

                // Get the Tag from the collider and send it to PhysMat variable
                PhysMat = hit.collider.tag;

                // Avoid Unity to send Wwise Switch if the PhysMat variable has not changed
                if (PhysMat != PhysMat_Last)
                {
                    //Send the Switch "Material" to Wwise
                    AkSoundEngine.SetSwitch("Materials", PhysMat, gameObject);

                    // debugging purpose : log the PhysMat in Unity console
                    print(PhysMat);
                }
		
                // Post the Wwise AKEvent each time player step onto the ground
                AkSoundEngine.PostEvent("Play_FS_Pawn", gameObject);
	    
            }*/
            // Post the Wwise AKEvent each time player step onto the ground
            //AkSoundEngine.PostEvent("Play_Hamster_Footstep", gameObject);
            Debug.Log("suono?");
        }

        void ProgressStepCycle(float speed)
        {
            nextFootstep += agent.velocity.magnitude;

            //Debug.Log("footstep?? " + agent.velocity.magnitude);
            if (nextFootstep <= speed) return;

            nextFootstep = 0;
            PlayFootstepAudio();
        }
    }

}