using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class NodeSupport : MonoBehaviour
{
	public CameraSwitcher cameraSwitcher;

	public Color hoverColor;
	public Color notEnoughMoneyColor;

	[HideInInspector]
	public GameObject bridge;

	[HideInInspector]
	public List<GameObject> bridges;

	[HideInInspector]
	public Transform parent;

	[HideInInspector]
	public BridgeBlueprint bridgeBlueprint;
	[HideInInspector]
	public bool isUpgraded = true;

	private Renderer rend;
	private Color startColor;

	public Vector3 spawnScale = new Vector3(1.0f, 1.0f, 1.0f);

	public float sizingFactor = 0.02f;
	public float rotatingFactor = 0.4f;
	public float maxSize = 1.5f;
	public float minSize = 0.5f;
	public float maxRot = 135;
	public float minRot = -270;

	private GameObject lastSpawn = null;

	private bool scaling = true;
	private bool rotating = false;

	private float startSize;
	private float startRotation;
	private float startX;
	private float startY;

	private int cost;

	[HideInInspector]
	public int totalCost;

	BuildManager buildManager;

	// Use this for initialization
	void Start ()
	{
		cameraSwitcher = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraSwitcher>();

		rend = GetComponent<Renderer> ();
		startColor = rend.material.color;

		bridge = null;
		lastSpawn = null;

		totalCost = 0;

		buildManager = BuildManager.instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (buildManager.Building && bridge != null && lastSpawn != null)
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				ToggleBoolValues();
			}
			if (scaling)
			{
				Vector3 size = lastSpawn.transform.localScale;
				size.x = startSize + (Input.mousePosition.x - startX) * sizingFactor;
				size.x = Mathf.Clamp(size.x, minSize, maxSize);
				lastSpawn.transform.localScale = size;
				
			}
			if (rotating)
			{
				Vector3 currentRotation = lastSpawn.transform.localRotation.eulerAngles;
				currentRotation.z = startRotation + (Input.mousePosition.y - startY) * rotatingFactor;
				currentRotation.z = Mathf.Clamp(currentRotation.z, minRot, maxRot);
				lastSpawn.transform.localRotation = Quaternion.Euler(currentRotation);
			}
			if (Input.GetMouseButton(1))
			{
				PlayerStats.Money -= cost;
				totalCost += cost;
				lastSpawn.transform.parent = parent;
				lastSpawn.GetComponentInChildren<HingeJoint>().connectedBody = bridge.gameObject.transform.parent.parent.GetChild(0).GetComponent<Rigidbody>();
				bridge.transform.GetChild(1).GetComponent<Collider>().isTrigger = false;
				bridges.Add(bridge);
				SetDefaults();
				GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
				Destroy(effect, 5f);
			}
			if (Input.GetMouseButton(2) || Input.GetKeyDown(KeyCode.Delete))
			{
				Destroy(lastSpawn);
				SetDefaults();
			}
		}
	}

	void ToggleBoolValues()
	{
		scaling = !scaling;
		rotating = !rotating;
		if (rotating)
		{
			buildManager.Console("ROTATE MODE!", new Color(1f, 1f, 1f, 1f));
		}
		else
		{
			buildManager.Console("SCALE MODE!", new Color(1f, 1f, 1f, 1f));
		}
	}

	void SetDefaults()
	{
		scaling = true;
		rotating = false;
		lastSpawn = null;
		buildManager.Building = false;
	}

	public Vector3 GetBuildPosition ()
	{
		return transform.position;
	}

	void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if (buildManager.Building)
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

		if (bridges.Count > 0)
		{
			buildManager.SelectNodeSupport(this);
			return;
		}

		if (buildManager.GetBridgeToBuild() != null)
		{
			if (buildManager.GetBridgeToBuild().prefab.name != "WoodenSupport" && buildManager.GetBridgeToBuild().prefab.name != "SteelSupport")
			{
				buildManager.Console("BRIDGES CAN'T BE BUILT THERE!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
				Debug.Log("Bridges can not be built there!");
				return;
			}
		}

		if (!buildManager.CanBuild) {
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {
			BuildBridge (buildManager.GetBridgeToBuild (), hitInfo);
		}
	}

	public void BuildBridge (BridgeBlueprint blueprint, RaycastHit hitInfo)
	{
		Vector3 spawnSpot = hitInfo.collider.transform.position;
		Quaternion spawnRotation = hitInfo.collider.transform.rotation;
		
		if (PlayerStats.Money < blueprint.cost) {
			buildManager.Console("NOT ENOUGH MONEY TO BUILD THAT!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
			Debug.Log ("Not enough money to build that!");
			return;
		}

		cost = blueprint.cost;

		GameObject _bridge = Instantiate (blueprint.prefab, spawnSpot, spawnRotation);

		parent = hitInfo.collider.transform;

		_bridge.transform.localScale = spawnScale;

		bridge = _bridge;
		
		bridgeBlueprint = blueprint;
		
		startX = Input.mousePosition.x;
		startY = Input.mousePosition.y;

		lastSpawn = bridge;
		startSize = lastSpawn.transform.localScale.x;
		startRotation = lastSpawn.transform.localRotation.z;

		buildManager.Building = true;
		buildManager.Console("SCALE MODE!", new Color(1f, 1f, 1f, 1f));
	}

	public void BuildBridge(BridgeBlueprint blueprint, NodeSupport nodeSupport)
	{
		Vector3 spawnSpot = nodeSupport.transform.position;
		Quaternion spawnRotation = nodeSupport.transform.rotation;
		
		if (PlayerStats.Money < blueprint.cost)
		{
			buildManager.Console("NOT ENOUGH MONEY TO BUILD THAT!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
			Debug.Log("Not enough money to build that!");
			return;
		}

		cost = blueprint.cost;

		GameObject _bridge = Instantiate(blueprint.prefab, spawnSpot, spawnRotation);

		parent = nodeSupport.transform;

		_bridge.transform.localScale = spawnScale;

		bridge = _bridge;

		bridgeBlueprint = blueprint;

		startX = Input.mousePosition.x;
		startY = Input.mousePosition.y;

		lastSpawn = bridge;
		startSize = lastSpawn.transform.localScale.x;
		startRotation = lastSpawn.transform.localRotation.z;

		buildManager.Building = true;
		buildManager.Console("SCALE MODE!", new Color(1f, 1f, 1f, 1f));
	}

	public void SellBridge()
	{
		PlayerStats.Money += totalCost;

		foreach (GameObject bridge in bridges)
		{
			if (bridge.transform.childCount > 1)
			{
				if (bridge.transform.GetChild(1).childCount > 0)
				{
					if (bridge.transform.GetChild(1).transform.tag == "SupportPlatform")
					{
						bridge.transform.GetChild(1).transform.parent = GameObject.FindGameObjectWithTag("Enviroment").transform;
					}
					else
					{
						bridge.transform.GetChild(1).transform.parent = null;
					}
				}
			}

			if (bridge.transform.parent != null)
			{
				if (bridge.transform.parent.parent == null)
				{
					Destroy(bridge.transform.parent.gameObject);
				}
			}

			Destroy(bridge);
		}

		GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		bridgeBlueprint = null;
		bridges.Clear();
		totalCost = 0;
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		if (!buildManager.CanBuild) {
			return;
		}
        
		if (buildManager.HasMoney) {
			rend.material.color = hoverColor;
		} else {
			rend.material.color = notEnoughMoneyColor;
		}
        
	}

	void OnMouseExit ()
	{
		rend.material.color = startColor;
	}
}
