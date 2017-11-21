/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

[System.Serializable]
public class TrainBlueprint {

	#region Variables
	public GameObject prefab;

	public GameObject upgradedPrefab;
	public int upgradeCost;
	#endregion

	#region Unity Methods

	public int GetSellAmount(bool upgraded)
	{
		if (upgraded)
		{
			return upgradeCost;
		}
		return 0;
	}

	#endregion
}
