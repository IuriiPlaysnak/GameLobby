using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaysnakRealms;

public class RealmVideoCard : MonoBehaviour {

	[SerializeField]
	private RealmYouTubePlaylist _playlist;

	[SerializeField]
	private RealmYouTubeVideoPlayer _player;

	void Awake() {

		Debug.Assert (_playlist != null, "Playlist is missing");
//		Debug.Assert (_player != null, "Video Player is missing");
	}

	void Start () {

		StartCoroutine (Init ());
	}

	private IEnumerator Init() {

		while (OutrunRealmDataProvider.isLoadingComlete == false) {
			yield return null;
		}

		_playlist.LoadPlaylist (OutrunRealmDataProvider.videosData.playlists [0].url);
	}
}