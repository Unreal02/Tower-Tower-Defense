using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyStatusSet : MonoBehaviour
{
    private SynergyManager synergyManager;
    private Dictionary<int, SynergyManager.Synergy> synergyInfo;
    public GameObject synergyStatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        synergyManager = FindObjectOfType<SynergyManager>();
        synergyInfo = synergyManager.synergyInfo;

        foreach (var synergyInfoPair in synergyInfo)
        {
            int idx = synergyInfoPair.Key;
            SynergyManager.Synergy synergy = synergyInfoPair.Value;
            GameObject o = Instantiate(synergyStatus, transform.position, transform.rotation, transform);
            o.GetComponent<SynergyStatus>().idxCountPair = synergy.idxCountPair;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSynergyStatusSet(Dictionary<int, int> stackedTower)
    {
        foreach (SynergyStatus synergyStatus in GetComponentsInChildren<SynergyStatus>())
        {
            synergyStatus.UpdateSynergyStatus(stackedTower);
        }
    }
}
