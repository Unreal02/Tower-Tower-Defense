using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyStatusSet : MonoBehaviour
{
    private SynergyManager synergyManager;
    private SortedDictionary<int, SynergyManager.Synergy> synergyInfo;
    private Dictionary<int, GameObject> synergyStatusDict;
    public GameObject synergyStatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        synergyManager = FindObjectOfType<SynergyManager>();
        synergyInfo = synergyManager.synergyInfo;
        synergyStatusDict = new Dictionary<int, GameObject>();

        foreach (var synergyInfoPair in synergyInfo)
        {
            int idx = synergyInfoPair.Key;
            SynergyManager.Synergy synergy = synergyInfoPair.Value;
            GameObject o = Instantiate(synergyStatus, transform.position, transform.rotation, transform);
            o.GetComponent<SynergyStatus>().Init(idx, synergy);
            synergyStatusDict.Add(idx, o);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSynergyStatusSet(int selectedTowerIdx, Dictionary<int, int> stackedTower)
    {

        foreach (var keyValuePair in synergyStatusDict)
        {
            int synergyIdx = keyValuePair.Key;
            GameObject gameObject = keyValuePair.Value;
            if (synergyInfo[synergyIdx].idxCountPairs.ContainsKey(selectedTowerIdx))
            {
                gameObject.SetActive(true);
                gameObject.GetComponent<SynergyStatus>().UpdateSynergyStatus(stackedTower);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
