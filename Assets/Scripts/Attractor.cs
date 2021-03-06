using System.Collections;
using System.Collections.Generic;
using Assets.Scrips;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
	[Header("Variables")]
	[SerializeField] private float attractiveDistance = 10f;
	[SerializeField] private bool positive = true;
	[SerializeField] private bool magnetic = false;
	[SerializeField] private bool cinematic = false;

	[Header("Magnetize")]
	[SerializeField] private UnityEvent magnetizeEvent;
	[SerializeField] private UnityEvent demagnetizeEvent;

	[Header("Particles")]
	[SerializeField] private ParticleSystem positiveParticles;
	[SerializeField] private ParticleSystem negativeParticles;

	[Header("Materials")]
	[SerializeField] private SkinnedMeshRenderer meshRenderer;
	[SerializeField] private Material positiveMaterial;
	[SerializeField] private Material negativeMaterial;

	[Header("Touching")]
	[SerializeField] private float minDistance = 1.5f;
	[SerializeField] private float debouncingTime = 0.5f;
	[SerializeField] private UnityEvent touchEvent;
	
	private Material idleMaterial;
	private bool hitted = false;

	const float G = 667.4f;
	public static List<Attractor> attractors;
	private Rigidbody rb;
	private float debouncingTimeCurrent = 0f;
	public bool canRemoveConstraint = false;
	private RigidbodyConstraints _originalConstraint;

	private void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
		if (meshRenderer) this.idleMaterial = this.meshRenderer.material;
		SetMagnetic(magnetic);

		_originalConstraint = GetComponent<Rigidbody>().constraints;
	}

	private void FixedUpdate()
	{
		if(!magnetic) { return; }
		foreach (Attractor attractor in attractors)
		{
			if (attractor != this) { Attract(attractor); }
		}
	}

	private void Update()
	{
		debouncingTimeCurrent += Time.deltaTime;
	}

	private void Attract(Attractor objToAttract)
	{
		if (!objToAttract.magnetic) { return; }
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;
		SoundEvent(distance);

		if (distance == 0f || distance > this.attractiveDistance) { return; }

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		int opposite = objToAttract.positive == this.positive ? -1 : 1;
		rbToAttract.AddForce(force * opposite);
	}

	private void SoundEvent(float distance)
	{
		if(debouncingTimeCurrent < debouncingTime) { return; }
		if (!hitted && distance < minDistance)
		{
			hitted = true;
			touchEvent.Invoke();
		}
		else if (hitted == true && distance > minDistance)
		{
			hitted = false;
		}
	}

	private void OnEnable()
	{
		if (attractors == null) { attractors = new List<Attractor>(); }
		attractors.Add(this);
		//TODO: Add this to CinemachineTargetGroup 
	}

	private void OnDisable()
	{
		attractors.Remove(this);
		//TODO: Remove this to CinemachineTargetGroup 
	}

	public bool IsPositive()
	{
		return this.positive;
	}

	public bool IsMagnetic()
	{
		return this.magnetic;
	}
	
	public void SetMagnetic(bool _magnetic) // Called from AttractorManager
	{
		if(_magnetic == magnetic) { return; }
		this.magnetic = _magnetic;
		//this.agent.enabled = !_magnetic;
		//this.rb.isKinematic = false;
		this.rb.isKinematic = cinematic ? true : !_magnetic;
		SetGFX();
		if (_magnetic)
		{
			magnetizeEvent.Invoke();
			if (canRemoveConstraint) GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
		else
		{
			demagnetizeEvent.Invoke();
			AkSoundEngine.PostEvent("Stop_obj_Magnetized", gameObject);
			if (canRemoveConstraint) GetComponent<Rigidbody>().constraints = _originalConstraint;
		}
	}

	public void SetPositive(bool _positive) // Called from AttractorManager
	{
		this.positive = _positive;
		SetMagnetic(true);

		var soundClip = _positive ? "Play_Obj_Magnetized_Positive" : "Play_Obj_Magnetized_Negative";
		AkSoundEngine.PostEvent(soundClip, gameObject);
	}

	void SetGFX()
	{
		if(!this.magnetic) {
			if (meshRenderer) this.meshRenderer.material = this.idleMaterial;
		}
		else {
			if (meshRenderer) this.meshRenderer.material = (this.positive ? this.positiveMaterial : this.negativeMaterial); 
		}
		
		if(positiveParticles) {
			if(this.magnetic && this.positive) { positiveParticles.Play(); }
			else { positiveParticles.Stop(); }
		}
		if(negativeParticles) {
			if(this.magnetic && !this.positive) { negativeParticles.Play(); }
			else { negativeParticles.Stop(); }
		}
	}

/*#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, attractiveDistance);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, minDistance);
	}
#endif*/

}