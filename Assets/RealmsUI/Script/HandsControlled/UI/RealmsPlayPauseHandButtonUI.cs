﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlaysnakRealms
{
	public class RealmsPlayPauseHandButtonUI : RealmsSimpleHandButtonUI
	{
		override public void OnClick ()
		{
			base.OnClick ();
			_state = _state == State.PAUSE ? State.PLAY : State.PAUSE;
			UpdateView ();
		}

		override protected void UpdateView() {

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

