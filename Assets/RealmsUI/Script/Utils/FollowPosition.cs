using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour {

	[SerializeField]
	private GameObject _target;

	[SerializeField]
	private float _distance;
	
	void Update() {

		if (Vector3.Distance (_target.transform.position, gameObject.transform.position) > _distance) {

			gameObject.transform.rotation = _target.transform.rotation;
			gameObject.transform.position = _target.transform.position - gameObject.transform.forward * _distance;
		}
	}
}