using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlaysnakRealms {
	
	public class OutrunRealmJSONDataSource : AbstractDataSource  {

		#region IOutrunRealmDataSource implementation

		override public void Load (string url)
		{
			OnContentItemLoadingStart ();
			OutrunRealmDataProvider.instance.StartCoroutine (LoadSettingsJSON (url));
		}

		private IEnumerator LoadSettingsJSON(string url) {

			Debug.Log ("Setting loading...");

			WWW request = new WWW (url);
			yield return request;

			if (request.error != null) {

				Debug.LogError (request.error);

			} else {

				Debug.Log ("Settings loading complete!");

				OutrunRealmDataProvider.SettingData result = JsonUtility.FromJson<OutrunRealmDataProvider.SettingData> (request.text);

				OnContentItemLoadingComplete ();
			}
		}

		#endregion
	}
}