using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealmsGazeController : MonoBehaviour {


	[SerializeField]
	private GameObject _headAnchor;

	[SerializeField]
	private RealmsGazeCursor _cursor;

	private const float CLICK_TIMER = 1.0f;

	private bool _isFocusing;
	private float _focusTimer;
	private bool _isFocusConsumed;
	private RealmsInteractiveItem _lastInteraction;

	// Update is called once per frame
	void Update () {

		CheckHit ();
	}
		
	private void CheckHit() {

		Ray ray = new Ray (_headAnchor.transform.position, _headAnchor.transform.forward);
		RaycastHit hit;

 		if (Physics.Raycast (ray, out hit, 100f)) {

			_cursor.transform.position = hit.point;
			_cursor.transform.rotation = Quaternion.LookRotation (hit.normal);

			RealmsInteractiveItem ii = hit.collider.GetComponent<RealmsInteractiveItem> ();

			if (ii == null)
				return;

			if (ii != _lastInteraction) {
			
				_focusTimer = 0f;
				_isFocusConsumed = false;
				_isFocusing = false;
				Out (_lastInteraction);
			}
			
			_lastInteraction = ii;

			MoveOver (_lastInteraction, hit);
			Over (_lastInteraction);

			if (_lastInteraction.doShowCursor == false) {
				_cursor.UpdateMode (RealmsGazeCursor.Mode.INVISIBLE);
				return;
			}

			if(_lastInteraction.isClickable == false) {
				_cursor.UpdateMode (RealmsGazeCursor.Mode.NORMAL);
				return;
			}

			if (_isFocusConsumed == false) {

				if (_isFocusing == false) {

					_isFocusing = true;
					_focusTimer = CLICK_TIMER;

				} else {

					_cursor.UpdateMode (RealmsGazeCursor.Mode.TIMER);

					_focusTimer -= Time.deltaTime;

					if (_focusTimer <= 0f) {

						_isFocusConsumed = true;
						_focusTimer = 0f;
						Click (_lastInteraction);
						_cursor.UpdateMode (RealmsGazeCursor.Mode.NORMAL);
					}
				}

				_cursor.UpdateTimer (_focusTimer);
			}

		} else {

			_cursor.UpdateMode (RealmsGazeCursor.Mode.NORMAL);
			_cursor.transform.position = _headAnchor.transform.position + _headAnchor.transform.forward * 10f;
			_cursor.transform.rotation = Quaternion.LookRotation (_cursor.transform.position - _headAnchor.transform.position);

			_isFocusing = false;
			_isFocusConsumed = false;
			_cursor.UpdateTimer (1);

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
