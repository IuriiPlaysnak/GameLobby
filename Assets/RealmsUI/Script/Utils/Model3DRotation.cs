using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model3DRotation : MonoBehaviour {

	void Awake() {

		RealmInteractiveItem ii = gameObject.GetComponent<RealmInteractiveItem> ();
		if (ii != null) {
			ii.OnOver += Ii_OnOver;
			ii.OnMoveOver += Ii_OnMoveOver;
		}
	}

	void Ii_OnMoveOver (RaycastHit obj)
	{
		transform.Rotate (new Vector3 (0f, 1f, 0f));
	}

	void Ii_OnOver ()
	{
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
