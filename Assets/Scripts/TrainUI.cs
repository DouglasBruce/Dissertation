/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;
using UnityEngine.UI;

public class TrainUI : MonoBehaviour {

	#region Variables

	BuildManager buildManager;
	public GameObject UI;

	public Text upgradeText;
	public Text upgradeCost;
	public Button upgradeButton;

	private Train target;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start()
	{
		buildManager = BuildManager.instance;
	}

	public void SetTarget(Train _target)
	{
		target = _target;

		transform.position = _target.GetTrainPosition();

		if (!buildManager.isTrainUpgraded)
		{
			upgradeText.text = "UPGRADE";
			upgradeCost.text = "£" + buildManager.trainBlueprint.upgradeCost;
		}
		else
		{
			upgradeText.text = "DOWNGRADE";
			upgradeCost.text = "£" + buildManager.trainBlueprint.upgradeCost;
		}

		UI.SetActive(true);
	}

	public void Hide()
	{
		UI.SetActive(false);
	}

	public void Upgrade()
	{
		Hide();
		if (buildManager.isTrainUpgraded)
		{
			target.DowngradeTrain();
		}
		else
		{
			target.UpgradeTrain();
		}
	}

	#endregion
}
