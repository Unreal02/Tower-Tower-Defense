using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStatus : MonoBehaviour
{
    private Tower selectedTower;
    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedTower(Tower t)
    {
        selectedTower = t;
    }

    public void SetTowerStatusUI(bool b)
    {
        child.SetActive(b);
    }
}
