using UnityEngine;

public class CompleteLevelTrigger : MonoBehaviour
{

	public GameManager gameManager;

	public Timer timer;

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.parent.tag == "Player")
		{
			PlayerStats.Score = (int)((timer.countdown + PlayerStats.Money) * 0.3f);
			gameManager.WinLevel ();
			this.enabled = false;
		}
	}
}
