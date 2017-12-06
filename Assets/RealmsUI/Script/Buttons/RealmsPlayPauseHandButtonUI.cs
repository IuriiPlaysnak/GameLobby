using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlaysnakRealms
{
	public class RealmsPlayPauseHandButtonUI : MonoBehaviour
	{
		[SerializeField]
		private Image _background;

		[SerializeField]
		private Image _filling;

		[SerializeField]
		private UnityEngine.UI.Text _buttonLabel;

		public enum State : byte
		{
			PLAY,
			PAUSE
		}

		private State _state;

		public void OnClick ()
		{
			_state = _state == State.PAUSE ? State.PLAY : State.PAUSE;
			UpdateView();
		}

		public void OnOver() {
			_filling.enabled = true;
		}

		public void OnOut() {
			_filling.enabled = false;
		}

		public void SetState(State state) {

			_state = state;
			UpdateView ();
		}

		private void UpdateView() {

			switch (_state) {

			case State.PLAY:
				_buttonLabel.text = "❚❚";
				break;

			case State.PAUSE:
				_buttonLabel.text = "►";
				break;
			}
		}
	}
}

