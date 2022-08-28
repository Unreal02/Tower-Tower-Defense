using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControlButton : MonoBehaviour
{
    private int speed;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickSpeedControlButton()
    {
        if (speed == 1) SetSpeed(2);
        else SetSpeed(1);
    }

    private void SetSpeed(int s)
    {
        speed = s;
        Time.timeScale = s;
        text.text = string.Format("{0}x", s);
    }

    public int GetSpeed() { return speed; }
}
