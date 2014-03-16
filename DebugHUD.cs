using UnityEngine;
using System.Collections;

public class DebugHUD : MonoBehaviour
{
	public static DebugHUD debugHUD;
	public bool debugOn = true;
	public float updateInterval = 0.5f;
	private int frames = 0;
	private float duration = 0;
	
	// Singleton
	void Awake ()
	{
		if (debugHUD == null) {
			DontDestroyOnLoad (gameObject);
			debugHUD = this;
		} else if (debugHUD != this) {
			Destroy (gameObject);
		}
		
		if (!Debug.isDebugBuild)
			debugOn = false;
		
		if (guiText) {
			guiText.fontSize = (int)(Screen.width * 0.05f);
			if (debugOn)
				StartCoroutine (UpdateFPS ());
			else
				guiText.enabled = false;
		} else {
			Debug.LogWarning ("Please attach GUIText component to game object");
		}
	}
	
	void Update ()
	{
		++frames;
		duration += Time.deltaTime;
	}
	
	// Display FPS data
	IEnumerator UpdateFPS ()
	{
		float fps = 0;
		string format;
		// Cache reference to components
		GUIText FPStext = guiText;
		Material material = FPStext.material;
		
		while (true) {
			duration = 0;
			frames = 0;
			yield return new WaitForSeconds (updateInterval);
			
			// Only do division once
			duration = 1 / duration;
			
			// Only update if FPS changes
			if (fps - (frames * duration) < 0.01 && fps - (frames * duration) > -0.01) {
				continue;
			}
			
			fps = frames * duration;
			format = System.String.Format ("{0:F2} FPS", fps);
			FPStext.text = format;
			if (fps < 30)
				material.color = Color.yellow;
			else if (fps < 10)
				material.color = Color.red;
			else
				material.color = Color.green;
		}
	}
}
