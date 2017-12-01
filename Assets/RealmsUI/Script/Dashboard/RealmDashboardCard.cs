using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaysnakRealms {
	
public class RealmDashboardCard : MonoBehaviour {

	[SerializeField]
	private RectTransform _canvasDimensions;

	private float _screenspaceWidth;

	public float screenspaceWidth {
		get {
			return _screenspaceWidth;
		}
		set {
			_screenspaceWidth = value;
		}
	}

    void Awake()
    {
        _screenspaceWidth = transform.localScale.x * _canvasDimensions.rect.width * _canvasDimensions.localScale.x;
    }
}

}