/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

public class CameraSwitcher : MonoBehaviour {

	#region Variables

	public GameManager gameManager;

	public PauseMenu pauseMenu;

	public Camera mainCamera;
	public Camera secondaryCamera;
	public Camera thirdCamera;

	public bool mainCameraActive = true;
	public bool thirdCameraActive = false;

	public GameObject shopUI;

	private GameObject trainForcesUI;
	private GameObject trainForcesUIBehind;
	private GameObject trainForcesUITop;

	private GameObject lastCam;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start () 
	{
		mainCamera.enabled = true;
		secondaryCamera.enabled = false;
		thirdCamera.enabled = false;

		lastCam = mainCamera.gameObject;

		trainForcesUI = GameObject.FindGameObjectWithTag("TrainForceUI");
		trainForcesUIBehind = GameObject.FindGameObjectWithTag("TrainForceUIBehind");
		trainForcesUITop = GameObject.FindGameObjectWithTag("TrainForceUITop");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Tab) && !pauseMenu.gamePaused)
		{
			if (lastCam.name == "Main Camera")
			{
				mainCamera.enabled = false;
				secondaryCamera.enabled = true;
				mainCameraActive = false;
				lastCam = secondaryCamera.gameObject;
			}
			else if (lastCam.name == "Secondary Camera")
			{
				secondaryCamera.enabled = false;
				thirdCamera.enabled = true;
				thirdCameraActive = true;
				lastCam = thirdCamera.gameObject;
			}
			else if (lastCam.name == "Third Camera")
			{
				thirdCamera.enabled = false;
				mainCamera.enabled = true;
				thirdCameraActive = false;
				mainCameraActive = true;
				lastCam = mainCamera.gameObject;
			}

			if (GameManager.Edit)
			{
				if (mainCamera.enabled)
				{
					shopUI.SetActive(true);
				}
				else
				{
					shopUI.SetActive(false);
				}
			}
			
			if (gameManager.forcesOn)
			{
				if (secondaryCamera.enabled)
				{
					trainForcesUI.GetComponent<CanvasGroup>().alpha = 0f;
					trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 1f;
					trainForcesUITop.GetComponent<CanvasGroup>().alpha = 0f;
				}
				else if (mainCamera.enabled)
				{
					trainForcesUI.GetComponent<CanvasGroup>().alpha = 1f;
					trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 0f;
					trainForcesUITop.GetComponent<CanvasGroup>().alpha = 0f;
				}
				else if (thirdCamera.enabled)
				{
					trainForcesUI.GetComponent<CanvasGroup>().alpha = 0f;
					trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 0f;
					trainForcesUITop.GetComponent<CanvasGroup>().alpha = 1f;
				}
			}
		}
	}
	
	#endregion
}
