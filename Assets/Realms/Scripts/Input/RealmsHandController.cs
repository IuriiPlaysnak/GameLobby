using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {

	[RequireComponent (typeof(LineRenderer))]
	public class RealmsHandController : MonoBehaviour {

		[SerializeField]
		private OVRInput.Controller _controllerID = OVRInput.Controller.None;

		[SerializeField]
		private OVRInput.RawButton _actionButton;

		[SerializeField]
		private OVRInput.RawButton _cancelButton;

		private RealmsInteractiveItem _lastInteraction;

		private LineRenderer _lineRender;

		void Awake() {

			_lineRender = gameObject.GetComponent<LineRenderer> ();
		}

		void Update() {

			_lineRender.enabled = (OVRInput.GetActiveController () & _controllerID) != 0;

			if (_lineRender.enabled == false)
				return;
			
			CheckHit ();
		}

		private void CheckHit() {

			_lineRender.SetPosition (0, transform.position);

			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;
			bool isSomethingHit = Physics.Raycast (ray, out hit, 100);

			if (isSomethingHit)
				_lineRender.SetPosition (1, hit.point);
			else
				_lineRender.SetPosition (1, transform.forward * 100);
	
			if (isSomethingHit) {

				RealmsInteractiveItem ii = hit.collider.GetComponent<RealmsInteractiveItem> ();

				if (ii == null || ii != _lastInteraction) {
					Out (_lastInteraction);
				}

				_lastInteraction = ii;

				MoveOver (_lastInteraction, hit);
				Over (_lastInteraction);

			} else {

				Out (_lastInteraction);
				_lastInteraction = null;
			}
		}

		private void Click(RealmsInteractiveItem item) {

			if(item != null)
				item.Click ();
		}

		private void MoveOver(RealmsInteractiveItem item, RaycastHit hit) {

			if (item != null)
				item.MoveOver (hit);
		}

		private void Out(RealmsInteractiveItem item) {

			if (item != null)
				item.Out ();
		}

		private void Over(RealmsInteractiveItem item) {

			if (item != null)
				item.Over ();
		}
	}
}
