using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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


		private Color _originalColor;
		private Color _transparentColor;

		void Awake() {
			Init ();
		}

		virtual protected void Init() {
			
			_originalColor = _filling.color;
			_transparentColor = _filling.color;
			_transparentColor.a = 0f;
		}

		virtual public void OnClick ()
		{
			_filling.color = _transparentColor;
			StartCoroutine (ChangeAlphaTo (1f));
		}

		IEnumerator ChangeAlphaTo(float newAlpha) {

			while (_filling.color.a < 1) {

				_filling.color = new Color (_filling.color.r, _filling.color.g, _filling.color.b, _filling.color.a + Time.deltaTime * 4);
				yield return null;
			}

			_filling.color = new Color (_filling.color.r, _filling.color.g, _filling.color.b, 1f);
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

