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

		if (RealmsContentProvider.videosData.isEmpty) {
			
			Debug.Log ("There is no playlist to play");
		
		} else {

			RealmsContentProvider.PlaylistData playlistToPlay;
			RealmsContentProvider.VideosData videosData = RealmsContentProvider.videosData;

			if (RealmsPersistenceData.doShowNewPlayerContent) {

				playlistToPlay = 
					videosData.GetPlaylistForANewPlayer () ?? videosData.GetPlaylistForARegularPlayer ();

				RealmsPersistenceData.doShowNewPlayerContent = false;
			} else {

				playlistToPlay = 
					videosData.GetPlaylistForARegularPlayer () ?? videosData.GetPlaylistForANewPlayer ();
			}

			_playlist.LoadPlaylist (playlistToPlay.url);
		}
	}
}