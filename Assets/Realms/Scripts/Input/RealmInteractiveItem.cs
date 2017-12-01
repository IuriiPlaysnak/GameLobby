using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(BoxCollider))]
public class RealmInteractiveItem : MonoBehaviour {

	public event Action OnClick;
	public event Action OnBack;
	public event Action OnOver;
	public event Action OnOut;
	public event Action<RaycastHit> OnMoveOver;

	[SerializeField]
	private bool _doShowCursor = true;

	public bool isClickable {
		get { return OnClick != null; }
	}

	public bool doShowCursor {
		get { return _doShowCursor; }
	}

	public void Click() {
		ProcessClick ();	
	}

	public void Back() {
		ProcessBack ();
	}

	public void Over() {
		ProcessOver ();
	}

	public void Out() {
		ProcessOut ();
	}

	public void MoveOver(RaycastHit hit) {
		ProcessMoveOver (hit);
	}
		
	private void ProcessBack() {

		if (OnBack != null)
			OnBack ();
	}

	private void ProcessClick() {

		if (OnClick != null)
			OnClick ();
	}

	private void ProcessOver() {

		if (OnOver != null)
			OnOver ();
	}

	private void ProcessMoveOver(RaycastHit hit) {

		if (OnMoveOver != null)
			OnMoveOver (hit);
	}

	private void ProcessOut() {
		if (OnOut != null)
			OnOut ();
	}

	static public void GetLocalHitData(RaycastHit hit, out Vector3 localColliderSize, out Vector3 localHitPoint ) {

		localColliderSize = hit.collider.gameObject.GetComponentInParent<Transform> ().InverseTransformVector (hit.collider.bounds.size);
		localHitPoint = hit.collider.gameObject.GetComponentInParent<Transform> ().InverseTransformPoint (hit.point);		
	}

	void OnDestroy() {

		OnClick = null;
		OnBack = null;
		OnOver = null;
		OnOut = null;
		OnMoveOver = null;
	}
}
