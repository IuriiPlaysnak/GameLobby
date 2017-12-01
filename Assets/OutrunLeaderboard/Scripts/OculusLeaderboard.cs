using System;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;

namespace Outrun
{
	public class OculusLeaderboard : AbstractLeaderboard
	{
		public event Action<List<OutrunLeaderboard.Entry>> OnLeaderboardData;

		private const string LONGEST_DISTANCE_LEADERBOARD_NAME = "LongestDistance";

		private ulong _playerId;
		private string _playerName;

		#region AbstractLeaderboard implementation

		override public void LogIn ()
		{
			UnityEngine.Debug.Log("Oculus authentification complete");

			_playerId = OculusManager.playerId;
			_playerName = OculusManager.playerName;
			_isReady = true;
		}

		override protected void GetGlobalLeaderboard (int numberOfEntries)
		{
			Leaderboards
				.GetEntries(
					LONGEST_DISTANCE_LEADERBOARD_NAME
					, numberOfEntries
					, LeaderboardFilterType.None
					, LeaderboardStartAt.Top)
				.OnComplete(OnData);
		}

		override protected void GetAroundMeLeaderboard (int numberOfEntries)
		{
			Leaderboards
				.GetEntries(
					LONGEST_DISTANCE_LEADERBOARD_NAME
					, numberOfEntries
					, LeaderboardFilterType.None
					, LeaderboardStartAt.CenteredOnViewer)
				.OnComplete(OnData);
		}

		protected override void GetFriendsLeaderboard (int numberOfEntries)
		{
			Leaderboards
				.GetEntries(
					LONGEST_DISTANCE_LEADERBOARD_NAME
					, numberOfEntries
					, LeaderboardFilterType.Friends
					, LeaderboardStartAt.CenteredOnViewerOrTop)
				.OnComplete(OnData);
		}

		override public void SavePlayerDistance (int distance)
		{
			_isReady = false;

			Leaderboards.WriteEntry(
				LONGEST_DISTANCE_LEADERBOARD_NAME
				, distance
			).OnComplete(OnNewEntryAdded);
		}

		void OnNewEntryAdded(Message msg) {

			_isReady = true;
		}

		#endregion

		// seems like this code represents a situation when user doesn't have an access to game (game is not intalled or such)
		private const int LOG_IN_ERROR = 1;

		// position of a player in leaderboard not found (played has never submited score)
		private const int NO_LEADERBOARD_ENTRY = 12074;

		private void OnData(Message<LeaderboardEntryList> msg) {

			if (msg.IsError) {

				switch(msg.GetError ().Code) {

				case NO_LEADERBOARD_ENTRY:
					UnityEngine.Debug.Log ("Oculus: No leaderboard entry for this user");
					SendDefaultEntry();
					break;

				case LOG_IN_ERROR:
					UnityEngine.Debug.Log ("Oculus: Log in error");
					SendDummyData ();
					break;

				default:
					UnityEngine.Debug.LogError (string.Format ("Error {0}: {1}", msg.GetError ().Code, msg.GetError ().Message));
					break;
				}

			} else {

				List<OutrunLeaderboard.Entry> result = new List<OutrunLeaderboard.Entry> ();

				foreach (var entry in msg.Data) {

					OutrunLeaderboard.Entry data = new OutrunLeaderboard.Entry () {
						userId = entry.User.ID.ToString()
							, userName = entry.User.OculusID
							, rank = entry.Rank
							, distance = (int)entry.Score
							, isMe = (entry.User.ID == _playerId)
					};

					result.Add (data);
				}

				if (OnLeaderboardData != null)
					OnLeaderboardData (result);
			}
		}

		private void SendDefaultEntry() {

			OutrunLeaderboard.Entry entry = new OutrunLeaderboard.Entry () {
				userId = _playerId.ToString()
					, userName = _playerName
					, distance = 0
					, rank = 0
					, isMe = true
			};

			if (OnLeaderboardData != null)
				OnLeaderboardData (new List<OutrunLeaderboard.Entry> () { entry });
		}

		private void SendDummyData() {

			UnityEngine.Debug.LogWarning ("Leaderboard dummy data");

			OutrunLeaderboard.Entry entry = new OutrunLeaderboard.Entry () {
				userId = "Disconnected"
					, userName = "Disconnected"
					, distance = 0
					, rank = 0
			};

			if (OnLeaderboardData != null)
				OnLeaderboardData (new List<OutrunLeaderboard.Entry> () { entry });
		}
	}
}