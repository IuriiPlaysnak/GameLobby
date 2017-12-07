using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeClickMeter : MonoBehaviour {

	private const float MIN_FILL = 0f;

	public System.Action OnFull;
	public System.Action<float> OnProgress;

	[SerializeField]
	private float _fillInSpeed = 1f;

	[SerializeField]
	private float _coolDownSpeed = 1f;

	[SerializeField]
	private bool _autoReset = false;

	public bool autoReset {
		get {
			return _autoReset;
		}
		set {
			_autoReset = value;
		}
	}

	private float _currentProgress = 0f;
    public float GetCompletion() { return _currentProgress; }

	void Update () {

		if (_isReleased)
			UpdateRelease ();
	}

	private void UpdateRelease() {
		
		_currentProgress -= Time.deltaTime / _coolDownSpeed;

		if (_currentProgress <= MIN_FILL) {
			_currentProgress = MIN_FILL;
			_isReleased = false;
		}

		UpdateProgress (_currentProgress);
	}

	public void UpdateProgress() {

		_isReleased = false;
		UpdateProgress (_currentProgress + Time.deltaTime * _fillInSpeed);
	}

	private bool _isFull;
	private bool _isReleased;
	public void Release() {

		_isFull = false;
		_isReleased = true;
	}

	public void Reset() {
		_currentProgress = MIN_FILL;
		_isFull = false;
		if (OnProgress != null)
			OnProgress (_currentProgress);
	}

	private void UpdateProgress(float progress) {

		if (_isFull)
			return;

		_currentProgress = progress;

		if (_currentProgress >= 1f) {

			if (_autoReset) {
				_currentProgress = 0f;
				_isFull = false;
			} else {

				_currentProgress = 1f;
				_isFull = true;
			}

			if (OnFull != null)
				OnFull ();
		}

		if (OnProgress != null)
			OnProgress (progress);
	}

	void OnDestroy() {

		OnFull = null;
		OnProgress = null;
	}
}