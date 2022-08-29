using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHelpScreen : MonoBehaviour
{
    private EnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        Transform hpSet = transform.GetChild(3);
        Transform speedSet = transform.GetChild(4);
        Transform moneySet = transform.GetChild(5);

        int i = 1;
        foreach (var pair in enemyManager.enemyInfo)
        {
            int idx = pair.Key;
            EnemyManager.EnemyData enemyData = pair.Value;
            hpSet.GetChild(i).GetComponent<Text>().text = enemyData.hp.ToString();
            speedSet.GetChild(i).GetComponent<Text>().text = enemyData.speed.ToString();
            moneySet.GetChild(i).GetComponent<Text>().text = enemyData.money.ToString();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
