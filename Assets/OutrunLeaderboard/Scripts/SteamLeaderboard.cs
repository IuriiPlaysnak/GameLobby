using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Outrun;

namespace Outrun {
	public class SteamLeaderboard : AbstractLeaderboard {

		private const string LEADERBOARD_NAME = "LongestDistance";

		public event System.Action<List<OutrunLeaderboard.Entry>> OnLeaderboardData;

		private SteamLeaderboard_t _leaderboardId;

		private CSteamID _playerId;
		private string _playerName;

		#region implemented abstract members of AbstractLeaderboard

		public override void LogIn ()
		{
			UnityEngine.Debug.Log ("Steam login");

			if (SteamManager.Initialized) {

				GetUserData ();
			}
			else {
				UnityEngine.Debug.LogError ("Steam is not initialized");
			}
		}

		private CallResult<LeaderboardScoresDownloaded_t> _getGlobalLeaderboardRequest;
		protected override void GetGlobalLeaderboard (int numberOfEntries)
		{
			_getGlobalLeaderboardRequest = CallResult<LeaderboardScoresDownloaded_t>.Create (OnGetEntriesRersponse);

			SteamAPICall_t handler = SteamUserStats.DownloadLeaderboardEntries (
				_leaderboardId
				, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal
				, 0
				, numberOfEntries
			);

			_getGlobalLeaderboardRequest.Set (handler);
		}

		private CallResult<LeaderboardScoresDownloaded_t> _getAroundMeLeaderboardRequest;
		protected override void GetAroundMeLeaderboard (int numberOfEntries)
		{
			_getAroundMeLeaderboardRequest = CallResult<LeaderboardScoresDownloaded_t>.Create (OnGetEntriesRersponse);

			SteamAPICall_t handler = SteamUserStats.DownloadLeaderboardEntries (
				_leaderboardId					
				, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser
				, -(int)UnityEngine.Mathf.Floor(numberOfEntries / 2)
				, (int)UnityEngine.Mathf.Ceil(numberOfEntries / 2)
			);

			_getAroundMeLeaderboardRequest.Set(handler);
		}

		private CallResult<LeaderboardScoresDownloaded_t> _getFriendsLeaderboardRequest;
		protected override void GetFriendsLeaderboard (int numberOfEntries)
		{
			_getFriendsLeaderboardRequest = CallResult<LeaderboardScoresDownloaded_t>.Create (OnGetEntriesRersponse);

			SteamAPICall_t handler = SteamUserStats.DownloadLeaderboardEntries (
				_leaderboardId					
				, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends
				,  0
				, numberOfEntries
			);

			_getFriendsLeaderboardRequest.Set(handler);
		}

		private void OnGetEntriesRersponse (LeaderboardScoresDownloaded_t callback, bool isFailure) {

			ParseLeaderboardData (callback);
		}

		private CallResult<LeaderboardScoreUploaded_t> _saveEntryRequest;
		public override void SavePlayerDistance (int distance)
		{
			_isReady = false;

			_saveEntryRequest = CallResult<LeaderboardScoreUploaded_t>.Create (OnSaveEntryResponse);

			SteamAPICall_t handler = SteamUserStats.UploadLeaderboardScore (
				_leaderboardId
				, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest
				, distance
				, null
				, 0
			);

			_saveEntryRequest.Set (handler);
		}

		private void OnSaveEntryResponse(LeaderboardScoreUploaded_t callback, bool isFail) {

			if(isFail) {
				UnityEngine.Debug.LogError("Something went wrong with writing to leaderboard");
			}
			else {
				UnityEngine.Debug.Log("Leaderboard entry added");
				_isReady = true;
			}

		}

		#endregion

		private void GetUserData () {

			_playerId = SteamUser.GetSteamID ();
			_playerName = SteamFriends.GetPersonaName ();

			UnityEngine.Debug.Log (string.Format("Steam GetUserData: id = {0}, name = {1}", _playerId, _playerName));

			GetLeaderboardId();
		}

		private CallResult<LeaderboardFindResult_t> _findLeaderboardRequest;
		private void GetLeaderboardId() {

			UnityEngine.Debug.Log ("Steam GetLeaderboardId");

			_findLeaderboardRequest = CallResult<LeaderboardFindResult_t>.Create (OnFindLeaderboardResponse);
			_findLeaderboardRequest.Set (SteamUserStats.FindLeaderboard (LEADERBOARD_NAME));
		}

		private void OnFindLeaderboardResponse(LeaderboardFindResult_t callback, bool isFailure) {

			if (isFailure || callback.m_bLeaderboardFound != 1) {
				UnityEngine.Debug.LogError (string.Format ("Leaderboard {0} is not found", LEADERBOARD_NAME));
			} else {
				UnityEngine.Debug.Log (string.Format ("Leaderboard {0} found", LEADERBOARD_NAME));
				_leaderboardId = callback.m_hSteamLeaderboard;
				_isReady = true;
			}
		}

		private void ParseLeaderboardData(LeaderboardScoresDownloaded_t steamData) {

			UnityEngine.Debug.Log ("Steam ParseLeaderboardData");

			List<OutrunLeaderboard.Entry> result = new List<OutrunLeaderboard.Entry> ();

			for(var i = 0; i < steamData.m_cEntryCount; i++)
			{
				LeaderboardEntry_t steamEntry;
				SteamUserStats.GetDownloadedLeaderboardEntry(steamData.m_hSteamLeaderboardEntries, i, out steamEntry, null, 0);

				OutrunLeaderboard.Entry resulEntry = new OutrunLeaderboard.Entry () {

					userId = steamEntry.m_steamIDUser.ToString()
						, userName = SteamFriends.GetFriendPersonaName(steamEntry.m_steamIDUser)
						, distance = steamEntry.m_nScore
						, rank = steamEntry.m_nGlobalRank
						, isMe = (steamEntry.m_steamIDUser == _playerId)
				};

				result.Add (resulEntry);
			}

			if (OnLeaderboardData != null)
				OnLeaderboardData (result);
		}
	}
}
