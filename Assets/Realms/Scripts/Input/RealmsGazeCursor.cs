using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealmsGazeCursor : MonoBehaviour {

	[SerializeField]
	private GameObject _normal;

	[SerializeField]
	private GameObject _timer;

	[SerializeField]
	private GameObject _invisible;

	void Awake() {

		_normal.SetActive (true);
		_timer.SetActive (false);
		_invisible.SetActive (false);
	}

	public void UpdateMode(Mode mode) {

		_normal.SetActive (mode == Mode.NORMAL);
		_timer.SetActive (mode == Mode.TIMER);
		_invisible.SetActive (mode == Mode.INVISIBLE);
	}

	public void UpdateTimer(float progress) {

		_timer.GetComponent<Renderer> ().material.SetFloat ("_ColorRampOffset", progress);
	}

	public enum Mode {
		NORMAL,
		TIMER,
		INVISIBLE
	}
}
