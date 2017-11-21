using UnityEngine;

public class MainMenu : MonoBehaviour
{

	public string levelToLoad = "LevelSelect";

	public SceneFader sceneFader;

	public void Play ()
	{
		sceneFader.FadeTo (levelToLoad);
	}

	public void Quit ()
	{
		Debug.Log ("Exiting...");

		// Application.Quit() will not run in the Unity Editor, therefore the above Debug.Log statement as been added
		Application.Quit ();
	}
}
