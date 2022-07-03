using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    [Serializable]
    public class TowerBonus
    {
        public int radiusBonus;
        public int delayBonus;
        public int damageBonus;
        public int speedBonus;
    }

    [Serializable]
    public class Synergy
    {
        public Synergy(TowerBonus b)
        {
            idxCountPair = new Dictionary<int, int>();
            bonus = b;
        }

        public Dictionary<int, int> idxCountPair;
        public TowerBonus bonus;
    }

    public Dictionary<int, Synergy> synergyInfo;
    public string synergyInfoFileName;

    // Start is called before the first frame update
    void Start()
    {
        synergyInfo = new Dictionary<int, Synergy>();
        List<List<string>> csv = CSVReader.Read(synergyInfoFileName);
        int currSynergyIdx = 0;
        foreach (List<string> list in csv.GetRange(2, csv.Count - 2))
        {
            if (list.Count != 7) continue;
            int synergyIdx;
            if (int.TryParse(list[0], out synergyIdx))
            {
                currSynergyIdx = synergyIdx;
                TowerBonus bonus = new TowerBonus();
                bonus.radiusBonus = int.Parse(list[1]);
                bonus.delayBonus = int.Parse(list[2]);
                bonus.damageBonus = int.Parse(list[3]);
                bonus.speedBonus = int.Parse(list[4]);
                synergyInfo.Add(currSynergyIdx, new Synergy(bonus));
            }
            int towerIdx = int.Parse(list[5]);
            int towerCount = int.Parse(list[6]);
            synergyInfo[currSynergyIdx].idxCountPair.Add(towerIdx, towerCount);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
