using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public CameraSwitcher cameraSwitcher;

	public Color hoverColor;
	public Color notEnoughMoneyColor;

	[HideInInspector]
	public GameObject bridge;
	[HideInInspector]
	public BridgeBlueprint bridgeBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;
	[HideInInspector]
	public Transform parent;

	public GameObject nodePrefab;

	private Renderer rend;
	private Color startColor;

	public Vector3 spawnScale = new Vector3(1.0f, 1.0f, 1.0f);

	public float sizingFactor = 0.02f;
	public float rotatingFactor = 0.2f;
	public float maxSize = 1.5f;
	public float minSize = 0.8f;
	public float maxRot = 30;
	public float minRot = -30;

	private GameObject lastSpawn = null;

	private bool scaling = true;
	private bool rotating = false;

	private float startSize;
	private float startRotation;
	private float startX;
	private float startY;

	private int cost;

	BuildManager buildManager;

	// Use this for initialization
	void Start ()
	{
		cameraSwitcher = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraSwitcher>();

		rend = GetComponent<Renderer> ();
		startColor = rend.material.color;

		bridge = null;
		lastSpawn = null;

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
				lastSpawn.transform.parent = parent;
				Debug.Log(bridge.gameObject.transform.parent.parent.tag);
				if (bridge.gameObject.transform.parent.parent.tag == "Track2")
				{
					lastSpawn.GetComponentInChildren<HingeJoint>().breakTorque = 1000;
					lastSpawn.GetComponentInChildren<HingeJoint>().breakForce = Mathf.Infinity;
				}
				lastSpawn.GetComponentInChildren<HingeJoint>().connectedBody = bridge.gameObject.transform.parent.parent.GetChild(0).GetComponent<Rigidbody>();
				bridge.transform.GetChild(1).GetComponent<Collider>().isTrigger = false;
				SetDefaults();
				GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
				Destroy(effect, 5f);
				Debug.Log("Bridge built!");
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

		if (bridge != null)
		{
			buildManager.SelectNode(this);
			return;
		}

		if (buildManager.GetBridgeToBuild() != null)
		{
			if (buildManager.GetBridgeToBuild().prefab.name != "WoodenBridge" && buildManager.GetBridgeToBuild().prefab.name != "SteelBridge")
			{
				buildManager.Console("SUPPORTS CAN'T BE BUILT THERE!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
				Debug.Log("Supports can't be built there!");
				buildManager.DeselectNodeSupport();
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

	void BuildBridge (BridgeBlueprint blueprint, RaycastHit hitInfo)
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

	public void UpgradeBridge ()
	{
		if (PlayerStats.Money < bridgeBlueprint.upgradeCost) {
			buildManager.Console("NOT ENOUGH MONEY TO UPGRADE THAT!", new Color(0.875f, 0.180f, 0.180f, 0.538f));
			Debug.Log ("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= bridgeBlueprint.upgradeCost;

		Vector3 lastPosition = bridge.transform.position;
		Vector3 lastScale = bridge.transform.localScale;
		Quaternion lastRotation = bridge.transform.rotation;

		Transform parent = null;
		Transform child = null;

		if (bridge.transform.parent != null) {
			parent = bridge.transform.parent;
		}

		if(bridge.transform.childCount > 1)
		{
			if (bridge.transform.GetChild(1).childCount > 0)
			{
				child = bridge.transform.GetChild(1);
				child.transform.parent = null;
			}
		}

		Destroy (bridge);

		GameObject _bridge = Instantiate(bridgeBlueprint.upgradedPrefab, lastPosition, lastRotation, parent.transform);

		_bridge.transform.localScale = lastScale;

		if (child != null) {
			Destroy (_bridge.transform.GetChild (1).gameObject);
			child.transform.parent = _bridge.transform;
			child.GetComponentInChildren<HingeJoint>().connectedBody = _bridge.gameObject.transform.GetComponentInChildren<Rigidbody>();
		}

		bridge = _bridge;

		if (bridge.transform.parent.parent != null)
		{
			bridge.GetComponentInChildren<HingeJoint>().connectedBody = bridge.gameObject.transform.parent.parent.GetChild(0).GetComponent<Rigidbody>();
		}
		
		GameObject effect = Instantiate (buildManager.buildEffect, GetBuildPosition (), Quaternion.identity);
		Destroy (effect, 5f);

		isUpgraded = true;

		Debug.Log ("Bridge upgraded!");
	}

	public void SellBridge ()
	{
		PlayerStats.Money += bridgeBlueprint.GetSellAmount (isUpgraded);

		if(bridge.transform.childCount > 1)
		{
			if (bridge.transform.GetChild(1).childCount > 0)
			{
				if (bridge.transform.GetChild(1).transform.tag == "Track2")
				{
					if (bridge.transform.GetChild(1).transform.name == "Track(4)")
					{
						bridge.transform.GetChild(1).transform.parent = GameObject.Find("Tracks(1)").transform;
					}
					else if (bridge.transform.GetChild(1).transform.name == "Track(2)")
					{
						bridge.transform.GetChild(1).transform.parent = GameObject.Find("Tracks").transform;
					}
				}
				else
				{
					bridge.transform.GetChild(1).transform.parent = null;
				}
			}
		}

		GameObject effect = Instantiate (buildManager.sellEffect, GetBuildPosition (), Quaternion.identity);
		Destroy (effect, 5f);

		if (bridge.transform.parent != null)
		{
			if (bridge.transform.parent.parent == null)
			{
				Destroy(bridge.transform.parent.gameObject);
			}
		}

		Destroy (bridge);
		bridgeBlueprint = null;
		isUpgraded = false;
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
