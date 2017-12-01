using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour {

	[SerializeField]
	protected Image bg;

	[SerializeField]
	protected Text buttonLabel;

	[SerializeField]
	protected bool active = false;

	[SerializeField]
	protected bool disabled = false;

    [SerializeField]
	protected bool toggle = true;

	void Awake () {
		Init ();
	}

	virtual protected void Init() {
		if (active) SetActiveState(true);
		if (disabled) DisableButton(true);
	}

	virtual public void SetActiveState(bool newActive)
	{
		if (!disabled)
		{
			active = newActive;
			if (newActive)
			{
				bg.color = OutrunGlobals.ColorScheme["mainColor"];
                if (buttonLabel != null)
                    buttonLabel.color = OutrunGlobals.ColorScheme["darkGrey"];
			}
			else
			{
				bg.color = OutrunGlobals.ColorScheme["darkGrey"];
                if(buttonLabel != null)
				    buttonLabel.color = OutrunGlobals.ColorScheme["secondaryColor"];
			}
		}
	}

	virtual public void DisableButton(bool shouldBeDisabled)
	{
		disabled = shouldBeDisabled;
		if (shouldBeDisabled)
		{
			bg.color = OutrunGlobals.ColorScheme["darkGrey"];
			buttonLabel.color = Color.black;
		}
		else
		{
			bg.color = OutrunGlobals.ColorScheme["darkGrey"];
			buttonLabel.color = OutrunGlobals.ColorScheme["secondaryColor"];
		}

	}

	public bool isDisabled {
		get {
			return disabled;
		}
	}

	virtual public void OnClick () {

		if(toggle)
			SetActiveState (true);
	}
    public void DeadmansSwitchOn()
    {
        SetActiveState(true);
    }
    public void DeadmansSwitchOff()
    {
        SetActiveState(false);
    }
}
