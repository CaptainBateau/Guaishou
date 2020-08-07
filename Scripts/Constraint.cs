using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint : MonoBehaviour
{
	public enum ConstraintType { Position,Rotation,PositionAndRotation,Aim }

	public Transform _target;
	public ConstraintType _constraintType;
	[Range(0, 1), Header("Only Apply To Position")]
	public float _weight = 1;

	[Space(10)]
	public Transform _flipTarget;

	//[HideInInspector]
	int _flip=1;

	Vector3 _initialLocalPosition;

	Quaternion _rotationOffset;
	Vector3 _localPosition;


	private void Awake()
	{
		if(_target)
		{
			switch(_constraintType)
			{
				case ConstraintType.Position:
					StartCoroutine(PositionSolving());
					break;
				case ConstraintType.Rotation:
					StartCoroutine(RotationSolving());
					break;
				case ConstraintType.PositionAndRotation:
					StartCoroutine(PositionSolving());
					StartCoroutine(RotationSolving());
					break;
				case ConstraintType.Aim:
					StartCoroutine(Aiming());
					break;
				default:
					break;
			}
		}

		if(_flipTarget)
		{
			StartCoroutine(FlipTargetChecking());
		}
	}

	IEnumerator FlipTargetChecking()
	{
		float initialXSign = Mathf.Sign(_flipTarget.localScale.x);
		while(true)
		{
			_flip = Mathf.Sign(_flipTarget.localScale.x) != initialXSign?-1:1;
			yield return null;
		}
	}

	IEnumerator PositionSolving()
	{
		_initialLocalPosition = transform.localPosition;
		_localPosition = _target.InverseTransformPoint(transform.position);

		yield return null;

		while(true)
		{
			transform.position = Vector3.Lerp(transform.parent.TransformPoint(_initialLocalPosition),_target.TransformPoint(_localPosition),_weight);
			yield return null;
		}
	}

	IEnumerator RotationSolving()
	{
		float angleOffset = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg - Mathf.Atan2(_target.right.y, _target.right.x) * Mathf.Rad2Deg;

		yield return null;

		float angle;
		Vector3 direction;
		while(true)
		{
			angle =  Mathf.Atan2(_target.right.y, _target.right.x) * Mathf.Rad2Deg + _flip *angleOffset;
			transform.eulerAngles = new Vector3(0, 0, angle);
			yield return null;
		}
	}

	IEnumerator Aiming()
	{
		Vector3 direction = _target.position - transform.position;
		float angleOffset = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg - Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		yield return null;

		float angle;
		while(true)
		{
			direction = _target.position - transform.position;
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0,0,((1-(_flip+1)*0.5f)*180) +(angle+ angleOffset));
			yield return null;
		}
	}
}
