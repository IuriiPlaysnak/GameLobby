using UnityEngine;
using UnityEngine.UI;

namespace PlaysnakRealms
{
	public class RealmsMuteHandButtonUI : RealmsHandButtonUI
	{
		override public void OnClick ()
		{
			_state = _state == State.UNMUTE ? State.MUTE : State.UNMUTE;
			UpdateView ();
		}

		override protected void UpdateView() {

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

