using UnityEngine;

namespace PlaysnakRealms
{
	[RequireComponent (typeof(RealmsMuteHandButtonUI))]

	public class RealmsMuteHandButton : RealmsHandButton {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		protected override void Init ()
		{
			base.Init ();

			_ui = gameObject.GetComponent<RealmsMuteHandButtonUI> ();
			_player.OnMute += OnPlayerMute;
		}

		protected override void OnInteractionClick ()
		{
			base.OnInteractionClick ();
			_player.OnMuteUnmute ();
		}

		void OnPlayerMute (bool isMuted) {

			_ui.SetState(isMuted ? RealmsMuteHandButtonUI.State.MUTE : RealmsMuteHandButtonUI.State.UNMUTE);
		}
	}
}