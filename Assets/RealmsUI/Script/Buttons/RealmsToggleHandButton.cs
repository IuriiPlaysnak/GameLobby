using UnityEngine;

namespace PlaysnakRealms
{
	public class RealmsToggleHandButton : RealmsHandButton
	{
		[SerializeField]
		protected bool _isToggledOnStart;

		[SerializeField]
		private RealmsToggleButtonsGroup _toggleGroup;

		protected override void OnAwake ()
		{
			base.OnAwake ();
			_toggleGroup.AddButton (this);
		}

		protected override void OnInteractionClick ()
		{
			base.OnInteractionClick ();
			_toggleGroup.OnButtonClick (this);
			SetInteractivityStatus (false);
		}

		public void Reset() {

			SetInteractivityStatus (true);
			_ui.Deactivate ();
		}
	}
}