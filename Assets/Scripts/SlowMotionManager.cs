/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

public class SlowMotionManager : MonoBehaviour {

	#region Variables
	public SlowMotion slowMotion;
	#endregion

	#region Unity Methods

	void OnTriggerEnter(Collider other)
	{
		// If collision between trigger and player is detected, activate slowmo 
		if (other.transform.parent.parent.tag == "Player")
		{
			slowMotion.DoSlowMotion();
		}
	}

	#endregion
}
