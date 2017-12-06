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
			_filling.color = _transparentColor;
			ChangeAlphaTo (1f, 0.1f);
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
			ChangeAlphaTo (1f, 0.25f);
		}

		private void ChangeAlphaTo(float newAlpha, float speed) {

			StopAllCoroutines ();
			StartCoroutine (AnimateAlpha (newAlpha, speed));
		}

		IEnumerator AnimateAlpha(float newAlpha, float speed) {

			while (Mathf.Abs(_filling.color.a - newAlpha) > 0.05f) {

				Color nextColor = _originalColor;
				nextColor.a = Mathf.Lerp (_filling.color.a, newAlpha, Mathf.Min(speed, 1f));
				_filling.color = nextColor;
				yield return null;
			}

			_filling.color = _originalColor;
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

