using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {
	
	public class ButtonWithCyclingClickMeterUI : ButtonWithClickMeterUI {

		private enum Direction {
			LEFT,
			RIGHT
		}

		private enum FillingType {
			FORWARD,
			BACKWARD
		}

		private enum FillOrigin	{
			LEFT = 0,
			RIGHT = 1
		}

		[SerializeField]
		private Direction _direction = Direction.RIGHT;

		private FillingType _fillingType = FillingType.FORWARD;

		protected override void Init ()
		{
			base.Init ();
			gauge.fillOrigin = _direction == Direction.LEFT ? (int)FillOrigin.LEFT : (int)FillOrigin.RIGHT;
		}

		void Update() {

			if (_isReleased) {
				UpdateRelease ();
			}
		}

		private float _releaseProgress;
		private void UpdateRelease ()
		{
			_releaseProgress -= Time.deltaTime / 0.5f;

			if (_releaseProgress <= 0) {

				if (_fillingType == FillingType.FORWARD) {
					ForwardReleaseComplete ();
				} else {
					BackwardReleaseComplete ();
				}
			}

			UpdateGauge (_releaseProgress);
		}

		private void BackwardReleaseComplete() {

			_releaseProgress += 1f;
			_fillingType = FillingType.FORWARD;
			gauge.fillOrigin = _direction == Direction.LEFT ? (int)FillOrigin.LEFT : (int)FillOrigin.RIGHT;
		}

		private void ForwardReleaseComplete() {
			
			_releaseProgress = 0f;
			_fillingType = FillingType.FORWARD;
			gauge.fillOrigin = _direction == Direction.LEFT ? (int)FillOrigin.LEFT : (int)FillOrigin.RIGHT;
			_isReleased = false;
		}

		private bool _isReleased;
		private float _prevProgress;
		override public void UpdateClickMeter (float progress)
		{
			float dProgress = progress - _prevProgress;
			_isReleased = dProgress < 0 && Mathf.Abs (dProgress) < 0.5f && progress == 0f;
			_prevProgress = progress;
			_releaseProgress = progress;

			UpdateGauge (progress);
		}

		private void UpdateGauge(float progress) {
			gauge.fillAmount = _fillingType == FillingType.FORWARD ? progress : 1f - progress;
		}

		public override void OnClick ()
		{
			_fillingType = _fillingType == FillingType.FORWARD ? FillingType.BACKWARD : FillingType.FORWARD;
			gauge.fillOrigin = gauge.fillOrigin == (int)FillOrigin.LEFT ? (int)FillOrigin.RIGHT : (int)FillOrigin.LEFT;
		}
	}
}