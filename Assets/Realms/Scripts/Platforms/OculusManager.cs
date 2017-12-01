using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using UnityEngine;

namespace Outrun {

	public class OculusManager : MonoBehaviour {

		public event System.Action<bool> OnInit;

		static private OculusManager _instance;

		private bool _isInitialized;
		private ulong _playerId;
		private string _playerName;

		void Awake() {

			if (_instance == null)
				_instance = this;

			_isInitialized = false;

			Init ();
		}

		private void Init() {

			Oculus.Platform.Core.AsyncInitialize ().OnComplete (OnInitComplete);
		}

		private void OnInitComplete(Message message) {

			if (message.IsError) {

				UnityEngine.Debug.LogError (
					string.Format(
						"Oculus authentification error: [{0}] {1}"
						, message.GetError().Code
						, message.GetError().Message
					)
				);

				InitComplete (false);

			} else {
				
				GetUserData ();
			}
		}

		private void GetUserData() {

			if (Oculus.Platform.Core.IsInitialized() == false) {
				UnityEngine.Debug.LogError ("Oculus Platform is not initialized. Call Init() method first");
				InitComplete (false);
				return;
			}
				
			Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
		}

		private void GetLoggedInUserCallback(Message<User> message) {
			
			if (message.IsError) {
				UnityEngine.Debug.LogError (
					string.Format(
						"Oculus authentification error: [{0}] {1}"
						, message.GetError().Code
						, message.GetError().Message
					)
				);

				InitComplete (false);
			}
			else {
				OnPlayerData (message.Data.ID, message.Data.OculusID);
			}
		}

		private void OnPlayerData(ulong id, string name) {

			Debug.Log (string.Format("Oculus player [{0}]: {1}", id, name));

			_playerId = id;
			_playerName = name;
			_isInitialized = true;

			InitComplete (true);
		}

		private void InitComplete(bool isSuccess) {

			if (OnInit != null)
				OnInit (isSuccess);
		}

		static public bool IsInitialized {
			get { return _instance._isInitialized; }
		}

		static public ulong playerId {
			get {
				return _instance._playerId;
			}
		}

		static public string playerName {
			get {
				return _instance._playerName;
			}
		}
	}
}