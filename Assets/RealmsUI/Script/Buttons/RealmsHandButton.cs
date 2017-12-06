using System;
using UnityEngine;

namespace PlaysnakRealms
{

	[RequireComponent (typeof(RealmsInteractiveItem))]
	[RequireComponent (typeof(RealmsHandButtonUI))]

	public class RealmsHandButton : MonoBehaviour
	{
		[SerializeField]
		private bool _isEnable = true;

		protected RealmsHandButtonUI _ui;
		protected RealmsInteractiveItem _interactivity;
	
		void Awake() {

			OnAwake ();
		}

		virtual protected void OnAwake() {

			_ui = gameObject.GetComponent<RealmsHandButtonUI> ();
		}

		private bool _isInteractive;
		protected void SetInteractivityStatus(bool isInteractive) {

			if(_interactivity == null)
				_interactivity = gameObject.GetComponent<RealmsInteractiveItem> ();

			if (_isInteractive == isInteractive)
				return;

			_isInteractive = isInteractive;

			if (isInteractive) {
				
				_interactivity.OnOver += OnInteractionOver;
				_interactivity.OnOut += OnInteractionOut;
				_interactivity.OnClick += OnInteractionClick;

			} else {

				_interactivity.OnOver -= OnInteractionOver;
				_interactivity.OnOut -= OnInteractionOut;
				_interactivity.OnClick -= OnInteractionClick;
			}
		}

		void Start() {

			isEnable = _isEnable;
			_ui.OnOut ();
		}

		virtual protected void OnInteractionClick ()
		{
			_ui.OnClick ();
		}

		virtual protected void OnInteractionOut ()
		{
			_ui.OnOut ();
		}

		virtual protected void OnInteractionOver ()
		{
			_ui.OnOver ();
		}

		public bool isEnable {
			get {
				return _isEnable;
			}
			set {
				_isEnable = value;
				_ui.SetEnable (value);
				SetInteractivityStatus (value);
			}
		}
	}
}
