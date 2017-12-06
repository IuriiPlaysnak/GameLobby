using UnityEngine;
using UnityEngine.UI;

namespace PlaysnakRealms
{
	public class RealmsMuteHandButtonUI : MonoBehaviour
	{
		[SerializeField]
		private Image _background;

		[SerializeField]
		private Image _filling;

		[SerializeField]
		private UnityEngine.UI.Text _buttonLabel;

		public enum State : byte
		{
			MUTE,
			UNMUTE
		}

		private State _state;

		public void OnOver() {
			_filling.enabled = true;
		}

		public void OnOut() {
			_filling.enabled = false;
		}

		public void OnClick ()
		{
			_state = _state == State.UNMUTE ? State.MUTE : State.UNMUTE;
			UpdateView();
		}

		public void SetState(State state) {

			_state = state;
			UpdateView ();
		}

		private void UpdateView() {

			switch (_state) {

			case State.MUTE:
				_buttonLabel.text = "ON";
				break;

			case State.UNMUTE:
				_buttonLabel.text = "OFF";
				break;
			}
		}
	}
}

