using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TowerManager towerManager;
    private GameObject towerInfo;
    private Text towerInfoText;
    private MouseCursor mouseCursor;
    private Button button;
    public GameObject tower;
    private Tower towerComponent;

    // Start is called before the first frame update
    void Start()
    {
        towerManager = FindObjectOfType<TowerManager>();
        towerInfo = GameObject.Find("Tower Button Set").transform.GetChild(0).gameObject;
        towerInfoText = towerInfo.GetComponentInChildren<Text>();
        mouseCursor = FindObjectOfType<MouseCursor>();
        button = GetComponent<Button>();
        towerComponent = tower.GetComponent<Tower>();

        button.onClick.AddListener(() => mouseCursor.OnClickTowerButton(tower));

        towerInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        towerInfo.SetActive(true);
        towerInfoText.text = string.Format("{0}\n{1}", towerManager.towerInfo[towerComponent.idx].towerName, towerManager.towerInfo[towerComponent.idx].cost[0]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        towerInfo.SetActive(false);
    }
}