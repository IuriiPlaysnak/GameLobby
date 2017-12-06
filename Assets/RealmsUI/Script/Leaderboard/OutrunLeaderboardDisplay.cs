using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outrun;

public class OutrunLeaderboardDisplay : MonoBehaviour
{
    public OutrunLeaderboardRow LeaderboardRowPrefab;
    public float MinY;
    public float YSpacing;
    public GameObject board;

    public enum LeaderboardOptions { global, myscore, friends};
    public LeaderboardOptions ListShown;

    List<OutrunLeaderboardRow> rows = new List<OutrunLeaderboardRow>();
    bool shouldFetch = false;

    void Update()
    {
		if (shouldFetch && OutrunLeaderboard.isReady)
        {
			OutrunLeaderboard.GetLeaderboardByType(AbstractLeaderboard.LeaderboardType.AROUND_ME, OnLeaderboardData);
            shouldFetch = false;
        }
    }

    public void ShowLeaderboard()
    {
        shouldFetch = true;
    }

    public void GetMyScoreLeaderboard()
    {
		LoadLeaderboard(AbstractLeaderboard.LeaderboardType.AROUND_ME, OnLeaderboardData);
    }
    public void GetGlobalLeaderboard()
    {
		LoadLeaderboard(AbstractLeaderboard.LeaderboardType.GLOBAL, OnLeaderboardData);
    }
	public void GetFriendsLeaderboard() {
        
		LoadLeaderboard(AbstractLeaderboard.LeaderboardType.FRIENDS, OnLeaderboardData);
	}

	private void LoadLeaderboard(AbstractLeaderboard.LeaderboardType type
		, System.Action<List<Outrun.OutrunLeaderboard.Entry>> onLeaderboardLoadedCallback) {

		StopAllCoroutines ();

		if (OutrunLeaderboard.isReady)
			OutrunLeaderboard.GetLeaderboardByType (type, onLeaderboardLoadedCallback);
		else
			StartCoroutine (LoadLeaderboardData (type, onLeaderboardLoadedCallback));
	}

	IEnumerator LoadLeaderboardData(
		AbstractLeaderboard.LeaderboardType type
		, System.Action<List<Outrun.OutrunLeaderboard.Entry>> onLeaderboardLoadedCallback) {

		while (OutrunLeaderboard.isReady == false)
			yield return null;

		OutrunLeaderboard.GetLeaderboardByType (type, onLeaderboardLoadedCallback);
	}

	private void OnLeaderboardData(List<Outrun.OutrunLeaderboard.Entry> entries)
    {
        foreach(OutrunLeaderboardRow row in rows)
        {
            GameObject.Destroy(row.gameObject);
        }

        rows.Clear();
        int Index = 0;
        foreach (var entry in entries)
        {
            OutrunLeaderboardRow newRow = GameObject.Instantiate<OutrunLeaderboardRow>(LeaderboardRowPrefab, board.transform);
            newRow.transform.localPosition = new Vector3(0, MinY + YSpacing * Index, 0);
            newRow.nameText.text =  entry.userName.ToLower();
			newRow.scoreText.text = ""+entry.distance.ToString().ToLower();
            newRow.rank.text = entry.rank.ToString() + ".";
            rows.Add(newRow);
            Index++;

			if (entry.isMe)
            {
				newRow.SetAsMe();
            }            
        }
    }
}
