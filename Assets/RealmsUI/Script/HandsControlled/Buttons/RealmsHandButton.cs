using System;
using UnityEngine;

namespace PlaysnakRealms
{

	[RequireComponent (typeof(RealmsInteractiveItem))]
	[RequireComponent (typeof(RealmsHandButtonUI))]

	public class RealmsHandButton : MonoBehaviour
	{
		[SerializeField]
		private bool _isEnableOnStart = true;

		private bool _isEnabled;

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

			OnStart ();
		}

		virtual protected void OnStart() {

			isEnabled = _isEnableOnStart;
			_ui.Deactivate ();
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

		public bool isEnabled {
			get {
				return _isEnabled;
			}
			set {
				_isEnabled = value;
				_ui.SetEnable (value);
				SetInteractivityStatus (value);
			}
		}
	}
}
