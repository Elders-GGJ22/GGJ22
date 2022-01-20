using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
	const float G = 667.4f;
	public static List<Attractor> attractors;
	[SerializeField] private bool positive = true;
	private Rigidbody rb;
	//public CinemachineTargetGroup ctg;

	private void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		foreach (Attractor attractor in attractors)
		{
			if (attractor != this)
				Attract(attractor);
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

	void Attract(Attractor objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;
		if (distance == 0f) { return; }

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		int opposite = objToAttract.positive == this.positive ? -1 : 1;
		rbToAttract.AddForce(force * opposite);
	}

}