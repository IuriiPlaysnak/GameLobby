using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

static public class OutrunAnalytics  {

	static public string VIDEO_EVENT = "videoPlayer";
//	static public string GALLERY_EVENT = "imageGallery";
	static public string SCENE_EVENT = "scene";
	static public string ENDLESS_RUNNER_STATS_EVENT = "endlessRunnerStats";

	static public void TrackEvent(string eventName, Dictionary<string, object> data) {

		Analytics.CustomEvent(eventName, data);

//		if (eventName == ENDLESS_RUNNER_STATS_EVENT) {
//			foreach (var item in data) {
//
//				UnityEngine.Debug.Log (item.Key + ": " + item.Value);
//			}
//		}
	}
}
