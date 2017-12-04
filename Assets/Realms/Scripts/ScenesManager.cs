using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour {

	private const float FADE_OUT_DURATION = 0.5f;
	private const float FADE_IN_DURATION = 0.5f;

	public enum SceneID
	{
		LOBBY = 0,
		GAME
	}

	static private ScenesManager _instance = null;
	static private GameObject _blackScreen = null;

	[SerializeField]
	private GameObject _blackScreenPrefab = null;

	void Awake() {

		if (_instance == null) {

			_instance = this;
			_enterSceneTimestamp = Time.time;
			DontDestroyOnLoad (this.gameObject);

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnNextSceneLoaded;

		} else {

			Destroy (this.gameObject);
		}
	}

	static private SceneID _nextSceneId;
	static public void LoadScene(SceneID sceneId) {

		if (_instance == null) {
			Debug.LogWarning ("ScenesManager doesn't have a game object");
			return;
		}

		_nextSceneId = sceneId;
		_instance.AttachBlackScreenToMainCamera ();
		_instance.StartCoroutine (_instance.FadeOut (_instance.LoadNextScene));
	}

	static public void QuitGame() {

		if (_instance == null) {
			Debug.LogWarning ("ScenesManager doesn't have a game object");
			return;
		}

		_instance.StartCoroutine (_instance.FadeOut(_instance.CloseApplication));
	}

	private IEnumerator FadeOut(System.Action onComplete) {

		AttachBlackScreenToMainCamera ();

		float startTimestamp = Time.time;
		_blackScreen.SetActive (true);
		_blackScreen.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 0, 0);

		while (Time.time - startTimestamp < FADE_OUT_DURATION) {
			_blackScreen.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 0, (Time.time - startTimestamp) / FADE_OUT_DURATION);
			yield return null;
		}

		_blackScreen.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 0, 1);

		if (onComplete != null)
			onComplete ();
	}

	private void CloseApplication() {
		Application.Quit ();
	}

	static private float _enterSceneTimestamp;
	private void LoadNextScene() {

		TrackSceneEvent ();
		UnityEngine.SceneManagement.SceneManager.LoadScene((int)_nextSceneId, UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

	void OnNextSceneLoaded (UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		_enterSceneTimestamp = Time.time;
		AttachBlackScreenToMainCamera ();
		StartCoroutine (FadeIn ());
	}

	private IEnumerator FadeIn() {

		float startTimestamp = Time.time;
		_blackScreen.SetActive (true);
		_blackScreen.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 0, 1);

		while (Time.time - startTimestamp <= FADE_IN_DURATION) {
			_blackScreen.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 0, 1f - (Time.time - startTimestamp) / FADE_IN_DURATION);
			yield return null;
		}

		_blackScreen.SetActive (false);
	}

	private void AttachBlackScreenToMainCamera() {

		if (_blackScreen == null)
			_blackScreen = GameObject.Instantiate (_instance._blackScreenPrefab) as GameObject;

		_blackScreen.transform.SetParent (Camera.main.gameObject.transform);
		_blackScreen.transform.localPosition = new Vector3 (0f, 0f, 0.3f);
	}

	void OnDestroy() {
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnNextSceneLoaded;
	}

	void OnApplicationQuit() {

		TrackSceneEvent ();
	}

	private void TrackSceneEvent() {

		OutrunAnalytics.TrackEvent (
			OutrunAnalytics.SCENE_EVENT, 
			new Dictionary<string, object> () 
			{
				{ "name", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name }
				, {"time", Time.time - _enterSceneTimestamp}
			} 
		);
	}
}
