using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 블록 오브젝트입니다.
public class Block : MonoBehaviour
{
    private TowerManager towerManager;

    // Start is called before the first frame update
    void Start()
    {
        towerManager = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton() {
        towerManager.OnClickInstallTower();
    }
}
