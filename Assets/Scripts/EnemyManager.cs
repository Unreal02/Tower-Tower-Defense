using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private HashSet<Enemy> enemySet;

    // Start is called before the first frame update
    void Start()
    {
        enemySet = new HashSet<Enemy>();
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
