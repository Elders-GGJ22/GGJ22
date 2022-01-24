using System.Collections;
using System.Collections.Generic;
using Assets.Scrips;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
	[Header("Variables")]
	[SerializeField] private bool positive = true;
	[SerializeField] private bool magnetic = true;
	[SerializeField] private bool cinematic = false;

	[Header("Magnetize")]
	[SerializeField] private UnityEvent magnetizeEvent;
	[SerializeField] private UnityEvent demagnetizeEvent;

	[Header("Particles")]
	[SerializeField] private ParticleSystem positiveParticles;
	[SerializeField] private ParticleSystem negativeParticles;

	[Header("GFX")]
	[SerializeField] private GameObject idleGFX;
	[SerializeField] private GameObject positiveGFX;
	[SerializeField] private GameObject negativeGFX;

	[Header("Touching")]
	[SerializeField] private float minDistance = 1.5f;
	[SerializeField] private float debouncingTime = 0.5f;
	[SerializeField] private UnityEvent touchEvent;
	private bool hitted = false;

	const float G = 667.4f;
	public static List<Attractor> attractors;
	private Rigidbody rb;
	private float debouncingTimeCurrent = 0f;

	private void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
		SetMagnetic(magnetic);
	}

	void FixedUpdate()
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

	void Attract(Attractor objToAttract)
	{
		if (!objToAttract.magnetic) { return; }
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;
		SoundEvent(distance);

		if (distance == 0f) { return; }

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		int opposite = objToAttract.positive == this.positive ? -1 : 1;
		rbToAttract.AddForce(force * opposite);
	}

	void SoundEvent(float distance)
	{
		if(debouncingTimeCurrent < debouncingTime) { return; }
		if (!hitted && distance < minDistance)
		{
			hitted = true;
			touchEvent.Invoke();
			EventsManager.Instance.SfxEvent_Collided.Post(this.gameObject);
			Debug.Log("minDistance in");
		}
		else if (hitted == true && distance > minDistance)
		{
			hitted = false;
			Debug.Log("minDistance out");
		}
	}

	void OnEnable()
	{
		if (attractors == null) { attractors = new List<Attractor>(); }
		attractors.Add(this);
		//TODO: Add this to CinemachineTargetGroup 
	}

	void OnDisable()
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
		this.magnetic = _magnetic;
		//this.agent.enabled = !_magnetic;
		this.rb.isKinematic = cinematic ? true : !_magnetic;
		SetGFX();
		if(_magnetic) { magnetizeEvent.Invoke(); }
		else { demagnetizeEvent.Invoke(); }
	}

	public void SetPositive(bool _positive) // Called from AttractorManager
	{
		this.positive = _positive;
		SetMagnetic(true);
	}

	void SetGFX()
	{
		if (idleGFX) { this.idleGFX.SetActive(!this.magnetic); }
		if (positiveGFX) { this.positiveGFX.SetActive(this.magnetic && this.positive); }
		if (negativeGFX) { this.negativeGFX.SetActive(this.magnetic && !this.positive); }
		if(positiveParticles) {
			if(this.magnetic && this.positive) { positiveParticles.Play(); }
			else { positiveParticles.Stop(); }
		}
		if(negativeParticles) {
			if(this.magnetic && !this.positive) { negativeParticles.Play(); }
			else { negativeParticles.Stop(); }
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, minDistance);
	}

}