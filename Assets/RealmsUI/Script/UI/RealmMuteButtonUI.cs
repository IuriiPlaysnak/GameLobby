using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaysnakRealms;

public class RealmMuteButtonUI : ButtonWithClickMeterUI {

	public enum State : byte
	{
		MUTE,
		UNMUTE
	}

	private State _state;

	override public void OnClick ()
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
			buttonLabel.text = "ON";
			break;

		case State.UNMUTE:
			buttonLabel.text = "OFF";
			break;
		}
	}
}
