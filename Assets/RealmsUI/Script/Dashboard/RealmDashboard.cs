﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {
	
	public class RealmDashboard : MonoBehaviour {

		private const float DISTANCE_BETWEEN_CARDS = 0.25f;
	    float angle = 0;
	    
	    public GameObject inBetweenSubs;
	    public RealmDashboardCard[] cards;

		// Use this for initialization

		void Start () {
	        float previousWidth = 0;
	        float DashboardRadius = 3.5f;
	        Vector3 previousMergePoint = Vector3.zero;
	        //transform.localScale = new Vector3(DashboardRadius, DashboardRadius, DashboardRadius);
	        
	        //Place cards on a circle with a distance from one another based on theire length
	        //the angle (a) between too cards depends on the length (x1, x2) of the cards
	        // a = cot(x1/2r)+cot(x2/2r)
			foreach (RealmDashboardCard card in cards)
	        {
				float thisAngle = Mathf.Atan2(card.screenspaceWidth*0.5f, DashboardRadius) * Mathf.Rad2Deg;
	            if (previousWidth != 0)
	            {
	                angle += thisAngle + (Mathf.Atan2(previousWidth*0.5f,  DashboardRadius) * Mathf.Rad2Deg) + (Mathf.Atan2(DISTANCE_BETWEEN_CARDS, DashboardRadius) * Mathf.Rad2Deg);
	            }
	            
				Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
	            card.transform.localPosition = Vector3.Normalize(direction) * DashboardRadius;
				Vector3 thisMerge = card.transform.localPosition + Vector3.Normalize(Quaternion.AngleAxis(-90, Vector3.up) * card.transform.localPosition) * card.screenspaceWidth / 2;
	            
	            float distOfMergePoints = (previousMergePoint - thisMerge).magnitude;
	            if (distOfMergePoints > 0.075f && distOfMergePoints != 0 && previousWidth != 0)
	            {
	                float tooLong = Mathf.Sin(thisAngle * Mathf.Deg2Rad) * distOfMergePoints;
					float factor = 1 - (2 * tooLong / card.screenspaceWidth);
	           
					card.screenspaceWidth = card.screenspaceWidth * (2 - factor);
	                angle -= thisAngle;
					thisAngle = Mathf.Atan2(card.screenspaceWidth*0.5f,DashboardRadius) * Mathf.Rad2Deg;
	                angle += thisAngle;
	                direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
	                card.transform.localPosition = Vector3.Normalize(direction) * DashboardRadius * factor;
	            }

				card.transform.localRotation = Quaternion.LookRotation (card.transform.localPosition - Vector3.zero);

				previousWidth = card.screenspaceWidth;
				previousMergePoint = card.transform.localPosition + Vector3.Normalize(Quaternion.AngleAxis(90, Vector3.up) * card.transform.localPosition) * card.screenspaceWidth / 2;
	        }
	        transform.localRotation = Quaternion.AngleAxis(-angle / 2, Vector3.up);
		}
	}
}