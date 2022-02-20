using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatus : MonoBehaviour
{
    private Tower selectedTower;
    private GameObject child;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        text = transform.GetChild(0).GetComponentInChildren<Text>();
        child.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSelectedTower(Tower t)
    {
        selectedTower = t;
        if (selectedTower)
            text.text = string.Format("공격력 {0}\n사정거리 {1}\n공격 시간 {2}", selectedTower.GetDamage(), selectedTower.GetRadius(), selectedTower.GetDelay());
    }

    public void SetTowerStatusUI(bool b)
    {
        child.SetActive(b);
    }
}
