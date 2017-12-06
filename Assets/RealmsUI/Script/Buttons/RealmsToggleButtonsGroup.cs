using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	public class RealmsToggleButtonsGroup : MonoBehaviour {

		private List<RealmsToggleHandButton> _buttons;

		public void OnButtonClick(RealmsToggleHandButton clickedButton) {

			if (_buttons == null || _buttons.Count == 0)
				return;

			foreach (var button in _buttons) {

				if (button != clickedButton)
					button.Reset ();
			}
		}

		public void AddButton(RealmsToggleHandButton button) {

			if (_buttons == null)
				_buttons = new List<RealmsToggleHandButton> ();

			_buttons.Add (button);
		}
	}
}