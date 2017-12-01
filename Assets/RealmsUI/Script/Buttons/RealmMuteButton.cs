using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {


	[RequireComponent (typeof(RealmMuteButtonUI))]
	public class RealmMuteButton : RealmButtonWithClickMeter {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		private RealmMuteButtonUI _ui;

		protected override void Init ()
		{
			base.Init ();

			if(_player != null)
				_player = gameObject.GetComponentInParent<RealmYouTubeVideoPlayer> ();

			Debug.Assert (_player != null, "Player is missing");

			if(_player != null)
				_player.OnMute += OnPlayerMute;

			_ui = gameObject.GetComponent<RealmMuteButtonUI> ();
		}

		void OnPlayerMute (bool isMuted) {

			_ui.SetState(isMuted ? RealmMuteButtonUI.State.MUTE : RealmMuteButtonUI.State.UNMUTE);
		}

		protected override void ProcessClick ()
		{
			base.ProcessClick ();
			_player.OnMuteUnmute ();
		}
	}
}