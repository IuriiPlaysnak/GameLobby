using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithClickMeterUI : ButtonUI {

	[SerializeField]
	protected Image gauge;

	protected override void Init ()
	{
		base.Init ();
		gauge.fillAmount = 0f;
	}

	virtual public void UpdateClickMeter (float progress) {

		if (active || disabled)
			return;

		gauge.fillAmount = progress;
	}

	public override void DisableButton (bool shouldBeDisabled)
	{
		base.DisableButton (shouldBeDisabled);
		if (!shouldBeDisabled)
			gauge.fillAmount = 0f;
	}

	public override void SetActiveState (bool newActive)
	{
		base.SetActiveState (newActive);
		if (newActive)
			gauge.fillAmount = 0f;
	}
}