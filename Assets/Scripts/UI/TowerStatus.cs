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
            return mouseCursor.GetCurrentTower()?.GetComponent<Tower>();
        }
    }
    private GameObject statusUI;
    private PlayerInfo playerInfo;
    private GameObject synergyStatusSet;
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
        if (b) UpdateTowerStatus();
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

        // todo: 쌓인 타워들 함께 매각하기
        if (selectedTower.GetUpperTower() != null) return;

        if (playerInfo.GetMoney() >= selectedTower.GetSellCost())
        {
            playerInfo.AddMoney(selectedTower.GetSellCost());
            selectedTower.OnSell();
            Destroy(selectedTower.gameObject);
            mouseCursor.OnSellTower();
        }
    }
}
