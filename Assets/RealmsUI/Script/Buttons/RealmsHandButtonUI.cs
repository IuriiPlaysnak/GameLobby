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

		private Color _fillingColor;
		private Color _fillingTransparentColor;
		private Color _disbaledColor;
		private Color _enabledColor;

		void Awake() {
			Init ();
		}

		virtual protected void Init() {

			_fillingColor = _fillingTransparentColor = _filling.color;
			_fillingTransparentColor.a = 0f;

			_enabledColor = _background.color;
			_disbaledColor = _background.color;
			_disbaledColor.a = 0.5f;
		}

		virtual public void OnOver() {

			_filling.enabled = true;
			_filling.color = _fillingTransparentColor;
			ChangeAlphaTo (1f, 0.1f);
		}

		virtual public void OnOut() {
			_filling.enabled = false;
		}

		virtual public void SetEnable(bool isEnable) {
			_background.color = isEnable ? _enabledColor : _disbaledColor;
		}

		virtual public void OnClick ()
		{
			_filling.color = _fillingTransparentColor;
			ChangeAlphaTo (1f, 0.25f);
		}

		private void ChangeAlphaTo(float newAlpha, float speed) {

			StopAllCoroutines ();
			StartCoroutine (AnimateAlpha (newAlpha, speed));
		}

		IEnumerator AnimateAlpha(float newAlpha, float speed) {

			while (Mathf.Abs(_filling.color.a - newAlpha) > 0.05f) {

				Color nextColor = _fillingColor;
				nextColor.a = Mathf.Lerp (_filling.color.a, newAlpha, Mathf.Min(speed, 1f));
				_filling.color = nextColor;
				yield return null;
			}

			_filling.color = _fillingColor;
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

