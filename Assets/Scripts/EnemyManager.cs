using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Serializable]
    public class EnemyData
    {
        public int hp; // 초기 체력
        public float speed; // 이동 속도
        public int money; // 제거하면 주는 자금
    }

    public Dictionary<int, EnemyData> enemyInfo;

    private HashSet<Enemy> enemySet;

    // Start is called before the first frame update
    void Start()
    {
        enemySet = new HashSet<Enemy>();

        enemyInfo = new Dictionary<int, EnemyData>();
        List<List<string>> csv = CSVReader.Read("EnemyInfo");
        csv.RemoveAt(0);
        foreach (List<string> list in csv)
        {
            if (list.Count != 5) continue;
            EnemyData data = new EnemyData();
            data.hp = int.Parse(list[1]);
            data.speed = float.Parse(list[2]);
            data.money = int.Parse(list[3]);
            enemyInfo.Add(int.Parse(list[0]), data);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public HashSet<Enemy> GetEnemySet() { return enemySet; }

    public void AddEnemy(Enemy e)
    {
        enemySet.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        if (!enemySet.Remove(e))
        {
            Debug.Log("enemy doesn't exist in enemySet");
        }
    }
}
