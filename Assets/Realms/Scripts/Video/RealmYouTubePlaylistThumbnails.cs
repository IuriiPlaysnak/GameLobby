using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	public class RealmYouTubePlaylistThumbnails : MonoBehaviour {

		[SerializeField]
		private RealmYouTubePlaylist _playlist;

		[SerializeField]
		private List<RealmYouTubeVideoThumbnail> _thumbnails;

		[SerializeField]
		private GameObject _darkOverlay;

		void Awake() {

			Debug.Assert (_playlist != null, "Playlist is missing");
		}

		// Use this for initialization
		void Start () {

			_playlist.OnPlayVideo += _playlist_OnNewVideoPlay;

			_playlist.player.OnComplete += OnVideoComplete;
			_playlist.player.OnPause += OnVideoPause;
			_playlist.player.OnPlay += OnVideoPlay;

			foreach (var thumbnail in _thumbnails) {
				thumbnail.OnClick += OnThumbnailClick;
			}

			SetThumbnailsVisibility (false);
		}


		void OnThumbnailClick (RealmYouTubeVideoThumbnail.Data data)
		{
			_playlist.PlayVideo (data.videoIndexInPlaylist);
		}

		void _playlist_OnNewVideoPlay (int obj)
		{
			LoadThumbnailsForVideo (obj);
		}
		
		private void LoadThumbnailsForVideo(int videoListItemIndex) {

			int nextItemIndex = videoListItemIndex + 1;

			for (int i = 0; i < _thumbnails.Count; i++) {

				if (nextItemIndex > _playlist.playlistItems.Length - 1)
					nextItemIndex = 0;

				YoutubePlaylistItems item = _playlist.playlistItems [nextItemIndex];

				_thumbnails [i].SetData (item.videoId, nextItemIndex, item.snippet.thumbnails);

				nextItemIndex++;
			}
		}

		private void SetThumbnailsVisibility(bool isVisible) {

			foreach (var thumbnail in _thumbnails) {
				thumbnail.gameObject.SetActive (isVisible);
			}

			if(_darkOverlay != null)
				_darkOverlay.SetActive(isVisible);
		}

		void OnVideoComplete ()
		{
			SetThumbnailsVisibility (true);
		}

		void OnVideoPlay ()
		{
			SetThumbnailsVisibility (false);
		}

		void OnVideoPause ()
		{
			SetThumbnailsVisibility (true);
		}
	}
}
