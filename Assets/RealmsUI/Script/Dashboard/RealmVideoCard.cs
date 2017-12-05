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

		if (RealmsContentProvider.videosData.playlists != null && RealmsContentProvider.videosData.playlists.Count > 0)
			_playlist.LoadPlaylist (RealmsContentProvider.videosData.playlists [0].url);
		else
			Debug.Log ("Playlist is empty");
	}
}