using UnityEngine;
using UnityEngine.UI;

namespace PlaysnakRealms
{
	public class RealmsHandButtonUI : MonoBehaviour
	{
		[SerializeField]
		protected Image _background;

		[SerializeField]
		protected Image _filling;

		[SerializeField]
		protected UnityEngine.UI.Text _buttonLabel;

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

		virtual public void OnOver() {
			_filling.enabled = true;
		}

		virtual public void OnOut() {
			_filling.enabled = false;
		}

		virtual public void OnClick ()
		{
			UpdateView();
		}

		public void SetState(State state) {

			_state = state;
			UpdateView ();
		}

		virtual protected void UpdateView() {

			Debug.LogWarning ("UpdateView: Has to be overriden");
		}
	}
}

