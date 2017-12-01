using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	public class RealmPlayPauseButtonUI : ButtonWithClickMeterUI {

		public enum State : byte
		{
			PLAY,
			PAUSE
		}

		private State _state;

		override public void OnClick ()
		{
			_state = _state == State.PAUSE ? State.PLAY : State.PAUSE;
			UpdateView();
		}

		public void SetState(State state) {

			_state = state;
			UpdateView ();
		}

		private void UpdateView() {

			switch (_state) {

			case State.PLAY:
				buttonLabel.text = "❚❚";
				break;

			case State.PAUSE:
				buttonLabel.text = "►";
				break;
			}
		}
	}
}