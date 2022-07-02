using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    [Serializable]
    public class Bonus
    {
        public float radiusBonus;
        public float delayBonus;
        public int damageBonus;
        public float speedBonus;
    }

    [Serializable]
    public class Synergy
    {
        public Synergy(Bonus b)
        {
            idxCountPair = new Dictionary<int, int>();
            bonus = b;
        }

        public Dictionary<int, int> idxCountPair;
        public Bonus bonus;
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
                Bonus bonus = new Bonus();
                bonus.radiusBonus = float.Parse(list[1]);
                bonus.delayBonus = float.Parse(list[2]);
                bonus.damageBonus = int.Parse(list[3]);
                bonus.speedBonus = float.Parse(list[4]);
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
