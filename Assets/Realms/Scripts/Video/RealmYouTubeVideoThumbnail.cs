﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(RawImage))]
[RequireComponent (typeof(RealmInteractiveItem))]
public class RealmYouTubeVideoThumbnail : MonoBehaviour {

	public event System.Action<RealmYouTubeVideoThumbnail.Data> OnClick;

	private RawImage _display;
	private RealmYouTubeVideoThumbnail.Data _data;

	void Awake() {

		_display = gameObject.GetComponent<RawImage> ();
		Debug.Assert (_display != null, "Display image is missing");

		RealmInteractiveItem ii = gameObject.GetComponent<RealmInteractiveItem> ();

		if (ii != null) {
			ii.OnClick += 
				() => { 
				if (OnClick != null)
					OnClick (_data); 
			};
		}
	}

	public void SetData(string videoId, int indexInPlaylist, YoutubeTumbnails thumbnails ) {

		_data = new Data () { 
			videoId = videoId,
			videoIndexInPlaylist = indexInPlaylist,
			thumnails = thumbnails
		};

		Load (thumbnails.defaultThumbnail.url);
	}

	private void Load(string url) {

		ResourceManager.LoadImage (url, OnImageLoaded);
	}

	private void OnImageLoaded(Texture2D texture) {
		_display.texture = texture;
	}

	public struct Data {

		public int videoIndexInPlaylist;
		public string videoId;
		public YoutubeTumbnails thumnails;
	}
}