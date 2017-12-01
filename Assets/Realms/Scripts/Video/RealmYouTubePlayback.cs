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
		private string audioVideoUrl;
		private bool videoIsReadyToPlay = false;

		private VideoPlayer _videoPlayer;
		private VideoPlayer _audioVideoPlayer;
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

		private bool audioDecryptDone = false;
		private bool videoDecryptDone = false;

		void FinishLoadingUrls()
		{
			List<VideoInfo> videoInfos = _requestResolver.videoInfos;
			videoDecryptDone = false;
			audioDecryptDone = false;

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
						//The string is the video url with audio
						StartCoroutine(_requestResolver.DecryptDownloadUrl (DecryptAudioDone, info));
					} else {
						audioVideoUrl = info.DownloadUrl;
					}
					break;
				}
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

		private void DecryptAudioDone(string url)
		{
			audioVideoUrl = url;
			audioDecryptDone = true;

			if (videoDecryptDone)
				videoIsReadyToPlay = true;
		}

		private void DecryptVideoDone(string url)
		{
			videoUrl = url;
			videoDecryptDone = true;

			if (audioDecryptDone)
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

				if (_audioVideoPlayer == null) {
					_audioVideoPlayer = gameObject.AddComponent<VideoPlayer> ();
					_audioVideoPlayer.playOnAwake = false;
				}

				_audioVideoPlayer.source = VideoSource.Url;
				_audioVideoPlayer.url = audioVideoUrl;
				_audioVideoPlayer.Prepare ();
			}

			if (checkIfVideoArePrepared) {
				checkIfVideoArePrepared = false;
				videoPrepared = false;
				_videoPlayer.prepareCompleted += VideoPrepared;
				audioPrepared = false;
				_audioVideoPlayer.prepareCompleted += AudioPrepared;
			}
		}

		private bool videoPrepared;
		private bool audioPrepared;

		void AudioPrepared(VideoPlayer vPlayer){
			_audioVideoPlayer.prepareCompleted -= AudioPrepared;
			audioPrepared = true;
			if (audioPrepared && videoPrepared)
				Play ();
		}

		void VideoPrepared(VideoPlayer vPlayer){
			_videoPlayer.prepareCompleted -= VideoPrepared;
			videoPrepared = true;
			if (audioPrepared && videoPrepared)
				Play ();
		}

		private void Play(){
			_videoPlayer.loopPointReached += PlaybackDone;
			StartCoroutine(WaitAndPlay());
		}

		private void PlaybackDone(VideoPlayer vPlayer){
			_videoPlayer.loopPointReached -= PlaybackDone;
			OnVideoFinished ();
		}

		IEnumerator WaitAndPlay()
		{
			_audioVideoPlayer.Play();
			yield return new WaitForSeconds(0.35f);

			_videoPlayer.Play();
			if (this.GetComponent<VideoController>() != null)
			{
				this.GetComponent<VideoController>().HideLoading();
			}
		}


		public event System.Action<VideoPlayer> OnVideoComplete;
		private void OnVideoFinished(){
			if (_videoPlayer.isPrepared) {
				Debug.Log ("Finished");
				if (_videoPlayer.isLooping)
				{
					_videoPlayer.time = 0;
					_videoPlayer.frame = 0;
					_audioVideoPlayer.time = 0;
					_audioVideoPlayer.frame = 0;
					_videoPlayer.Play();
					_audioVideoPlayer.Play();
				}
			}

			if (OnVideoComplete != null)
				OnVideoComplete (_videoPlayer);
		}

		public void Pause() {

			_videoPlayer.Pause ();
			_audioVideoPlayer.Pause ();
		}

		public void Resume() {

			_videoPlayer.Play ();
			_audioVideoPlayer.Play ();
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