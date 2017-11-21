using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

    public GameObject UI;

    public Text upgradeText;
    public Text upgradeCost;
    public Button upgradeButton;

    public Text sellText;

    private Node target;

    public void SetTarget (Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        if (!target.isUpgraded)
        {
            upgradeText.text = "UPGRADE";
            upgradeCost.text = "£" + target.bridgeBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeText.text = "MAX";
            upgradeCost.text = "LEVEL";
            upgradeButton.interactable = false;
        }

        sellText.text = "£" + target.bridgeBlueprint.GetSellAmount(target.isUpgraded);

        UI.SetActive(true);
    }

    public void Hide ()
    {
        UI.SetActive(false);
    }

    public void Upgrade ()
    {
        target.UpgradeBridge();
        BuildManager.instance.DeselectNode();
    }

    public void Sell ()
    {
        target.SellBridge();
        BuildManager.instance.DeselectNode();
    }
}
