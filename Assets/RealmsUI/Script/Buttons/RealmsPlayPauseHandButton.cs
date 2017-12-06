using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	public class RealmsPlayPauseHandButton : MonoBehaviour {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		private RealmsPlayPauseHandButtonUI _ui;
		private RealmsHandButton _buttonHandler;

		void Awake() {

			_ui = gameObject.GetComponent<RealmsPlayPauseHandButtonUI> ();
			_buttonHandler = gameObject.GetComponent<RealmsHandButton> ();
			_buttonHandler.OnClick += _buttonHandler_OnClick;
			_buttonHandler.OnOver += _buttonHandler_OnOver;
			_buttonHandler.OnOut += _buttonHandler_OnOut;

			_player.OnPlay += OnVideoPlay;
		}

		void _buttonHandler_OnOut ()
		{
			_ui.OnOut ();
		}

		void _buttonHandler_OnOver ()
		{
			_ui.OnOver ();
		}

		void _buttonHandler_OnClick ()
		{
			_ui.OnClick ();
			_player.OnPlayPause ();
		}

		void OnVideoPlay ()
		{
			_ui.SetState (RealmsPlayPauseHandButtonUI.State.PLAY);
		}
	}
}