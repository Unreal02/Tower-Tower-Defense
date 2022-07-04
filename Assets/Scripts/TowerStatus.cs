using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatus : MonoBehaviour
{
    private Tower selectedTower;
    private GameObject statusUI;
    private PlayerInfo playerInfo;
    private GameObject synergyStatusSet;
    public Text statusText;
    public Text upgradeText;

    // Start is called before the first frame update
    void Start()
    {
        statusUI = transform.GetChild(0).gameObject;
        synergyStatusSet = transform.GetChild(1).gameObject;
        statusText = transform.GetChild(0).GetComponentInChildren<Text>();
        SetTowerStatusUI(false);
        upgradeText = statusUI.transform.GetChild(1).GetComponentInChildren<Text>();
        playerInfo = FindObjectOfType<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSelectedTower(Tower t)
    {
        selectedTower = t;
        if (selectedTower)
        {
            UpdateTowerStatus();
        }
    }

    public void SetTowerStatusUI(bool b)
    {
        statusUI.SetActive(b);
        synergyStatusSet.SetActive(b);
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

        synergyStatusSet.GetComponent<SynergyStatusSet>().UpdateSynergyStatusSet(selectedTower.GetStackedTower());
    }

    public void OnClickTowerUpgradeButton()
    {
        if (selectedTower == null || selectedTower.GetLevel() == 4)
        {
            return;
        }

        if (playerInfo.GetMoney() >= selectedTower.GetNextCost())
        {
            playerInfo.SubtractMoney(selectedTower.GetNextCost());
            selectedTower.AddLevel();
            UpdateTowerStatus();
        }
    }
}
