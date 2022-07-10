using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Serializable]
    public class TowerData
    {
        public string towerName;
        public RadiusType radiusType;
        public List<int> cost; // 가격
        public List<float> radius; // 공격 반경
        public List<float> delay; // 공격 딜레이 시간
        public List<GameObject> bullet; // 투사체 오브젝트
        public List<int> damage; // 공격력
        public List<float> speed; // 투사체 발사 속도
        public List<bool> targeting; // 투사체가 목표를 따라가는지
        public List<float> life; // 투사체 지속 시간
        public List<int> bulletHp; // 투사체 관통력
    }

    public Dictionary<int, TowerData> towerInfo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        towerInfo = new Dictionary<int, TowerData>();
        List<List<string>> csv = CSVReader.Read("TowerInfo");
        int currTowerIdx = 0;
        foreach (List<string> list in csv)
        {
            switch (list[0])
            {
                case "Tower Idx":
                    currTowerIdx = int.Parse(list[1]);
                    towerInfo.Add(currTowerIdx, new TowerData());
                    break;
                case "Tower Name":
                    towerInfo[currTowerIdx].towerName = list[1];
                    break;
                case "Radius Type":
                    towerInfo[currTowerIdx].radiusType = (RadiusType)Enum.Parse(typeof(RadiusType), list[1]);
                    break;
                case "Cost":
                    towerInfo[currTowerIdx].cost = new List<int>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].cost.Add(int.Parse(list[i + 1]));
                    break;
                case "Radius":
                    towerInfo[currTowerIdx].radius = new List<float>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].radius.Add(float.Parse(list[i + 1]));
                    break;
                case "Delay":
                    towerInfo[currTowerIdx].delay = new List<float>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].delay.Add(float.Parse(list[i + 1]));
                    break;
                case "Bullet Idx":
                    towerInfo[currTowerIdx].bullet = new List<GameObject>();
                    for (int i = 0; i < 5; i++)
                    {
                        int bulletIdx = int.Parse(list[i + 1]);
                        towerInfo[currTowerIdx].bullet.Add(Resources.Load<GameObject>("Objects/Bullet" + bulletIdx));
                    }
                    break;
                case "Damage":
                    towerInfo[currTowerIdx].damage = new List<int>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].damage.Add(int.Parse(list[i + 1]));
                    break;
                case "Speed":
                    towerInfo[currTowerIdx].speed = new List<float>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].speed.Add(float.Parse(list[i + 1]));
                    break;
                case "Targeting":
                    towerInfo[currTowerIdx].targeting = new List<bool>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].targeting.Add(Convert.ToBoolean(int.Parse(list[i + 1])));
                    break;
                case "Life":
                    towerInfo[currTowerIdx].life = new List<float>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].life.Add(float.Parse(list[i + 1]));
                    break;
                case "Bullet HP":
                    towerInfo[currTowerIdx].bulletHp = new List<int>();
                    for (int i = 0; i < 5; i++) towerInfo[currTowerIdx].bulletHp.Add(int.Parse(list[i + 1]));
                    break;
            }
        }
    }
}
