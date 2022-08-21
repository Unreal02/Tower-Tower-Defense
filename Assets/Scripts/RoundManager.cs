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
        public Spawn(float t, int i, string e)
        {
            time = t;
            enemyIdx = i;
            enemy = e;
            enemyObject = Resources.Load<GameObject>("Prefabs/Enemies/" + enemy);
        }

        public float time;
        public int enemyIdx;
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

    private Dictionary<int, Quaternion> enemyRotation;
    private int[] enemyIdxList = { 101, 102, 103, 301, 302, 303, 401, 402, 403 };

    void Start()
    {
        roundInfo = new List<List<Spawn>>();
        text = GameObject.Find("Next Round Button").GetComponentInChildren<Text>();

        enemyRotation = new Dictionary<int, Quaternion>();

        // CSV 읽기
        List<List<string>> csv = CSVReader.Read("RoundInfo");
        csv.RemoveAt(0);
        roundInfo.Add(new List<Spawn>());
        foreach (List<string> list in csv)
        {
            // list: (level, time, idx, amount, interval)
            if (list.Count != 5) continue;
            int round = int.Parse(list[0]);
            float time = float.Parse(list[1]);
            int enemyIdx = int.Parse(list[2]);
            string enemyName = "Enemy" + enemyIdx.ToString();
            int amount = int.Parse(list[3]);
            float interval = float.Parse(list[4]);

            if (roundInfo.Count <= round)
            {
                roundInfo.Add(new List<Spawn>());
            }
            for (int i = 0; i < amount; i++) roundInfo[round].Add(new Spawn(time + i * interval, enemyIdx, enemyName));
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
                GameObject enemyObject = Instantiate(roundInfo[currentRound][next].enemyObject, Vector3.zero, Quaternion.identity, transform);
                enemyObject.transform.rotation = enemyRotation[roundInfo[currentRound][next].enemyIdx];
                next++;
            }
            else if (next >= roundInfo[currentRound].Count)
            {
                if (transform.childCount == 0)
                {
                    onRound = false;
                    if (currentRound >= maxRound)
                    {
                        onGameWin.Invoke();
                    }
                    else
                    {
                        currentRound++;
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

        // enemyRotation 수정
        enemyRotation.Clear();
        foreach (int enemyIdx in enemyIdxList)
        {
            enemyRotation.Add(enemyIdx, Quaternion.Euler(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180)));
        }
    }

    public int GetCurrentRound() { return currentRound; }

    public int GetMaxRound() { return maxRound; }
}
