using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubeLight;
using SimpleJSON;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RequestResolver))]
	[RequireComponent (typeof(VideoPlayer))]

	public class RealmYouTubePlayback : MonoBehaviour {

		private string videoUrl;
		private bool videoIsReadyToPlay = false;

		private VideoPlayer _videoPlayer;
		private RequestResolver _requestResolver;

		void Awake() {

			_requestResolver = gameObject.GetComponent<RequestResolver>();

			_videoPlayer = gameObject.GetComponent<VideoPlayer> ();
			_videoPlayer.playOnAwake = false;
		}

		public void PlayYoutubeVideo(string videoId)
		{
			if(this.GetComponent<VideoController>() != null)
			{
				this.GetComponent<VideoController>().ShowLoading("Loading...");
			}

			StartCoroutine(_requestResolver.GetDownloadUrls(FinishLoadingUrls, videoId, false));
		}

		void FinishLoadingUrls()
		{
			List<VideoInfo> videoInfos = _requestResolver.videoInfos;

			int maxQuality = 0;
			foreach (VideoInfo info in videoInfos) 
			{
				if (info.VideoType == VideoType.Mp4)
					maxQuality = Mathf.Max (maxQuality, info.Resolution);
			}

			foreach (VideoInfo info in videoInfos) 
			{
				if (info.VideoType == VideoType.Mp4 && info.Resolution == maxQuality) {
					if (info.RequiresDecryption) {
						StartCoroutine(_requestResolver.DecryptDownloadUrl(DecryptVideoDone, info));
					} else {
						videoUrl = info.DownloadUrl;
						videoIsReadyToPlay = true;
					}
					break;
				}
			}
		}

		private void DecryptVideoDone(string url)
		{
			videoUrl = url;
			videoIsReadyToPlay = true;
		}

		bool checkIfVideoArePrepared = false;

		void FixedUpdate(){
			//used this to play in main thread.
			if (videoIsReadyToPlay) {
				videoIsReadyToPlay = false;
				//play using the old method
				Debug.Log ("Play!!" + videoUrl);
				_videoPlayer.source = VideoSource.Url;
				_videoPlayer.url = videoUrl;
				checkIfVideoArePrepared = true;
				_videoPlayer.Prepare ();
			}

			if (checkIfVideoArePrepared) {
				checkIfVideoArePrepared = false;
				_videoPlayer.prepareCompleted += VideoPrepared;
			}
		}

		void VideoPrepared(VideoPlayer vPlayer){
			_videoPlayer.prepareCompleted -= VideoPrepared;
			Play ();
		}

		private void Play(){
			_videoPlayer.loopPointReached += PlaybackDone;
			_videoPlayer.Play();
		}

		private void PlaybackDone(VideoPlayer vPlayer){
			_videoPlayer.loopPointReached -= PlaybackDone;
			OnVideoFinished ();
		}

		public event System.Action<VideoPlayer> OnVideoComplete;
		private void OnVideoFinished() {
			
			if (_videoPlayer.isPrepared) {
				if (_videoPlayer.isLooping)
				{
					_videoPlayer.time = 0;
					_videoPlayer.frame = 0;
					_videoPlayer.Play();
				}
			}

			if (OnVideoComplete != null)
				OnVideoComplete (_videoPlayer);
		}

		public void Pause() {

			_videoPlayer.Pause ();
		}

		public void Resume() {

			_videoPlayer.Play ();
		}

		public bool isPlaying {

			get { return _videoPlayer.isPlaying; }
		}

		void OnDestroy() {

			OnVideoComplete = null;
			_videoPlayer.targetTexture.Release ();
		}
	}
}