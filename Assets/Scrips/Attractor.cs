using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
	const float G = 667.4f;
	public static List<Attractor> attractors;
	[SerializeField] private bool positive = true;
	[SerializeField] private bool interagible = true;
	private Rigidbody rb;

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

	public void Add(bool positive)
	{
		if(!interagible) { return; }
		//TODO
	}

	public void Remove(Rigidbody rb)
	{
		if (!interagible) { return; }
		//TODO
	}

	void OnEnable()
	{
		if (attractors == null) { attractors = new List<Attractor>(); }
		attractors.Add(this);
	}

	void OnDisable()
	{
		attractors.Remove(this);
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