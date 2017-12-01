using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutrunLeaderboardToolbar : MonoBehaviour {

    public ButtonWithClickMeterUI global;
    public ButtonWithClickMeterUI myScore;
    public ButtonWithClickMeterUI friends;

    // Use this for initialization
    void Start () {
        myScore.SetActiveState(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateGlobal()
    {
        friends.SetActiveState(false);
        myScore.SetActiveState(false);
        global.SetActiveState(true);
    }
    public void ActivateMyScore()
    {
        friends.SetActiveState(false);
        myScore.SetActiveState(true);
        global.SetActiveState(false);
    }
    public void ActivateFriends()
    {
        friends.SetActiveState(true);
        myScore.SetActiveState(false);
        global.SetActiveState(false);
    }
}
