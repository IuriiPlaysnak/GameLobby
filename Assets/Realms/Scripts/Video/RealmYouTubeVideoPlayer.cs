using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(YoutubeAPIManager))]
	[RequireComponent (typeof(RealmYouTubePlayback))]
	[RequireComponent (typeof(AudioSource))]
	public class RealmYouTubeVideoPlayer : MonoBehaviour {

		[SerializeField]
		private string _videoURL;

		public event System.Action OnPlay;
		public event System.Action OnPause;
		public event System.Action OnComplete;
		public event System.Action<bool> OnMute;
		public event System.Action<YoutubeData> OnVideoData;

		private RealmYouTubePlayback _playback;
		private YoutubeAPIManager _youtubeManager;
		private RealmInteractiveItem _interaction;
		private RealmPlayPauseButtonUI _playButtonUI;
		private AudioSource _playerAudio;

		void Awake() {

			_interaction = gameObject.GetComponent<RealmInteractiveItem> ();
			_playback = gameObject.GetComponentInChildren<RealmYouTubePlayback> ();
			_youtubeManager = gameObject.GetComponent<YoutubeAPIManager> ();
			_playButtonUI = gameObject.GetComponentInChildren<RealmPlayPauseButtonUI> ();		

			_playerAudio = _playback.GetComponentInChildren<AudioSource> ();
			_playerAudio.playOnAwake = false;
		}

		void Start () {

			_playback.OnVideoComplete += OnVideoComplete;

			if(_videoURL != null && _videoURL != "") 
				PlayVideo (YouTubeUtils.GetVideoIdFromUrl (_videoURL));
		}

		public void OnPlayPause ()
		{
			if (_playback.isPlaying) {

				OutrunAnalytics.TrackEvent (OutrunAnalytics.VIDEO_EVENT, new Dictionary<string, object> () {{ "state", "pause" }} );

				_playback.Pause ();
				if (OnPause != null)
					OnPause ();
				
			} else {

				OutrunAnalytics.TrackEvent (OutrunAnalytics.VIDEO_EVENT, new Dictionary<string, object> () {{ "state", "play" }} );

				_playback.Resume ();
				if (OnPlay != null)
					OnPlay ();
			}
		}

		public void OnMuteUnmute(){

			Mute (!_playerAudio.mute);
		}

		public void Mute(bool isMute) {

			_playerAudio.mute = isMute;

			if (OnMute != null)
				OnMute (isMute);
		}

		public bool isMuted {
			get { return _playerAudio.mute; }
		}

		public void PlayVideo(string videoId) {

			if (OnPlay != null)
				OnPlay ();

			_playback.PlayYoutubeVideo (videoId);
			_youtubeManager.GetVideoData (videoId, OnVideoDataLoaded);
		}

		private void OnVideoDataLoaded(YoutubeData data) {

			if (OnVideoData != null)
				OnVideoData (data);
		}

		void OnVideoComplete (UnityEngine.Video.VideoPlayer source)
		{
			if (OnComplete != null)
				OnComplete ();
		}

		void OnDestroy() {

			OnPlay = null;
			OnPause = null;
			OnComplete = null;
		}
	}
}