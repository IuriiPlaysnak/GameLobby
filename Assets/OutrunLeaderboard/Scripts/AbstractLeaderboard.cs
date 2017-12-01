using System;

namespace Outrun
{
	abstract public class AbstractLeaderboard
	{
		public enum LeaderboardType : int
		{
			GLOBAL,
			AROUND_ME,
			FRIENDS
		}

		protected bool _isReady;

		public AbstractLeaderboard() {
			_isReady = false;
		}

		abstract public void LogIn();
		abstract protected void GetGlobalLeaderboard(int numberOfEntries);
		abstract protected void GetAroundMeLeaderboard(int numberOfEntries);
		abstract protected void GetFriendsLeaderboard(int numberOfEntries);
		abstract public void SavePlayerDistance(int distance);
		virtual public bool isReady { get { return _isReady; } }

		public void GetLeaderboardByType(LeaderboardType type, int numberOfEntries) {

			switch (type) {

			case LeaderboardType.GLOBAL:
				GetGlobalLeaderboard (numberOfEntries);
				break;

			case LeaderboardType.AROUND_ME:
				GetAroundMeLeaderboard (numberOfEntries);
				break;

			case LeaderboardType.FRIENDS:
				GetFriendsLeaderboard (numberOfEntries);
				break;
			}
		}
	}
}