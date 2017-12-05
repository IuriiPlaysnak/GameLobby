using System;
using System.Collections.Generic;

namespace PlaysnakRealms
{
	abstract public class RealmsAbstractDataSource
	{
		public event Action<RealmsContentProvider.SettingData> OnLoadingComplete;

		private int _itemsAreLoading;
		private RealmsContentProvider.GalleryData _gallery;

		protected RealmsContentProvider.GalleryData gallery {
			get {
				return _gallery;
			}
		}

		private RealmsContentProvider.VideosData _videos;

		protected RealmsContentProvider.VideosData videos {
			get {
				return _videos;
			}
		}

		abstract public void Load (string url);

		public RealmsAbstractDataSource() {

			_itemsAreLoading = 0;
			
			_videos = new RealmsContentProvider.VideosData ();
			_videos.playlists = new List<RealmsContentProvider.PlaylistData> ();

			_gallery = new RealmsContentProvider.GalleryData ();
			_gallery.images = new List<RealmsContentProvider.ImageData> ();
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

			RealmsContentProvider.SettingData result = new RealmsContentProvider.SettingData ();
			result.galleryData = _gallery;
			result.videosData = _videos;

			if (OnLoadingComplete != null)
				OnLoadingComplete (result);
		}
	}
}