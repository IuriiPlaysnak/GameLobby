using UnityEngine;

public class RealmPlayGameButton : MonoBehaviour
{
    public UnityEngine.UI.Text notEntitledMessage;

	public void PlayGame ()
	{
		if (Outrun.PlatformsManager.isEntitled == false) {
			Debug.LogError ("Player is not entitled");
			return;
		}

		ScenesManager.LoadScene(ScenesManager.SceneID.GAME);
    }
}
