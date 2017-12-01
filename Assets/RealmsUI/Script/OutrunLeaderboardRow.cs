using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutrunLeaderboardRow : MonoBehaviour
{
    public Text nameText;
    public Text scoreText;
    public Text rank;
    public Image active;
    public Image active2;
    public Image currency;

    public bool itsMe;

    private void Start()
    {
        if (itsMe)
        {

        }
    }

    public void SetAsMe()
    {
        itsMe = true;
        nameText.color = Color.black;
        scoreText.color = Color.black;
        rank.color = Color.black;
        currency.color = Color.black;
        active.color = OutrunGlobals.ColorScheme["mainColor"];
        active2.color = OutrunGlobals.ColorScheme["mainColor"];
    }
}
