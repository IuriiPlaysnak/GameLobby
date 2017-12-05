using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubeLight;
using SimpleJSON;

namespace PlaysnakRealms {

	[RequireComponent (typeof(VideoPlayer))]
	[RequireComponent (typeof(AudioSource))]

	public class RealmYouTubePlayback : HighQualityPlayback {

		void Awake() {

			useNewUnityPlayer = true;
			noHD = false;
			playOnStart = false;

			unityVideoPlayer = gameObject.GetComponent<VideoPlayer> ();

			audioVplayer = gameObject.AddComponent<VideoPlayer> ();

			audioVplayer.source = VideoSource.Url;
			audioVplayer.playOnAwake = false;
			audioVplayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
			audioVplayer.controlledAudioTrackCount = 1;
			audioVplayer.SetTargetAudioSource (0, gameObject.GetComponent<AudioSource> ());
		}

		public event UnityEngine.Video.VideoPlayer.EventHandler OnVideoComplete {
			add {
				unityVideoPlayer.loopPointReached += value;
			}
			remove{
				unityVideoPlayer.loopPointReached -= value;
			}
		}

		public bool isPlaying {
			get { return unityVideoPlayer.isPlaying; } 
		}

		public void Pause() {

			unityVideoPlayer.Pause ();
			audioVplayer.Pause ();
		}

		public void Resume() {
			unityVideoPlayer.Play ();
			audioVplayer.Play ();
		}

		void OnDestroy() {

			unityVideoPlayer.targetTexture.Release ();
		}
	}
}