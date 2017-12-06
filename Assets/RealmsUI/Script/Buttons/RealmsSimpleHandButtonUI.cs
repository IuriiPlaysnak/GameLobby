using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PlaysnakRealms
{
	public class RealmsSimpleHandButtonUI : RealmsHandButtonUI
	{
		[SerializeField]
		protected Image _background;

		[SerializeField]
		protected Image _filling;

		[SerializeField]
		protected UnityEngine.UI.Text _buttonLabel;

		private Color _fillingColor;
		private Color _fillingTransparentColor;
		private Color _disbaledColor;
		private Color _enabledColor;

		protected override void Init ()
		{
			base.Init ();

			_fillingColor = _fillingTransparentColor = _filling.color;
			_fillingTransparentColor.a = 0f;

			_enabledColor = _background.color;
			_disbaledColor = _background.color;
			_disbaledColor.a = 0.5f;
		}

		public override void OnClick ()
		{
			_filling.enabled = true;
			_filling.color = _fillingTransparentColor;
			ChangeAlphaTo (1f, 0.25f);
		}

		public override void OnOut ()
		{
			_filling.enabled = false;
		}

		public override void OnOver ()
		{
			_filling.enabled = true;
			_filling.color = _fillingTransparentColor;
			ChangeAlphaTo (1f, 0.1f);
		}

		public override void SetEnable (bool isEnable)
		{
			_background.color = isEnable ? _enabledColor : _disbaledColor;
		}

		public override void Activate ()
		{
			_filling.enabled = true;
		}

		public override void Deactivate ()
		{
			_filling.enabled = false;
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
	}
}

