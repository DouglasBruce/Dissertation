/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	#region Variables
	public Text countdownText;
	public float countdown = 60f;

	[HideInInspector]
	public bool pause = false;
	
	#endregion

	#region Unity Methods
	
	// Update is called once per frame
	void Update () 
	{
		if (!pause)
		{
			countdown -= Time.deltaTime;
		}
		
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

		int minutes = (int) countdown / 60;
		int seconds = (int) countdown % 60;
		float fraction = countdown * 100;
		fraction = fraction % 100;

		if (minutes == 0)
		{
			countdownText.text = string.Format("{0:0}:{1:00}", seconds, fraction);
		}
		else
		{
			countdownText.text = string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, fraction);
		}

		if (countdown <= 10)
		{
			countdownText.color = Color.red;
		}
		else
		{
			countdownText.color = Color.white;
		}

		if (countdown <= 0)
		{
			GameManager.GameInPlay = true;
		}
	}
	
	#endregion
}
