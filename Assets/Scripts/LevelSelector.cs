using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{

	public SceneFader sceneFader;

	public Button[] levelButtons;

	// Use this for initialization
	public void Start ()
	{
		// If PlayerPrefs "levelReached does not exist, create one and give it the default value of 1
		// This ensures that the user will always be able to play the first level
		int levelReached = PlayerPrefs.GetInt ("levelReached", 1);

		// Loops around each of the level tiles
		for (int i = 0; i < levelButtons.Length; i++) {
			// If greater than levelReached disable button, this prevents players from accessing all of the levels immediately
			if (i + 1 > levelReached)
				levelButtons [i].interactable = false;
		}
	}

	// Allows me to delete all PlayerPrefs, used only for testing and in the final build will be deleted or will not be called on a button click.
	public void Delete ()
	{
		PlayerPrefs.DeleteAll ();
	}

	public void Select (string levelName)
	{
		// Passes the desired level to be loaded to the scene fader
		sceneFader.FadeTo (levelName);
	}
}
