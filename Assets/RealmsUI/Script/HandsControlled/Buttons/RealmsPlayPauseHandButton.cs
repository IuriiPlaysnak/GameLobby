using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RealmsPlayPauseHandButtonUI))]

	public class RealmsPlayPauseHandButton : RealmsHandButton {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		override protected void OnAwake(){

			base.OnAwake ();

			_ui = gameObject.GetComponent<RealmsPlayPauseHandButtonUI> ();
			_player.OnPlay += OnVideoPlay;
		}

		protected override void OnInteractionClick ()
		{
			base.OnInteractionClick ();
			_player.OnPlayPause ();
		}

		void OnVideoPlay ()
		{
			_ui.SetState (RealmsPlayPauseHandButtonUI.State.PLAY);
		}
	}
}