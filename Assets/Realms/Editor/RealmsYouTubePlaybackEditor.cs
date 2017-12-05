using UnityEditor;

// the pupropse of this script is to hide public fields from HighQualityPlayback class
[CustomEditor(typeof(PlaysnakRealms.RealmYouTubePlayback))]

public class RealmsYouTubePlaybackEditor : Editor {

	void OnEnable () {
	}

	public override void OnInspectorGUI() {
		EditorGUILayout.HelpBox ("All the public fields of HighQualityPlayback are hidden and will be set automatically", MessageType.Info);
	}
}