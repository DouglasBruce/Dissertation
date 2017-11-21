using UnityEngine;

public class Shop : MonoBehaviour {

    public BridgeBlueprint woodenBridge;
	public BridgeBlueprint woodenSupport;
	public BridgeBlueprint steelBridge;
	public BridgeBlueprint steelSupport;

	BuildManager buildManager;

	private Color white = new Color(1f, 1f, 1f, 1f);

    // Use this for initialization
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectWoodenBridge()
    {
		buildManager.Console("WOODEN BRIDGE SELECTED!", white);
		Debug.Log("Wooden Bridge Selected");
        buildManager.SelectBridgeToBuild(woodenBridge);
    }

	public void SelectWoodenSupport()
	{

		buildManager.Console("WOODEN SUPPORT SELECTED!", white);
		buildManager.SelectBridgeToBuild(woodenSupport);
	}

	public void SelectSteelBridge()
    {
		buildManager.Console("STEEL BRIDGE SELECTED!", white);
		Debug.Log("Steel Bridge Selected");
        buildManager.SelectBridgeToBuild(steelBridge);
    }

	public void SelectSteelSupport()
	{
		buildManager.Console("STEEL SUPPORT SELECTED!", white);
		Debug.Log("Steel Support Selected");
		buildManager.SelectBridgeToBuild(steelSupport);
	}
}
