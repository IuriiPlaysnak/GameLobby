using System;
using System.Collections.Generic;

namespace PlaysnakRealms
{
	abstract public class AbstractDataSource
	{
		public event Action<OutrunRealmDataProvider.SettingData> OnLoadingComplete;

		private int _itemsAreLoading;
		private OutrunRealmDataProvider.GalleryData _gallery;

		protected OutrunRealmDataProvider.GalleryData gallery {
			get {
				return _gallery;
			}
		}

		private OutrunRealmDataProvider.VideosData _videos;

		protected OutrunRealmDataProvider.VideosData videos {
			get {
				return _videos;
			}
		}

		abstract public void Load (string url);

		public AbstractDataSource() {

			_itemsAreLoading = 0;
			
			_videos = new OutrunRealmDataProvider.VideosData ();
			_videos.playlists = new List<OutrunRealmDataProvider.PlaylistData> ();

			_gallery = new OutrunRealmDataProvider.GalleryData ();
			_gallery.images = new List<OutrunRealmDataProvider.ImageData> ();
		}

		protected void OnContentItemLoadingStart() {
			_itemsAreLoading++;
		}

		protected void OnContentItemLoadingComplete() {

			_itemsAreLoading--;

			if (_itemsAreLoading == 0)
				ContentIsLoaded ();
		}

		private void ContentIsLoaded() {

			OutrunRealmDataProvider.SettingData result = new OutrunRealmDataProvider.SettingData ();
			result.galleryData = _gallery;
			result.videosData = _videos;

			if (OnLoadingComplete != null)
				OnLoadingComplete (result);
		}
	}
}