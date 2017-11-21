using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance;

	public GameObject buildEffect;
	public GameObject sellEffect;

	private BridgeBlueprint bridgeToBuild;

	[HideInInspector]
	public TrainBlueprint trainBlueprint;

	[HideInInspector]
	public bool isTrainUpgraded = false;

	public Text consoleText;
	public GameObject console;

	private Node selectedNode;
	private NodeSupport selectedNodeSupport;

	public NodeUI nodeUI;
	public NodeSupportUI nodeSupportUI;
	public TrainUI trainUI;

	public bool CanBuild { get { return bridgeToBuild != null; } }
	public bool HasMoney { get { return PlayerStats.Money >= bridgeToBuild.cost; } }

	[HideInInspector]
	public bool Building = false;

	void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in the scene!");
            return;
        }
        instance = this;
    }

	public void Console (string text, Color color)
	{
		consoleText.text = text;
		consoleText.color = color;
		console.SetActive(true);
		StartCoroutine(DeactivateConsole ());
	}

	IEnumerator DeactivateConsole ()
	{
		yield return new WaitForSeconds(2.01f);
		console.SetActive(false);
		yield return null;
	}

	public void SelectNode (Node node)
    {
		trainUI.Hide();
		DeselectNodeSupport();

		if (selectedNode == node)
        {
            DeselectNode();
			return;
        }

        selectedNode = node;
        bridgeToBuild = null;

        nodeUI.SetTarget(node);
    }

	public void SelectNodeSupport(NodeSupport nodeSupport)
	{
		trainUI.Hide();
		DeselectNode();

		if (selectedNodeSupport == nodeSupport)
		{
			DeselectNodeSupport();
			return;
		}

		selectedNodeSupport = nodeSupport;

		nodeSupportUI.SetTarget(nodeSupport);
	}

	public void DeselectNode ()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

	public void DeselectNodeSupport()
	{
		selectedNodeSupport = null;
		nodeSupportUI.Hide();
	}
	
	public void SelectBridgeToBuild(BridgeBlueprint bridge)
    {
        bridgeToBuild = bridge;
        DeselectNode();
		DeselectNodeSupport();
	}

    public BridgeBlueprint GetBridgeToBuild ()
    {
        return bridgeToBuild;
    }
}
