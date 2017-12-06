using System;
using UnityEngine;

namespace PlaysnakRealms
{

	[RequireComponent (typeof(RealmsInteractiveItem))]

	public class RealmsHandButton : MonoBehaviour
	{
		public event System.Action OnClick;
		public event System.Action OnOver;
		public event System.Action OnOut;
	
		void Awake() {

			RealmsInteractiveItem ii = gameObject.GetComponent<RealmsInteractiveItem> ();
			ii.OnOver += Ii_OnOver;
			ii.OnOut += Ii_OnOut;
			ii.OnClick += Ii_OnClick;
		}

		void Ii_OnClick ()
		{
			if (OnClick != null)
				OnClick ();
		}

		void Ii_OnOut ()
		{
			if (OnOut != null)
				OnOut ();
		}

		void Ii_OnOver ()
		{
			if (OnOver != null)
				OnOver ();
		}
	}
}

