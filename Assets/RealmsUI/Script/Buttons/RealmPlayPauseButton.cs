using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RealmPlayPauseButtonUI))]
	public class RealmPlayPauseButton : RealmButtonWithClickMeter {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		private RealmPlayPauseButtonUI _ui;

		protected override void Init ()
		{
			base.Init ();

			if(_player != null)
				_player = gameObject.GetComponentInParent<RealmYouTubeVideoPlayer> ();

			Debug.Assert (_player != null, "Player is missing");

			_player.OnPlay += OnVideoPlay;

			_ui = gameObject.GetComponent<RealmPlayPauseButtonUI> ();
		}

		void OnVideoPlay ()
		{
			_ui.SetState (RealmPlayPauseButtonUI.State.PLAY);
		}

		protected override void ProcessClick ()
		{
			base.ProcessClick ();
			_player.OnPlayPause ();
		}
	}
}