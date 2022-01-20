using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
	[Header("Variables")]
	[SerializeField] private bool positive = true;
	[SerializeField] private bool magnetic = true;

	[Header("Materials")]
	[SerializeField] private Material idleMaterial;
	[SerializeField] private Material positiveMaterial;
	[SerializeField] private Material negativeMaterial;

	const float G = 667.4f;
	public static List<Attractor> attractors;
	private Rigidbody rb;
	private MeshRenderer meshRenderer;
	//public CinemachineTargetGroup ctg;

	private void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
		this.meshRenderer = this.GetComponent<MeshRenderer>();
		SetMaterial();
	}

	void FixedUpdate()
	{
		if(!magnetic) { return; }
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

	public bool IsPositive()
	{
		return this.positive;
	}


	public bool IsMagnetic()
	{
		return this.magnetic;
	}

	public void SetMagnetic(bool _magnetic)
	{
		this.magnetic = _magnetic;
		SetMaterial();
	}

	public void SetPositive(bool _positive) // Called from AttractorManager
	{
		this.magnetic = true;
		this.positive = _positive;
		SetMaterial();
	}

	void SetMaterial()
	{
		if(!this.magnetic) { this.meshRenderer.material = idleMaterial; }
		else if(this.positive) { this.meshRenderer.material = positiveMaterial; }
		else { this.meshRenderer.material = negativeMaterial; }
	}

	void Attract(Attractor objToAttract)
	{
		if(!objToAttract.magnetic) { return; }
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