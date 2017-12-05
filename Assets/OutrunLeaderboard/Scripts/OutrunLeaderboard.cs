using System;
using UnityEngine;
using System.Collections.Generic;
using PlaysnakRealms;

namespace Outrun
{
	public class OutrunLeaderboard : MonoBehaviour
	{

		private enum DataSource : int {
			OCULUS,
			STEAM,
			LOCAL
		}

		private DataSource _dataSource;

		[SerializeField]
		private int _leaderboardEntriesCount = 10;

		public int leaderboardEntriesCount {
			get {
				return _leaderboardEntriesCount;
			}
		}

		static private OutrunLeaderboard _instance;
		static private AbstractLeaderboard _leaderboard;

		private System.Action<List<Entry>> _onLeaderboardLoadedCallback;

		static public void GetLeaderboardByType(AbstractLeaderboard.LeaderboardType type, System.Action<List<Entry>> onLeaderboardLoadedCallback) {

			if (isReady == false) {

				if(onLeaderboardLoadedCallback != null)
					onLeaderboardLoadedCallback (new List<Entry> ());
				
				return;
			}

			_instance._onLeaderboardLoadedCallback = onLeaderboardLoadedCallback;
			_leaderboard.GetLeaderboardByType (type, _instance.leaderboardEntriesCount);
		}

		static public void SavePlayerDistance(int distance) {

			if (isReady == false) {
				return;
			}

			_leaderboard.SavePlayerDistance(distance);
		}

		static public bool isReady {
			get { return (_leaderboard != null && _leaderboard.isReady); }
		}

		void Awake() {

			if (_instance == null) 
				_instance = this;

			RealmsPlatformsManager.OnInitialized += OnPlatformInitialized;
		}

		void OnPlatformInitialized (RealmsPlatformsManager.PlatformType platformType)
		{
			switch (platformType) {

			case RealmsPlatformsManager.PlatformType.OCULUS:
				_dataSource = DataSource.OCULUS;
				break;

			case RealmsPlatformsManager.PlatformType.STEAM:
				_dataSource = DataSource.STEAM;
				break;

			case RealmsPlatformsManager.PlatformType.LOCAL:
				_dataSource = DataSource.LOCAL;
				break;
			}

			switch (_dataSource) {

			case DataSource.OCULUS:
				_leaderboard = new OculusLeaderboard ();
				(_leaderboard as OculusLeaderboard).OnLeaderboardData += OnLeaderboardFormatedData;
				break;

			case DataSource.STEAM:
				_leaderboard = new SteamLeaderboard ();
				(_leaderboard as SteamLeaderboard).OnLeaderboardData += OnLeaderboardFormatedData;
				break;

			case DataSource.LOCAL:
				_leaderboard = new LocalLeaderboard ();
				break;
			}

			LogInToLeaderboard ();
		}

		void OnLeaderboardFormatedData(List<Entry> entries) {

			if (_instance._onLeaderboardLoadedCallback != null)
				_instance._onLeaderboardLoadedCallback (entries);
		}

		private void LogInToLeaderboard() {

			_leaderboard.LogIn();
		}

		public struct Entry {

			public bool isMe;
			public string userId;
			public string userName;
			public int rank;
			public int distance;
		}
	}
}