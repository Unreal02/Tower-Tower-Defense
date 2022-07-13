using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 라운드를 관리합니다.
public class RoundManager : MonoBehaviour
{

    [Serializable]
    public class Spawn
    {
        public Spawn(float t, string e)
        {
            time = t;
            enemy = e;
            enemyObject = Resources.Load<GameObject>("Objects/Enemies/" + enemy);
        }

        public float time;
        public string enemy;
        public GameObject enemyObject;
    }

    private List<List<Spawn>> roundInfo;

    private int next = 0;
    private int currentRound = 1;
    private float currentTime = 0;
    private bool onRound = false;
    private Text text;

    void Start()
    {
        roundInfo = new List<List<Spawn>>();
        text = GameObject.Find("Next Round Button").GetComponentInChildren<Text>();

        // CSV 읽기
        List<List<string>> csv = CSVReader.Read("RoundInfo");
        int currRound = 0;
        roundInfo.Add(new List<Spawn>());
        foreach (List<string> list in csv.GetRange(1, csv.Count - 1))
        {
            // list: (level, ID, amount)
            if (list.Count != 3) continue;
            int round;
            if (int.TryParse(list[0], out round))
            {
                currRound = round;
            }
            float time = float.Parse(list[1]);
            int enemyIdx = int.Parse(list[2]);
            string enemyName = "Enemy" + enemyIdx.ToString();

            if (roundInfo.Count <= currRound)
            {
                roundInfo.Add(new List<Spawn>());
            }
            roundInfo[currRound].Add(new Spawn(time, enemyName));
        }
    }

    void Update()
    {
        if (onRound)
        {
            currentTime += Time.deltaTime;
            if (next < roundInfo[currentRound].Count && currentTime >= roundInfo[currentRound][next].time)
            {
                Instantiate(roundInfo[currentRound][next].enemyObject, Vector3.zero, Quaternion.identity, transform);
                next++;
            }
            else if (next >= roundInfo[currentRound].Count)
            {
                if (transform.childCount == 0)
                {
                    onRound = false;
                    currentRound++;
                }
            }
            text.text = "라운드 진행 중";
        }
        else text.text = "다음 라운드";
    }

    public void OnClickNextRound()
    {
        if (currentRound >= roundInfo.Count) return;
        if (onRound) return;
        currentTime = 0;
        next = 0;
        onRound = true;
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }
}
