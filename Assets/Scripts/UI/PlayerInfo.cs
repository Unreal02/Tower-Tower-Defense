using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    public UnityEvent onGameLose;

    public int initialMoney;
    public int initialLife;

    private int money;
    private int life;

    private RoundManager roundManager;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        money = initialMoney;
        life = initialLife;
        roundManager = FindObjectOfType<RoundManager>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int round = roundManager.GetCurrentRound();
        int maxRound = roundManager.GetMaxRound();
        text.text = string.Format("라운드 {0} / {1}\n자금 {2}\n체력 {3}", round, maxRound, money, life);
    }

    public int GetMoney() { return money; }
    public void AddMoney(int delta) { money += delta; }
    public void SubtractMoney(int delta) { money -= delta; }

    public void SubtractLife(int delta)
    {
        life -= delta;
        if (life <= 0)
        {
            life = 0;
            onGameLose.Invoke();
        }
    }
}
