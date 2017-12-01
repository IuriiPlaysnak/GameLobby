using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealmAutoplayController : MonoBehaviour {

	[SerializeField]
	private float _delay = 3f;

	public event System.Action OnStarted;
	public event System.Action OnStopped;
	public event System.Action OnComplete;
	public event System.Action<float> OnProgress;
	public event System.Action<int> OnTime;

	// Update is called once per frame
	void Update () {

		if (_isTimeTicking) {

			_timer -= Time.deltaTime;
			if (_timer <= 0) {

				Stop ();
				if (OnComplete != null)
					OnComplete ();
				
			} else {

				if (OnProgress != null)
					OnProgress (_timer / _delay);

				if (OnTime != null)
					OnTime ((int)Mathf.Ceil(_timer));
			}
		}
	}

	private bool _isTimeTicking;
	private float _timer;

	public void Start() {

		_timer = _delay;
		_isTimeTicking = true;
		if (OnStarted != null)
			OnStarted ();
	}

	public void Stop() {
		_isTimeTicking = false;
		if (OnStopped != null)
			OnStopped ();
	}

	public void Pause() {
		_isTimeTicking = false;
	}

	public void Resume() {
		_isTimeTicking = true;
	}

	public float delay {
		get { return _delay; }
		set { _delay = value; }
	}
}