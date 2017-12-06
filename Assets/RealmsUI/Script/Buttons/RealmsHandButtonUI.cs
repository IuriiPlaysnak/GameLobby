using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace PlaysnakRealms
{
	abstract public class RealmsHandButtonUI : MonoBehaviour
	{
		public enum State : byte
		{
			MUTE,
			UNMUTE,
			PLAY,
			PAUSE,
			PREV,
			NEXT
		}

		protected State _state;

		void Awake() {
			Init ();
		}

		virtual protected void Init() {	}
		virtual public void OnOver() { }
		virtual public void OnOut() { }
		virtual public void SetEnable(bool isEnable) { }
		virtual public void OnClick () { }

		public void SetState(State state) {

			_state = state;
			UpdateView ();
		}

		virtual protected void UpdateView() {

			Debug.LogWarning ("UpdateView: Has to be overriden");
		}
	}
}