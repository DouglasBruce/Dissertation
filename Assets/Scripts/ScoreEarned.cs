using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreEarned : MonoBehaviour
{

	public Text scoreText;

	void OnEnable ()
	{
		StartCoroutine (AnimateText ());
	}

	IEnumerator AnimateText ()
	{
		scoreText.text = "0";
		int score = 0;

		yield return new WaitForSeconds (.5f);

		while (score < PlayerStats.Score) {
			score++;
			scoreText.text = score.ToString ();

			yield return new WaitForSeconds (.03f);
		}
	}
}
