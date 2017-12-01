using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealmDashboardCard : MonoBehaviour {

    public enum CardType { News, Video, Video360, Images, ThreeD, Custom};
    public CardType type;
    public RectTransform canvasDimensions;
    float screenspaceWidth;
    public float GetScreenspaceWidth() { return screenspaceWidth; }
    public void SetScreenspaceWidth(float newWidth) { screenspaceWidth = newWidth; }

    public enum IsSubCard {none, subToLeft, parentToLeft};
    public IsSubCard subTo;

    void Awake()
    {
        screenspaceWidth = transform.localScale.x * canvasDimensions.rect.width * canvasDimensions.localScale.x;
    }

	private bool _isFrontShown = true;

	[SerializeField]
	private Animation _flipAnimation;

	private void Flip() {

		if (_isFrontShown)
		{
			_flipAnimation["flip"].speed = 1;
			_flipAnimation.Play();
			_isFrontShown = false;
		}
		else {
			_flipAnimation["flip"].speed = -1;
			_flipAnimation["flip"].time = _flipAnimation["flip"].length;
			_flipAnimation.Play();
			_isFrontShown = true;
		}
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void CreateSubcardLayout()
    {

    }
}
