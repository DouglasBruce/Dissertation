using UnityEngine;

[System.Serializable]
public class BridgeBlueprint {

    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

	// Returns the amount of money the user will receive from selling
	public int GetSellAmount (bool upgraded)
    {
        if (upgraded)
        {
            return cost + upgradeCost;
        }
        else
        {
            return cost;
        }
    }
}
