//------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISpace : UIManager
{
    public Button upgradeButton;
    public Button explorationButton;

    void Start()
    {
        upgradeButton.onClick.AddListener(() => GameManager.Instance.UpgradeWeapon());

        explorationButton.onClick.AddListener(() => GameManager.Instance.SendExplorationCommand());
    }


}