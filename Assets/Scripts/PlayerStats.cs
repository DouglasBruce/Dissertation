using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public static int Money;
	public int startMoney = 500; // Default value, changed in inspector of each level

	public static int Score;

	public static bool IsDead;

	// Use this for initialization
	void Start ()
	{
		Money = startMoney;
		Score = 0;
		IsDead = false;
	}
}
