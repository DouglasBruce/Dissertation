using UnityEngine;
using UnityEngine.UI;

public class NodeSupportUI : MonoBehaviour {

    public GameObject UI;

	public Text buildText;
	public Text sellText;

	private int cost;

    private NodeSupport target;

	BuildManager buildManager;

	void Start()
	{
		buildManager = BuildManager.instance;
	}

	public void SetTarget (NodeSupport _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

		cost = target.bridgeBlueprint.GetSellAmount(false);

		cost = cost * target.bridges.Count;

		buildText.text = "£" + buildManager.GetBridgeToBuild().cost;

		sellText.text = "£" + target.totalCost;

        UI.SetActive(true);
    }

	public void Build()
	{
		buildManager.DeselectNodeSupport();
		target.BuildBridge(buildManager.GetBridgeToBuild(), target);
	}

    public void Hide ()
    {
        UI.SetActive(false);
    }

    public void Sell ()
    {
        target.SellBridge();
		buildManager.DeselectNodeSupport();
    }
}
