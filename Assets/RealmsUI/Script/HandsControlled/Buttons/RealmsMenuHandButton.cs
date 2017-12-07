using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RealmsMenuHandButtonUI))]
	public class RealmsMenuHandButton : RealmsToggleHandButton {

		[SerializeField]
		private UnityEvent _onClick;

		protected override void OnStart ()
		{
			base.OnStart ();

			if (_isToggledOnStart) {
				OnInteractionClick ();
				_ui.Activate ();
			}
		}

		protected override void OnInteractionClick ()
		{
			base.OnInteractionClick ();
			_onClick.Invoke ();
		}
	}
}