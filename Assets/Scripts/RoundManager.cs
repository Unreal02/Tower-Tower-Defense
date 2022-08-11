using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// 라운드를 관리합니다.
public class RoundManager : MonoBehaviour
{
    public UnityEvent onGameWin;

    [Serializable]
    public class Spawn
    {
        public Spawn(float t, string e)
        {
            time = t;
            enemy = e;
            enemyObject = Resources.Load<GameObject>("Prefabs/Enemies/" + enemy);
        }

        public float time;
        public string enemy;
        public GameObject enemyObject;
    }

    private List<List<Spawn>> roundInfo;

    private int next = 0;
    private int currentRound = 1;
    private int maxRound;
    private float currentTime = 0;
    private bool onRound = false;
    private Text text;

    void Start()
    {
        roundInfo = new List<List<Spawn>>();
        text = GameObject.Find("Next Round Button").GetComponentInChildren<Text>();

        // CSV 읽기
        List<List<string>> csv = CSVReader.Read("RoundInfo");
        csv.RemoveAt(0);
        roundInfo.Add(new List<Spawn>());
        foreach (List<string> list in csv)
        {
            // list: (level, time, idx)
            if (list.Count != 4) continue;
            int round = int.Parse(list[0]);
            float time = float.Parse(list[1]);
            int enemyIdx = int.Parse(list[2]);
            string enemyName = "Enemy" + enemyIdx.ToString();

            if (roundInfo.Count <= round)
            {
                roundInfo.Add(new List<Spawn>());
            }
            roundInfo[round].Add(new Spawn(time, enemyName));
            if (maxRound < round) maxRound = round;
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
                    if (currentRound > maxRound)
                    {
                        onGameWin.Invoke();
                    }
                }
            }
            text.text = "라운드 진행 중";
        }
        else text.text = "라운드 시작";
    }

    public void OnClickNextRound()
    {
        if (currentRound >= roundInfo.Count) return;
        if (onRound) return;
        currentTime = 0;
        next = 0;
        onRound = true;
    }

    public int GetCurrentRound() { return currentRound; }

    public int GetMaxRound() { return maxRound; }
}
