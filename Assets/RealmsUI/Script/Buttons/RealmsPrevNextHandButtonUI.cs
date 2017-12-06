using UnityEngine;

namespace PlaysnakRealms
{
	public class RealmsPrevNextHandButtonUI : RealmsSimpleHandButtonUI
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

