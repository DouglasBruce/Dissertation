/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Train : MonoBehaviour {

	#region Variables

	BuildManager buildManager;

	public CameraSwitcher cameraSwitcher;

	public TrainUI trainUI;

	public float mass;
	public float force;

	public Text trainMass;
	public Text trainForceUp;
	public Text trainForceDown;
	public Text trainForceUpBehind;
	public Text trainForceDownBehind;

	[HideInInspector]
	public GameObject train;

	private GameObject trainForcesUI;
	private GameObject trainForcesUIBehind;
	private GameObject trainForcesUITop;
	private Train selectedTrain;
	private bool isActive;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start()
	{
		cameraSwitcher = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraSwitcher>();

		trainForcesUI = GameObject.FindGameObjectWithTag("TrainForceUI");
		trainForcesUIBehind = GameObject.FindGameObjectWithTag("TrainForceUIBehind");
		trainForcesUITop = GameObject.FindGameObjectWithTag("TrainForceUITop");

		trainForcesUI.transform.SetParent(this.transform, true);
		trainForcesUIBehind.transform.SetParent(this.transform, true);
		trainForcesUITop.transform.SetParent(this.transform, true);

		trainMass = GameObject.FindGameObjectWithTag("TrainMass").GetComponent<Text>();
		mass = GetComponent<Rigidbody>().mass;
		trainMass.text = mass.ToString() + " kg";

		trainForceUp = GameObject.FindGameObjectWithTag("TrainForceUp").GetComponent<Text>();
		force = mass * 9.81f; // F = mg
		trainForceUp.text = force.ToString() + " N";

		trainForceDown = GameObject.FindGameObjectWithTag("TrainForceDown").GetComponent<Text>();
		trainForceDown.text = force.ToString() + " N";

		trainForceUpBehind = GameObject.FindGameObjectWithTag("TrainForceUpBehind").GetComponent<Text>();
		trainForceUpBehind.text = force.ToString() + " N";

		trainForceDownBehind = GameObject.FindGameObjectWithTag("TrainForceDownBehind").GetComponent<Text>();
		trainForceDownBehind.text = force.ToString() + " N";
		
		trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 0f;
		trainForcesUITop.GetComponent<CanvasGroup>().alpha = 0f;

		train = this.gameObject.transform.parent.gameObject;
		trainUI = GameObject.Find("TrainUI").GetComponent<TrainUI>();
		isActive = false;
		buildManager = BuildManager.instance;
	}

	public Vector3 GetTrainPosition()
	{
		return transform.position;
	}

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		if(GameManager.GameInPlay)
		{
			return;
		}

		if(!GameManager.Edit)
		{
			return;
		}

		if (!cameraSwitcher.mainCameraActive)
		{
			buildManager.Console("CAN'T BE SELECTED FROM THIS CAMERA!", new Color(1f, 1f, 1f, 1f));
			return;
		}

		if (!isActive)
		{
			isActive = true;
			buildManager.DeselectNode();
			buildManager.DeselectNodeSupport();
			trainUI.SetTarget(this);
		}
		else
		{
			isActive = false;
			trainUI.Hide();
		}
	}

	public void UpgradeTrain()
	{
		if (PlayerStats.Money < buildManager.trainBlueprint.upgradeCost)
		{
			buildManager.Console("NOT ENOUGH MONEY TO UPGRADE THAT!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
			Debug.Log("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= buildManager.trainBlueprint.upgradeCost;

		Vector3 lastPosition = train.transform.position;
		Quaternion lastRotation = train.transform.rotation;

		Destroy(train);

		GameObject _train = Instantiate(buildManager.trainBlueprint.upgradedPrefab, lastPosition, lastRotation);

		train = _train;

		GameObject effect = Instantiate(buildManager.buildEffect, GetTrainPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		buildManager.isTrainUpgraded = true;

		Debug.Log("Train upgraded!");
	}

	public void DowngradeTrain()
	{
		PlayerStats.Money += buildManager.trainBlueprint.upgradeCost;

		Vector3 lastPosition = train.transform.position;
		Quaternion lastRotation = train.transform.rotation;

		GameObject effect = Instantiate(buildManager.sellEffect, GetTrainPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(train);

		GameObject _train = Instantiate(buildManager.trainBlueprint.prefab, lastPosition, lastRotation);

		train = _train;

		buildManager.isTrainUpgraded = false;

		Debug.Log("Train downgraded!");
	}

	#endregion
}
