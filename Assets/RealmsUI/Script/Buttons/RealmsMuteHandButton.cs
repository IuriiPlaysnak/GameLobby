using UnityEngine;

namespace PlaysnakRealms
{
	[RequireComponent (typeof(RealmsMuteHandButtonUI))]
	[RequireComponent (typeof(RealmsHandButton))]
	public class RealmsMuteHandButton : MonoBehaviour {

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		private RealmsMuteHandButtonUI _ui;
		private RealmsHandButton _buttonHandler;

		void Awake() {

			_ui = gameObject.GetComponent<RealmsMuteHandButtonUI> ();
			_buttonHandler = gameObject.GetComponent<RealmsHandButton> ();
			_buttonHandler.OnClick += OnButtonClick;
			_buttonHandler.OnOver += OnButtonOver;
			_buttonHandler.OnOut += OnButtonOut;

			_player.OnMute += OnPlayerMute;
		}

		void OnButtonOut ()
		{
			_ui.OnOut ();
		}

		void OnButtonOver ()
		{
			_ui.OnOver ();
		}

		void OnButtonClick ()
		{
			_ui.OnClick ();
			_player.OnMuteUnmute ();
		}

		void OnPlayerMute (bool isMuted) {

			_ui.SetState(isMuted ? RealmsMuteHandButtonUI.State.MUTE : RealmsMuteHandButtonUI.State.UNMUTE);
		}
	}
}