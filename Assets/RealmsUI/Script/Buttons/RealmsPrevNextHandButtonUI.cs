using UnityEngine;

namespace PlaysnakRealms
{
	public class RealmsPrevNextHandButtonUI : RealmsHandButtonUI
	{
		protected override void UpdateView ()
		{
			switch (_state) {

			case State.NEXT:
				_buttonLabel.text = ">";
				break;

			case State.PREV:
				_buttonLabel.text = "<";
				break;
			}
		}
	}
}

