using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaysnakRealms;

public class RealmVideoCard : MonoBehaviour {

	[SerializeField]
	private RealmYouTubePlaylist _playlist;

	void Awake() {

		Debug.Assert (_playlist != null, "Playlist is missing");
	}

	void Start () {

		StartCoroutine (Init ());
	}

	private IEnumerator Init() {

		while (RealmsContentProvider.isLoadingComlete == false) {
			yield return null;
		}

		if (RealmsContentProvider.videosData.playlists.Count > 0) {

			int playlistIndex;

			if (RealmsPersistenceData.doShowNewPlayerContent) {
				
				RealmsPersistenceData.doShowNewPlayerContent = false;
				playlistIndex = 0;

			} else {

				playlistIndex = RealmsContentProvider.videosData.playlists.Count > 1 ? 1 : 0;
			}

			_playlist.LoadPlaylist (RealmsContentProvider.videosData.playlists [playlistIndex].url);
			
		} else {
			Debug.Log ("Playlist is empty");
		}
	}
}