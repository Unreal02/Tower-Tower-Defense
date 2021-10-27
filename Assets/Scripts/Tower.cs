using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타워 오브젝트입니다.
// mouseCursor에서 타워 설치하기 전에는 비활성화 상태입니다.
// 타워를 설치하는 순간 활성화됩니다.
public class Tower : MonoBehaviour
{
    public float radius;

    private Component radiusSphere;
    private MouseCursor mouseCursor;
    private bool select;

    // Start is called before the first frame update
    void Start()
    {
        radiusSphere = transform.GetChild(1);
        radiusSphere.transform.localScale = new Vector3(radius, radius, radius) * 2;
        mouseCursor = GameObject.Find("Mouse Cursor").GetComponent<MouseCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelect(bool b)
    {
        radiusSphere.gameObject.SetActive(b);
        select = b;
    }

    private void OnMouseUpAsButton()
    {
        mouseCursor.OnClickTower(gameObject);
    }
}
