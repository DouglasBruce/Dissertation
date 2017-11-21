/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

public class SlowMotion : MonoBehaviour {

	#region Variables
	public float slowdownFactor = 0.05f;
	public float slowdownLength = 5f;
	#endregion

	#region Unity Methods

	// Update is called once per frame
	void Update()
	{
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;

		// Clamp ensures the timescale never passes 1, which is real time
		Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

	public void DoSlowMotion()
	{
		// Set the timescale to the slowdown factor, if timescale equals 0.5, time is passing 2x slower than realtime
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
	
	#endregion
}
