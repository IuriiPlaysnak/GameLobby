using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(RealmInteractiveItem))]
[RequireComponent (typeof(GazeClickMeter))]
[RequireComponent (typeof(ButtonWithClickMeterUI))]
public class RealmButtonWithClickMeter : MonoBehaviour {

	[SerializeField]
	public UnityEvent OnClick;

	private GazeClickMeter _clickMeter; 
	private ButtonWithClickMeterUI _buttonUI;
    private bool becomeEnabledOnOut = false;

    void Awake() {

		Init ();
	}

	virtual protected void Init() {

		_buttonUI = GetComponent<ButtonWithClickMeterUI>();

		RealmInteractiveItem gazeInteraction = gameObject.GetComponent<RealmInteractiveItem> ();
		if (gazeInteraction != null) {
			gazeInteraction.OnOut += OnOut;
			gazeInteraction.OnMoveOver += OnMoveOver;
		}

		_clickMeter = gameObject.GetComponent<GazeClickMeter> ();
		Debug.Assert (_clickMeter != null, this + ": Click meter is missing");

		_clickMeter.OnFull += OnMeterFull;
		_clickMeter.OnProgress += OnMeterProgress;
	}

	void OnMeterProgress(float progress) {

		_buttonUI.UpdateClickMeter (progress);
	}

	void OnMeterFull() {
		
		if (_buttonUI.isDisabled == false) {
			ProcessClick ();
		}
	}

	virtual protected void ProcessClick() {
		
		_buttonUI.OnClick ();
		OnClick.Invoke ();
	}

	void OnOut ()
	{
		_clickMeter.Release ();

        if(becomeEnabledOnOut)
        {
            becomeEnabledOnOut = false;
            _buttonUI.DisableButton(false);
        }
	}

	void OnMoveOver (RaycastHit hit)
	{
		if(_buttonUI.isDisabled == false)
			_clickMeter.UpdateProgress ();
	}

	void OnDestroy() {

		OnClick.RemoveAllListeners ();
	}

	void OnDisable() {
		_clickMeter.Reset ();
	}

    public void DisableWhileGazedAt()
    {
		if (!_buttonUI.isDisabled)
        {
			_buttonUI.DisableButton(true);
            becomeEnabledOnOut = true;
        }
    }
}