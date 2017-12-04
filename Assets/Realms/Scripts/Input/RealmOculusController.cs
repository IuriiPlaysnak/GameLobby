using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(LineRenderer))]
	public class RealmOculusController : MonoBehaviour {

		[SerializeField]
		private OVRInput.Controller _controllerID = OVRInput.Controller.None;

		[SerializeField]
		private OVRInput.RawButton _actionButton;

		[SerializeField]
		private OVRInput.RawButton _cancelButton;

		private LineRenderer _lineRender;

		void Awake() {

			_lineRender = gameObject.GetComponent<LineRenderer> ();
		}

		void Update() {

			_lineRender.enabled = (OVRInput.GetActiveController () & _controllerID) != 0;

			if (_lineRender.enabled == false)
				return;

			_lineRender.SetPosition (0, transform.position);

			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;
			bool isSomethingHit = Physics.Raycast (ray, out hit, 100);

			if (isSomethingHit)
				_lineRender.SetPosition (1, hit.point);
			else
				_lineRender.SetPosition (1, transform.forward * 100);

			if (OVRInput.GetUp (_actionButton)) {

				if (isSomethingHit) {

					RealmInteractiveItem interactiveItem = hit.collider.GetComponent<RealmInteractiveItem> ();
					if (interactiveItem != null)
						interactiveItem.Click ();
				}
			} else if (OVRInput.GetUp (_cancelButton)) {

				if (isSomethingHit) {

					RealmInteractiveItem interactiveItem = hit.collider.GetComponent<RealmInteractiveItem> ();
					if (interactiveItem != null)
						interactiveItem.Back ();
				}
			}
		}
	}
}
