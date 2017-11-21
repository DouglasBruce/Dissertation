using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	public GameObject UI;

	public string menuSceneName = "MainMenu";

	public SceneFader sceneFader;

	public Timer timer;
	public CameraController cameraController;

	public bool gamePaused = false;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.P)) {
			Toggle ();
		}
	}

	public void Toggle ()
	{
		UI.SetActive (!UI.activeSelf);
		gamePaused = !gamePaused;

		if (UI.activeSelf) {
			Time.timeScale = 0f;
			timer.pause = true;
			cameraController.doMovement = false;
		} else {
			Time.timeScale = 1f;
			timer.pause = false;
			cameraController.doMovement = true;
		}
	}

	public void Retry ()
	{
		Toggle ();
		sceneFader.FadeTo (SceneManager.GetActiveScene ().name);
	}

	public void Menu ()
	{
		Toggle ();
		sceneFader.FadeTo (menuSceneName);
	}
}
