using System;
using UnityEngine;

namespace PlaysnakRealms
{

	[RequireComponent (typeof(RealmsInteractiveItem))]
	[RequireComponent (typeof(RealmsHandButtonUI))]

	public class RealmsHandButton : MonoBehaviour
	{
		public event System.Action OnClick;
		public event System.Action OnOver;
		public event System.Action OnOut;

		protected RealmsHandButtonUI _ui;
	
		void Awake() {

			Init ();
		}

		virtual protected void Init() {

			RealmsInteractiveItem ii = gameObject.GetComponent<RealmsInteractiveItem> ();
			ii.OnOver += OnInteractionOver;
			ii.OnOut += OnInteractionOut;
			ii.OnClick += OnInteractionClick;

			_ui = gameObject.GetComponent<RealmsHandButtonUI> ();
			_ui.OnOut ();
		}

		virtual protected void OnInteractionClick ()
		{
			_ui.OnClick ();

			if (OnClick != null)
				OnClick ();
		}

		virtual protected void OnInteractionOut ()
		{
			_ui.OnOut ();

			if (OnOut != null)
				OnOut ();
		}

		virtual protected void OnInteractionOver ()
		{
			_ui.OnOver ();

			if (OnOver != null)
				OnOver ();
		}
	}
}

