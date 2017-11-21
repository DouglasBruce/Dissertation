using UnityEngine;

public class CompleteLevel : MonoBehaviour
{

	public string menuSceneName = "MainMenu";

	public string nextLevel = "Level02"; // Default value, changed in inspector
	public int levelToUnlock = 2; // Default value, changed in inspector

	public SceneFader sceneFader;

	public void Continue ()
	{
		UnlockNextLevel();
		sceneFader.FadeTo (nextLevel);
	}

	public void Menu ()
	{
		UnlockNextLevel();
		sceneFader.FadeTo (menuSceneName);
	}

	void UnlockNextLevel()
	{
		// Override PlayerPrefs levelReached to ensure the user can access the next level from the level select menu
		PlayerPrefs.SetInt("levelReached", levelToUnlock);
	}
}
