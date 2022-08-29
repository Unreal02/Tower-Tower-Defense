using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHelpScreen : MonoBehaviour
{
    private int selectedTowerIdx = 0;
    private TowerManager towerManager;
    private Transform towerDetailButtonSet;
    private Transform towerDetailSet;
    private GameObject[] towerDetail;

    // Start is called before the first frame update
    void Start()
    {
        towerManager = FindObjectOfType<TowerManager>();
        towerDetailButtonSet = transform.GetChild(2);
        foreach (var i in towerManager.towerInfo.Keys)
        {
            towerDetailButtonSet.GetChild(i - 1).GetComponent<Button>().onClick.AddListener(() => OnClickTowerDetailButton(i));
        }

        towerDetailSet = transform.GetChild(3);
        towerDetail = new GameObject[towerManager.towerInfo.Count + 1];
        towerDetail[0] = towerDetailSet.GetChild(0).gameObject;
        foreach (var idx in towerManager.towerInfo.Keys)
        {
            towerDetail[idx] = towerDetailSet.GetChild(idx).gameObject;
            Transform level = towerDetail[idx].transform.GetChild(3);
            Transform cost = towerDetail[idx].transform.GetChild(4);
            Transform damage = towerDetail[idx].transform.GetChild(5);
            Transform radius = towerDetail[idx].transform.GetChild(6);
            Transform delay = towerDetail[idx].transform.GetChild(7);
            for (int lv = 0; lv < 5; lv++)
            {
                level.GetChild(lv).GetComponent<Text>().text = (lv + 1).ToString();
                cost.GetChild(lv).GetComponent<Text>().text = towerManager.towerInfo[idx].cost[lv].ToString();
                damage.GetChild(lv).GetComponent<Text>().text = towerManager.towerInfo[idx].damage[lv].ToString();
                radius.GetChild(lv).GetComponent<Text>().text = towerManager.towerInfo[idx].radius[lv].ToString();
                delay.GetChild(lv).GetComponent<Text>().text = towerManager.towerInfo[idx].delay[lv].ToString();
            }
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        if (towerDetail != null)
        {
            OnClickTowerDetailButton(0);
        }
    }

    private void OnClickTowerDetailButton(int idx)
    {
        selectedTowerIdx = idx;
        foreach (GameObject o in towerDetail) o.SetActive(false);
        towerDetail[idx].SetActive(true);
    }
}
