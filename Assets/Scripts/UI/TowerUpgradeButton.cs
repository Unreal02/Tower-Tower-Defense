using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TowerStatus towerStatus;
    private GameObject previewText;

    // Start is called before the first frame update
    void Start()
    {
        towerStatus = transform.parent.parent.GetComponent<TowerStatus>();
        previewText = transform.parent.GetChild(0).GetChild(0).gameObject;
        previewText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        previewText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        previewText.SetActive(false);
    }
}
