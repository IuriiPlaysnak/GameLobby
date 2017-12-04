using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outrun {

	public class PlatformsManager : MonoBehaviour {

		private const string OCULUS_DEVICE_NAME = "Oculus";
		private const string VIVE_DEVICE_NAME = "OpenVR";

		static public event System.Action<PlatformType> OnInitialized;

		private static PlatformsManager _instance;
		private bool _isEntitled;

		void Awake() {

			if (_instance == null) {

				_instance = this;
				DontDestroyOnLoad (this.gameObject);
				OnAwake ();

			} else {
				
				Destroy (this.gameObject);
			}
		}

		private bool _isOculusSupported;
		private bool _isOpenVRSupported;

		private void OnAwake() {

			foreach (var device in UnityEngine.XR.XRSettings.supportedDevices) {

				_isOculusSupported = _isOculusSupported || (device == OCULUS_DEVICE_NAME);
				_isOpenVRSupported = _isOpenVRSupported || (device == VIVE_DEVICE_NAME);
			}

			string deviceName = UnityEngine.XR.XRSettings.loadedDeviceName;

			Debug.Log (deviceName);

			if (deviceName == OCULUS_DEVICE_NAME) {

				gameObject
					.AddComponent<Outrun.OculusManager> ()
					.OnInit += OnOculusInit;

				if(_isOpenVRSupported)
					gameObject.AddComponent<SteamManager> ();

			} else if (deviceName == VIVE_DEVICE_NAME) {

				gameObject.AddComponent<SteamManager> ();
				StartCoroutine (WaitForSteamInit ());

			} else {

				Debug.LogError (string.Format ("Unknown device: {0}", deviceName));
			}
		}

		private float _timeOut = 10f;
		private IEnumerator WaitForSteamInit(){

			while (_timeOut > 0 &&  SteamManager.Initialized == false) {

				_timeOut -= Time.deltaTime;
				yield return null;
			}

			if (SteamManager.Initialized)
				InitComplete (PlatformType.STEAM);
			else
				InitComplete (PlatformType.LOCAL);
		}

		private void OnOculusInit(bool isSuccess) {

			if (isSuccess) {

				InitComplete (PlatformType.OCULUS);
			} else {

				if (_isOpenVRSupported && SteamManager.Initialized) {
					InitComplete (PlatformType.STEAM);
				} else { 
					InitComplete (PlatformType.LOCAL);
				}
			}
		}

		private void InitComplete(PlatformType platformType) {

			if (platformType == PlatformType.LOCAL) {

				if (_isOculusSupported) {

					Oculus
						.Platform
						.Entitlements
						.IsUserEntitledToApplication ()
						.OnComplete (OnOculusEntitlementResponse);
				}

			} else {
				
					_isEntitled = true;
			}
			
			if (OnInitialized != null)
				OnInitialized (platformType);
		}

		void OnOculusEntitlementResponse(Oculus.Platform.Message msg)
		{
			if(msg.IsError) {

				_isEntitled = false;
				Application.Quit();
				UnityEngine.Debug.LogError (string.Format("Entitlement error [{0}]: {1}", msg.GetError().Code, msg.GetError().Message));
			}
		}

		static public bool isEntitled {
			get {
				return _instance._isEntitled;
			}
		}

		public enum PlatformType : int {
			OCULUS,
			STEAM,
			LOCAL
		}
	}
}