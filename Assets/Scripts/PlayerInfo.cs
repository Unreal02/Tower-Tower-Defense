using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
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
        int round = roundManager.GetCurrentRound() + 1;
        text.text = string.Format("라운드 {0}\n자금 {1}\n체력 {2}", round, money, life);
    }

    public int GetMoney() { return money; }
    public void AddMoney(int delta) { money += delta; }
    public void SubtractMoney(int delta) { money -= delta; }

    public void SubtractLife(int delta)
    {
        life -= delta;
        // todo: life가 0 이하가 되면 게임 오버
    }
}
