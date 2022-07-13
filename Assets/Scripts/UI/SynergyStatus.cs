using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyStatus : MonoBehaviour
{
    public int synergyIdx;
    public SortedDictionary<int, int> idxCountPairs;
    public SynergyManager.TowerBonus bonus;
    private Text text;
    private Transform towerImageSet;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(int idx, SynergyManager.Synergy synergy)
    {
        idxCountPairs = synergy.idxCountPairs;
        bonus = synergy.bonus;

        text = GetComponentInChildren<Text>();
        text.text = string.Format("시너지 {0}", idx);
        towerImageSet = transform.GetChild(1).transform;

        if (bonus.radiusBonus != 0)
        {
            text.text += string.Format("\n사정거리 +{0}%", bonus.radiusBonus);
        }
        if (bonus.delayBonus != 0)
        {
            text.text += string.Format("\n공격 속도 -{0}%", bonus.delayBonus);
        }
        if (bonus.damageBonus != 0)
        {
            text.text += string.Format("\n공격력 +{0}", bonus.damageBonus);
        }
        if (bonus.speedBonus != 0)
        {
            text.text += string.Format("\n투사체 속도 +{0}%", bonus.speedBonus);
        }

        foreach (var idxCountPair in idxCountPairs)
        {
            GameObject o = new GameObject("Tower Image");
            Sprite sprite = Resources.Load<Sprite>(string.Format("Images/Tower Buttons/Tower Button {0}", idxCountPair.Key));
            Image image = o.AddComponent<Image>();
            image.sprite = sprite;
            image.color = Color.HSVToRGB(0, 0, 0.4f);
            for (int i = 0; i < idxCountPair.Value; i++)
            {
                GameObject child = Instantiate(o, transform.position, transform.rotation, towerImageSet);
                child.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
            Destroy(o);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSynergyStatus(Dictionary<int, int> stackedTower)
    {
        int childIdx = 0;
        bool check = true; // 시너지가 활성화되었는가?

        // 타워 이미지 색 업데이트
        foreach (var keyValuePair in idxCountPairs)
        {
            int idx = keyValuePair.Key;
            int count = keyValuePair.Value;
            for (int i = 1; i <= count; i++)
            {
                Image image = towerImageSet.GetChild(childIdx).GetComponent<Image>();
                int stackedCount = 0;
                stackedTower.TryGetValue(idx, out stackedCount);
                if (stackedCount >= i)
                {
                    image.color = Color.HSVToRGB(0, 0, 1);
                }
                else
                {
                    image.color = Color.HSVToRGB(0, 0, 0.4f);
                    check = false;
                }
                childIdx++;
            }
        }

        // 시너지 글자 색 업데이트
        if (check)
        {
            text.color = Color.HSVToRGB(0, 0, 1);
        }
        else
        {
            text.color = Color.HSVToRGB(0, 0, 0.4f);
        }
    }
}
