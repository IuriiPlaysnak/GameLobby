using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_leaderboardsVFX_01 : MonoBehaviour {

    public Image[] flickers;
    float flickerSpeed = 0.3f;
    float tstamp;
    int maxFlicker = 15;

	// Use this for initialization
	void Start () {
        tstamp = Random.Range(0,flickerSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time - tstamp >= flickerSpeed)
        {
            foreach (Image img in flickers)
            {
                float rndX = Random.Range(-maxFlicker, maxFlicker);
                float rndY = Random.Range(-maxFlicker, maxFlicker);
                float rndZ = Random.Range(-maxFlicker, maxFlicker);
                img.transform.localPosition = new Vector3(rndX, rndY, rndZ);
            }
            tstamp = Time.time;
        }
	}
}
