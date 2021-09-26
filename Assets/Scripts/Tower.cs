using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타워 오브젝트입니다.
// TowerManager에서 타워 설치하기 전에는 비활성화 상태입니다.
// 타워를 설치하는 순간 활성화됩니다.
public class Tower : MonoBehaviour
{
    public float radius;

    private Component radiusSphere;
    private TowerManager towerManager;

    // Start is called before the first frame update
    void Start()
    {
        radiusSphere = transform.GetChild(1);
        radiusSphere.transform.localScale = new Vector3(radius, radius, radius) * 2;
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
