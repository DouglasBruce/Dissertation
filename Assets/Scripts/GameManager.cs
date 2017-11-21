using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public Timer timer;

	public Button play;
	public Button sellAll;
	public Button reset;
	public Button run;
	public Button edit;

	public static bool GameIsOver;
	public static bool GameInPlay;
	public static bool Edit;

	public GameObject shopUI;
	public GameObject gameOverUI;
	public GameObject completeLevelUI;

	public TrainUI trainUI;

	public GameObject trainForcesUI;
	public GameObject trainForcesUIBehind;
	public GameObject trainForcesUITop;
	public GameObject gravityUI;
	public GameObject trainStatsUI;
	public Text trainForcesUIText;
	public bool forcesOn = true;

	public CameraSwitcher cameraSwitcher;
	BuildManager buildManager;

	[HideInInspector]
	public GameObject[] nodes;
	[HideInInspector]
	public GameObject[] bridges;
	[HideInInspector]
	public GameObject[] components;
	[HideInInspector]
	public List<Vector3> positions;
	[HideInInspector]
	public List<Quaternion> rotations;

	private int startMoney;

	private bool running = false;

	// Use this for initialization
	void Start ()
	{
		startMoney = PlayerStats.Money;
		GameIsOver = false;
		GameInPlay = false;
		Edit = true;
		buildManager = BuildManager.instance;
	}

	// Update is called once per frame
	void Update ()
	{
		if (PlayerStats.Money == startMoney || buildManager.isTrainUpgraded && PlayerStats.Money == startMoney - 200)
		{
			play.interactable = false;
			sellAll.interactable = false;
		} 
		else
		{
			play.interactable = true;
			sellAll.interactable = true;
		}

		if (running)
		{
			run.interactable = false;
		}
		else
		{
			run.interactable = true;
		}

		if (GameIsOver) {
			return;
		}

		if (PlayerStats.IsDead) {
			EndGame ();
		}
	}

	public void ToggleForces ()
	{
		forcesOn = !forcesOn;
		if (forcesOn)
		{
			if (cameraSwitcher.mainCameraActive)
			{
				trainForcesUI.GetComponent<CanvasGroup>().alpha = 1f;
			}
			else if (cameraSwitcher.thirdCameraActive)
			{
				trainForcesUITop.GetComponent<CanvasGroup>().alpha = 1f;
			}
			else
			{
				trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 1f;
			}
			
			gravityUI.SetActive(true);
			trainStatsUI.GetComponent<CanvasGroup>().alpha = 1f;
			trainForcesUIText.text = "FORCES OFF";
		}
		else
		{
			trainForcesUI.GetComponent<CanvasGroup>().alpha = 0f;
			trainForcesUIBehind.GetComponent<CanvasGroup>().alpha = 0f;
			trainForcesUITop.GetComponent<CanvasGroup>().alpha = 0f;
			gravityUI.SetActive(false);
			trainStatsUI.GetComponent<CanvasGroup>().alpha = 0f;
			trainForcesUIText.text = "FORCES ON";
		}
	}

	public void SellAll ()
	{
		buildManager.DeselectNode();
		buildManager.DeselectNodeSupport();
		trainUI.Hide();
		components = null;
		bridges = null;
		nodes = null;
		components = GameObject.FindGameObjectsWithTag("Bridge");
		positions.Clear();
		rotations.Clear();

		foreach (GameObject component in components)
		{
			if (component.transform.parent != null)
			{
				if (component.name == "WoodenBridge(Clone)" || component.name == "UpgradedWoodenBridge(Clone)" || component.name == "SteelBridge(Clone)" || component.name == "UpgradedSteelBridge(Clone)")
				{
					Node temp = component.GetComponentInParent<Node>();
					temp.bridge = null;
					temp.isUpgraded = false;
				}
				else if (component.name == "WoodenSupport(Clone)" || component.name == "SteelSupport(Clone)")
				{
					NodeSupport temp = component.GetComponentInParent<NodeSupport>();
					temp.bridges.Clear();
					temp.totalCost = 0;
				}
			}

			if (component.transform.childCount > 1)
			{
				if (component.transform.GetChild(1).tag == "Track2")
				{
					if (component.transform.GetChild(1).transform.name == "Track(4)")
					{
						component.transform.GetChild(1).transform.parent = GameObject.Find("Tracks(1)").transform;
					}
					else if (component.transform.GetChild(1).transform.name == "Track(2)")
					{
						component.transform.GetChild(1).transform.parent = GameObject.Find("Tracks").transform;
					}
				}
				else if (component.transform.GetChild(1).tag == "SupportPlatform")
				{
					component.transform.GetChild(1).transform.parent = GameObject.FindGameObjectWithTag("Enviroment").transform;
				}
			}
			Destroy(component);
		}

		if (buildManager.isTrainUpgraded)
		{
			PlayerStats.Money = startMoney - 200;
		}
		else
		{
			PlayerStats.Money = startMoney;
		}
	}

	public void DisableButtons()
	{
		running = true;
		edit.interactable = false;
		reset.interactable = false;
	}

	public void StartGame ()
	{
		shopUI.SetActive(false);
		buildManager.DeselectNode();
		buildManager.DeselectNodeSupport();
		trainUI.Hide();

		bridges = null;
		bridges = GameObject.FindGameObjectsWithTag("Bridge");

		Rigidbody rb;

		positions.Clear();
		rotations.Clear();

		foreach (GameObject bridge in bridges)
		{
			positions.Add(bridge.gameObject.transform.GetChild(0).transform.position);
			rotations.Add(bridge.gameObject.transform.GetChild(0).transform.rotation);

			rb = bridge.gameObject.GetComponentInChildren<Rigidbody>();
			rb.isKinematic = false;
		}

		nodes = null;
		nodes = GameObject.FindGameObjectsWithTag("Node");

		foreach (GameObject node in nodes)
		{
			node.GetComponent<Renderer>().enabled = false;
			node.GetComponent<Collider>().enabled = false;
		}

		play.gameObject.SetActive(false);
		sellAll.gameObject.SetActive(false);
		reset.gameObject.SetActive(true);
		run.gameObject.SetActive(true);
		edit.gameObject.SetActive(true);
		Edit = false;
		timer.pause = true;
	}

	public void StartTrain()
	{
		GameInPlay = true;
		running = true;
	}

	public void ResetGame()
	{
		GameInPlay = false;
		running = false;
		
		int i = 0;
		foreach (GameObject bridge in bridges)
		{
			bridge.gameObject.transform.GetChild(0).transform.position = positions[i];
			bridge.gameObject.transform.GetChild(0).transform.rotation = rotations[i];
			i++;
		}
	}

	public void EditMode()
	{
		if (cameraSwitcher.mainCameraActive)
		{
			shopUI.SetActive(true);
		}

		Rigidbody rb;

		int i = 0;
		foreach (GameObject bridge in bridges)
		{
			rb = bridge.gameObject.GetComponentInChildren<Rigidbody>();
			rb.isKinematic = true;

			bridge.gameObject.transform.GetChild(0).transform.position = positions[i];
			bridge.gameObject.transform.GetChild(0).transform.rotation = rotations[i];
			i++;
		}

		foreach (GameObject node in nodes)
		{
			node.GetComponent<Renderer>().enabled = true;
			node.GetComponent<Collider>().enabled = true;
		}

		play.gameObject.SetActive(true);
		sellAll.gameObject.SetActive(true);
		reset.gameObject.SetActive(false);
		run.gameObject.SetActive(false);
		edit.gameObject.SetActive(false);
		GameInPlay = false;
		running = false;
		Edit = true;
		timer.pause = false;
	}

	void EndGame ()
	{
		GameIsOver = true;
		gameOverUI.SetActive (true);
	}

	public void WinLevel ()
	{
		GameInPlay = false;
		GameIsOver = true;
		completeLevelUI.SetActive (true);
	}
}
