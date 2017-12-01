using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace PlaysnakRealms {

	[RequireComponent (typeof(YoutubeAPIManager))]
	public class RealmYouTubePlaylist : MonoBehaviour {

		private const float DEFAULT_AUTOPLAY_DELAY = 3f;

		[SerializeField]
		private string _playlistUrl;

		[SerializeField]
		private int _maxNumberOfItemsInPlaylist = 10;

		[SerializeField]
		private RealmYouTubeVideoPlayer _player;

		[SerializeField]
		private bool _playOnAwake;

		private YoutubeAPIManager _youtubeManager;
		private YoutubePlaylistItems[] _playlistItems;
		private RealmAutoplayController _autoplay;
		private int _currentVideoIndex;


		public RealmYouTubeVideoPlayer player {
			get {
				return _player;
			}
		}

		public YoutubePlaylistItems[] playlistItems {
			get {
				return _playlistItems;
			}
		}

		void Awake() {

			Debug.Assert (_player != null, "Player is missing");

			_youtubeManager = gameObject.GetComponent<YoutubeAPIManager> ();

			_autoplay = gameObject.GetComponent<RealmAutoplayController> ();
			if (_autoplay == null) {

				_autoplay = gameObject.AddComponent<RealmAutoplayController> ();
				_autoplay.delay = DEFAULT_AUTOPLAY_DELAY;
			}
		}

		void Start () {

			_autoplay.Stop ();
			AfterDataLoadedInit = InitInteractions;

			if (_playlistUrl != null && _playlistUrl != "")
				LoadPlaylist (_playlistUrl);
		}

		public void LoadPlaylist(string url) {
			
			_youtubeManager.GetPlaylistItems (
				YouTubeUtils.GetPlaylistIdFromUrl (url)
				, _maxNumberOfItemsInPlaylist
				, OnListDataLoaded
			);
		}

		private System.Action AfterDataLoadedInit;
		private void OnListDataLoaded(YoutubePlaylistItems[] playlistItems) {

			if (AfterDataLoadedInit != null)
				AfterDataLoadedInit ();

			_playlistItems = playlistItems;

			if (_playOnAwake) {
				
				_player.Mute (true);
				PlayVideo (0);

			} else {

				_player.OnPlay += OnPlayerPlay;
			}
		}

		void OnPlayerPlay() {

			if(_playlistItems != null && _playlistItems.Count() > 0) {

				_player.OnPlay -= OnPlayerPlay;
				PlayVideo (0);
			}
		}

		private void PlayNextVideo() {
			_currentVideoIndex++;
			if (_currentVideoIndex > _playlistItems.Count () - 1)
				_currentVideoIndex = 0;
			PlayVideo (_currentVideoIndex);
		}

		public event System.Action<int> OnPlayVideo;

		public void PlayVideo(int videoListItemIndex) {

			_currentVideoIndex = videoListItemIndex;
			_player.PlayVideo (_playlistItems [_currentVideoIndex].videoId);

			if (OnPlayVideo != null)
				OnPlayVideo (videoListItemIndex);
		}

		private void InitInteractions() {

			AfterDataLoadedInit = null;

			_player.OnComplete += OnVideoComplete;
			_player.OnPlay += OnVideoPlay;

			_autoplay.OnComplete += OnAutoplayComplete;
		}

		void OnAutoplayComplete ()
		{
			PlayNextVideo ();
		}

		void OnVideoComplete ()
		{
			_autoplay.Start ();
		}

		void OnVideoPlay ()
		{
			_autoplay.Stop ();
		}
	}
}