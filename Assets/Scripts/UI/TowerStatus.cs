using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatus : MonoBehaviour
{
    private MouseCursor mouseCursor;
    private Tower selectedTower
    {
        get
        {
            return mouseCursor.GetSelectedTower()?.GetComponent<Tower>();
        }
    }
    private GameObject statusUI;
    private PlayerInfo playerInfo;
    private GameObject synergyStatusSet;
    private GameObject stackedSell;
    private GameObject towerRotateButton;
    private Text statusText;
    private Text upgradeText;
    private Text sellText;

    // Start is called before the first frame update
    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
        statusUI = transform.GetChild(0).gameObject;
        playerInfo = FindObjectOfType<PlayerInfo>();
        synergyStatusSet = transform.GetChild(1).gameObject;
        stackedSell = transform.GetChild(2).gameObject;
        towerRotateButton = transform.GetChild(3).gameObject;
        statusText = transform.GetChild(0).GetComponentInChildren<Text>();
        upgradeText = statusUI.transform.GetChild(1).GetComponentInChildren<Text>();
        sellText = statusUI.transform.GetChild(2).GetComponentInChildren<Text>();
        SetTowerStatusUI(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTowerStatusUI(bool b)
    {
        statusUI.SetActive(b);
        synergyStatusSet.SetActive(b);
        stackedSell.SetActive(false);
        if (b)
        {
            towerRotateButton.SetActive(selectedTower.GetType() == typeof(TowerStraight));
            UpdateTowerStatus();
        }
        else
        {
            towerRotateButton.SetActive(false);
        }
    }

    public void UpdateTowerStatus()
    {
        statusText.text = string.Format("공격력 {0}\n사정거리 {1}\n공격 시간 {2}", selectedTower.GetDamage(), selectedTower.GetRadius(), selectedTower.GetDelay());
        int level = selectedTower.GetLevel();
        if (level < 4)
        {
            upgradeText.text = string.Format("레벨 {0} -> {1}\n{2}", level, level + 1, selectedTower.GetNextCost());
        }
        else
        {
            upgradeText.text = string.Format("레벨 {0}\n최대 레벨", level);
        }

        sellText.text = string.Format("매각\n{0}", selectedTower.GetSellCost());

        synergyStatusSet.GetComponent<SynergyStatusSet>().UpdateSynergyStatusSet(selectedTower.idx, selectedTower.GetStackedTower());
    }

    public void OnClickTowerUpgradeButton()
    {
        if (selectedTower == null || selectedTower.GetLevel() == 4) return;

        if (playerInfo.GetMoney() >= selectedTower.GetNextCost())
        {
            playerInfo.SubtractMoney(selectedTower.GetNextCost());
            selectedTower.AddLevel();
            UpdateTowerStatus();
        }
    }

    public void OnClickTowerSellButton()
    {
        if (selectedTower == null) return;

        if (selectedTower.GetUpperTower() != null)
        {
            // 위에 쌓인 타워들이 있는 경우
            stackedSell.SetActive(true);
        }
        else
        {
            // 위에 쌓인 타워들이 없는 경우
            Sell(selectedTower);
        }
    }

    public void OnClickTowerRotateButton()
    {
        if (selectedTower == null) return;

        ((TowerStraight)selectedTower).Rotate();
    }

    private void Sell(Tower tower)
    {
        playerInfo.AddMoney(tower.GetSellCost());
        tower.OnSell();
        Destroy(tower.gameObject);
        mouseCursor.OnSellTower();
        stackedSell.SetActive(false);
    }

    public void StackedSell() { StackedSell(selectedTower); }
    public void StackedSell(Tower tower)
    {
        // 위에 쌓인 타워를 먼저 매각
        if (tower.GetUpperTower() != null) StackedSell(tower.GetUpperTower());
        Sell(tower);
    }
}
