using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outrun {

	public class LocalLeaderboard : AbstractLeaderboard {

		#region implemented abstract members of AbstractLeaderboard

		public override void LogIn ()
		{
			throw new System.NotImplementedException ();
		}

		public override void SavePlayerDistance (int distance)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region implemented abstract members of AbstractLeaderboard

		protected override void GetGlobalLeaderboard (int numberOfEntries)
		{
			throw new System.NotImplementedException ();
		}

		protected override void GetAroundMeLeaderboard (int numberOfEntries)
		{
			throw new System.NotImplementedException ();
		}

		protected override void GetFriendsLeaderboard (int numberOfEntries)
		{
			throw new System.NotImplementedException ();
		}

		#endregion
	}
}