using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealmToolbarWithStaticButtons : MonoBehaviour {

	[SerializeField]
	private RectTransform _buttonPlaceholder;

	[SerializeField]
	private List<RealmToolbarButton> _buttons;

	void Awake() {

		RealmsInteractiveItem interaction = gameObject.GetComponent<RealmsInteractiveItem> ();

		if (interaction != null) {
			interaction.OnOver += OnOver;
			interaction.OnOut += OnOut;
		}
	}

	void Start() {
		foreach (var button in _buttons) {
			button.Hide (false);
		}
	}

	void OnOver() {

		foreach (var button in _buttons) {
			button.ChangeParent (_buttonPlaceholder);
		}
	}

	void OnOut() {

		foreach (var button in _buttons) {
			button.ReleaseFromParent ();
		}
	}
}