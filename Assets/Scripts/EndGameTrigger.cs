using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{

	public GameObject deathEffect;

	public Timer timer;

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.parent.tag == "Player")
		{
			PlayerStats.Score = (int)((timer.countdown + PlayerStats.Money) * 0.3f);
			GameObject effect = Instantiate (deathEffect, other.transform.position, Quaternion.identity);
			Destroy (effect, 5f);
			Destroy (other.gameObject);
			PlayerStats.IsDead = true;
		}
	}
}
