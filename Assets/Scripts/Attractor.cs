using System.Collections;
using System.Collections.Generic;
using Assets.Scrips;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
//using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(NavMeshAgent))]
public class Attractor : MonoBehaviour
{
	[Header("Variables")]
	[SerializeField] private bool positive = true;
	[SerializeField] private bool magnetic = true;
	//[SerializeField] private Vector2 walkEverySeconds = new Vector2(1, 3);
	//[SerializeField] private int walkRadius = 3;

	[Header("Magnetize")]
	[SerializeField] private UnityEvent magnetizeEvent;
	[SerializeField] private UnityEvent demagnetizeEvent;

	[Header("Materials")]
	[SerializeField] private Material idleMaterial;
	[SerializeField] private Material positiveMaterial;
	[SerializeField] private Material negativeMaterial;

	[Header("Touching")]
	[SerializeField] private float minDistance = 1.5f;
	//[SerializeField] private float debouncingTime = 0.5f;
	[SerializeField] private UnityEvent touchEvent;
	private bool hitted = false;

	const float G = 667.4f;
	public static List<Attractor> attractors;
	private Rigidbody rb;
	private MeshRenderer meshRenderer;
	//private NavMeshAgent agent;
	//private float debouncingTimeCurrent = 0f;
	//public CinemachineTargetGroup ctg;

	private void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
		//this.agent = this.GetComponent<NavMeshAgent>();
		//StartCoroutine(MoveRandomOverTime());
		this.meshRenderer = this.GetComponent<MeshRenderer>();
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

	/*private void Update()
	{
		debouncingTimeCurrent += Time.deltaTime;
	}*/

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
		//if(debouncingTimeCurrent < debouncingTime) { return; }
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

	/*IEnumerator MoveRandomOverTime()
	{
		while(true)
		{
			yield return new WaitForSeconds(Random.Range(walkEverySeconds.x, walkEverySeconds.y));
			MoveRandom();
		}
	}

	void MoveRandom()
	{
		if(!agent.enabled) { return; }
		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
		agent.destination = hit.position;
	}*/

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
		this.rb.isKinematic = !_magnetic;
		SetMaterial();
		if(_magnetic) { magnetizeEvent.Invoke(); }
		else { demagnetizeEvent.Invoke(); }
	}

	public void SetPositive(bool _positive) // Called from AttractorManager
	{
		this.positive = _positive;
		SetMagnetic(true);
	}

	void SetMaterial()
	{
		if(!this.magnetic) { this.meshRenderer.material = idleMaterial; }
		else if(this.positive) { this.meshRenderer.material = positiveMaterial; }
		else { this.meshRenderer.material = negativeMaterial; }
	}

	private void OnDrawGizmosSelected()
	{
		//Gizmos.color = Color.red;
		//Gizmos.DrawWireSphere(this.transform.position, walkRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, minDistance);
	}

}