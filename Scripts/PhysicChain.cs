using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PhysicChain : MonoBehaviour
{
	[Header("Here you need to set every member of the descending hierarchy that does not belong in the chain")]
    public Transform[] _exclude = new Transform[0];

    public float _angleLimit=60;

	[HideInInspector]
	public Transform _target;
	[HideInInspector]
	Link[] _savedLinks;
	List<Link> _links = new List<Link>();

	[HideInInspector]
	public Rigidbody2D _rigidbody;

	JointAngleLimits2D _jointLimit;

	private void Awake()
	{
		if(_target == null) Destroy(this);
	}

	private void FixedUpdate()
	{
		_rigidbody.MovePosition(_target.TransformPoint(Vector3.zero));
	}

	[ContextMenu("Create Chain")]
	public void CreateChain()
	{
		_target = new GameObject("ChainAnchor").transform;
		_target.position = transform.position;
		_target.SetParent(transform.parent);

		_rigidbody = GetComponent<Rigidbody2D>();
		CreateAllLinks(_rigidbody);
		_savedLinks = _links.ToArray();
	}

	[ContextMenu("Clear Chain")]
	public void ClearChain()
	{
		transform.SetParent(null);
		if(_target!=null) DestroyImmediate(_target.gameObject);

		_target = null;

		for(int i = 0; i < _savedLinks.Length; i++)
		{
			DestroyImmediate(_savedLinks[i]._joint);
			DestroyImmediate(_savedLinks[i]._rigidbody);
			if(_savedLinks[i]._connectedRigidbody != _rigidbody) DestroyImmediate(_savedLinks[i]._connectedRigidbody);
		}
		_savedLinks = new Link[0];
		_links.Clear();
	}


	void CreateAllLinks(Rigidbody2D parent)
	{
		Rigidbody2D[] parents = new Rigidbody2D[1] { parent };
		CreateAllLinks(parents);
	}
	void CreateAllLinks(Rigidbody2D[] parents)
	{
		List<Rigidbody2D> newParents = new List<Rigidbody2D>();

		Rigidbody2D rigidbodyTemp;

		_jointLimit = new JointAngleLimits2D();
		_jointLimit.min = -_angleLimit * 0.5f;
		_jointLimit.max = _angleLimit * 0.5f;

		for(int i = 0; i < parents.Length; i++)
		{
			for(int j= 0; j < parents[i].transform.childCount; j++)
			{
				if(!IsExclude(parents[i].transform.GetChild(j)))
				{
					rigidbodyTemp = parents[i].transform.GetChild(j).gameObject.AddComponent<Rigidbody2D>();
					if(parents[i].transform.GetChild(j).childCount>0) newParents.Add(rigidbodyTemp);
					_links.Add(new Link(rigidbodyTemp, parents[i], _jointLimit));
				}
			}
		}
		if(newParents.Count > 0) CreateAllLinks(newParents.ToArray());
		newParents.Clear();
	}

	bool IsExclude(Transform toCheck)
	{
		for(int i = 0; i < _exclude.Length; i++)
		{
			if(toCheck == _exclude[i]) return true;
		}
		return false;
	}
}

public struct Link
{
	public Rigidbody2D _rigidbody;
	public Rigidbody2D _connectedRigidbody;
	public HingeJoint2D _joint;

	public Link(Rigidbody2D body, Rigidbody2D connectedBody, JointAngleLimits2D limits)
	{
		_rigidbody = body;
		_connectedRigidbody = connectedBody;

		_joint = body.gameObject.AddComponent<HingeJoint2D>();
		_joint.connectedAnchor = _rigidbody.transform.localPosition;
		_joint.connectedBody = connectedBody;
		_joint.useLimits = true;
		_joint.limits = limits;

	}
}
