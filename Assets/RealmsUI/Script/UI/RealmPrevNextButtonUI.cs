using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	public class RealmPrevNextButtonUI : ButtonWithCyclingClickMeterUI {

		public void UpdateView(RealmPrevNextButton.Type type ) {

			switch(type) {

			case RealmPrevNextButton.Type.NEXT:
				buttonLabel.text = ">";
				break;

			case RealmPrevNextButton.Type.PREV:
				buttonLabel.text = "<";
				break;
			}
		}
	}
}