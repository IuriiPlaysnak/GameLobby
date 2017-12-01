using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outrun;

public class FetchLeaderboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


	private bool _isFetched;
	void Update () {

		if (_isFetched == false) {

			if (OutrunLeaderboard.isReady) {

				_isFetched = true;
				OutrunLeaderboard.GetLeaderboardByType (AbstractLeaderboard.LeaderboardType.GLOBAL, OnLeaderboardData);
				OutrunLeaderboard.GetLeaderboardByType (AbstractLeaderboard.LeaderboardType.AROUND_ME, OnLeaderboardData);
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {

			OutrunLeaderboard.SavePlayerDistance (Random.Range(0, 10000));
			_isFetched = false;
		}
	}

	private void OnLeaderboardData(List<OutrunLeaderboard.Entry> entries) {

		Debug.Log ("Leaderboard:");

		foreach (var entry in entries) {

			Debug.Log (string.Format("Entry: Username = {0}, Rank = {1}, Distance = {2}", entry.userName, entry.rank, entry.distance));
		}
	}
}
