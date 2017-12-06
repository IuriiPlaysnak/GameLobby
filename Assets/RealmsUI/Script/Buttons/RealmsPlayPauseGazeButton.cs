using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RealmButtonWithClickMeter))]
	[RequireComponent (typeof(RealmPlayPauseButtonUI))]

	public class RealmsPlayPauseGazeButton : MonoBehaviour {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		private RealmPlayPauseButtonUI _ui;
		private RealmButtonWithClickMeter _buttonHandler;

		void Awake() {
			
			if(_player != null)
				_player = gameObject.GetComponentInParent<RealmYouTubeVideoPlayer> ();

			Debug.Assert (_player != null, "Player is missing");

			_player.OnPlay += OnVideoPlay;

			_ui = gameObject.GetComponent<RealmPlayPauseButtonUI> ();
			_buttonHandler = gameObject.GetComponent<RealmButtonWithClickMeter> ();

			_buttonHandler.OnClick.AddListener(OnClick);
		}

		void OnVideoPlay ()
		{
			_ui.SetState (RealmPlayPauseButtonUI.State.PLAY);
		}

		private void OnClick ()
		{
			_player.OnPlayPause ();
		}
	}
}