using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(RealmPrevNextButtonUI))]
	public class RealmPrevNextButton : RealmButtonWithClickMeter {

		public enum Type : int {
			PREV,
			NEXT
		}

		[SerializeField]
		private Type _type;

		[SerializeField]
		private RealmGallery _gallery;

		private RealmPrevNextButtonUI _ui;

		protected override void Init ()
		{
			base.Init ();

			if(_gallery != null)
				_gallery = gameObject.GetComponentInParent<RealmGallery> ();

			Debug.Assert (_gallery != null, "Gallery is missing");

			_ui = gameObject.GetComponent<RealmPrevNextButtonUI> ();
			_ui.UpdateView (_type);

			gameObject.GetComponent<GazeClickMeter> ().autoReset = true;
		}

		protected override void ProcessClick ()
		{
			base.ProcessClick ();

			switch (_type) {

			case Type.NEXT:
				_gallery.NextImage ();
				break;

			case Type.PREV:
				_gallery.PrevImage ();
				break;
			}
		}
	}
}