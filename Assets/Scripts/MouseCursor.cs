using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 타워 설치에 대한 작업을 처리합니다.
public class MouseCursor : MonoBehaviour
{
    private enum CursorState { idle, installTower, selectTower }

    public GameObject towerStack;

    private PlayerInfo playerInfo;
    private CursorState cursorState;
    private CameraManager cameraManager;
    private Camera cam;
    private GameObject currentTower = null;
    private Vector3 size;
    private TowerStatus towerStatus;

    private GraphicRaycaster gr; // UI 클릭

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        cameraManager = FindObjectOfType<CameraManager>();
        towerStatus = FindObjectOfType<TowerStatus>();
        cam = Camera.main;
        size = FindObjectOfType<MapManager>().GetSize();
        cursorState = CursorState.idle;
        gr = FindObjectOfType<GraphicRaycaster>(); // UI 클릭
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            int layerMask = (1 << LayerMask.NameToLayer("Block")) + (1 << LayerMask.NameToLayer("Tower"));
            bool raycastResult = Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane)), cam.transform.forward, out hit, Mathf.Infinity, layerMask);

            // UI 클릭
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            // 빈 공간(UI도 없는 곳)을 좌클릭하면 idle 상태로 전환
            if (!raycastResult && results.Count <= 0)
            {
                if (cursorState == CursorState.installTower) Destroy(currentTower);
                if (cursorState == CursorState.selectTower) currentTower.GetComponent<Tower>().SetSelect(false);
                SetCursorState(CursorState.idle);
            }
        }

        if (cursorState == CursorState.installTower)
        {
            RaycastHit hit, _hit;
            Vector3 camDir = cameraManager.transform.position - cam.transform.position;
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            int layerMask = (1 << LayerMask.NameToLayer("Block")) + (1 << LayerMask.NameToLayer("Tower"));

            // 커서가 블록 또는 타워에 닿아 있고
            // 윗 칸이 비어 있고
            // todo: 경로를 건드리지 않는 경우
            if (Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane)), cam.transform.forward, out hit, Mathf.Infinity, layerMask)
                && !Physics.Raycast(hit.transform.position, new Vector3(0, 1, 0), out _hit, 1, layerMask))
            {
                float x = hit.transform.position.x;
                float z = hit.transform.position.z;
                if (0 <= x && x < size.x && 0 <= z && z < size.z)
                {
                    currentTower.SetActive(true);
                    currentTower.GetComponent<Tower>().SetTowerStack(hit.transform.GetComponent<Tower>());
                    currentTower.transform.position = hit.transform.position + Vector3.up;
                }
                else
                {
                    currentTower.SetActive(false);
                    currentTower.GetComponent<Tower>().SetTowerStack(false);
                }
            }
            else
            {
                currentTower.SetActive(false);
                currentTower.GetComponent<Tower>().SetTowerStack(false);
            }
        }
    }

    public GameObject GetCurrentTower() { return currentTower; }

    public void OnClickBlock()
    {
        if (cursorState == CursorState.installTower) OnInstallTower();
        else
        {
            if (currentTower) currentTower.GetComponent<Tower>().SetSelect(false);
            SetCursorState(CursorState.idle);
        }
    }

    public void OnClickTower(GameObject tower)
    {
        switch (cursorState)
        {
            case CursorState.installTower:
                OnInstallTower();
                break;
            case CursorState.selectTower:
                if (currentTower == tower)
                {
                    currentTower.GetComponent<Tower>().SetSelect(false);
                    SetCursorState(CursorState.idle);
                }
                else
                {
                    currentTower.GetComponent<Tower>().SetSelect(false);
                    currentTower = tower;
                    currentTower.GetComponent<Tower>().SetSelect(true);
                }
                break;
            case CursorState.idle:
                currentTower = tower;
                currentTower.GetComponent<Tower>().SetSelect(true);
                SetCursorState(CursorState.selectTower);
                break;
        }
    }

    public void OnClickTowerButton(GameObject tower)
    {
        if (cursorState == CursorState.installTower) Destroy(currentTower);
        if (cursorState == CursorState.selectTower) currentTower.GetComponent<Tower>().SetSelect(false);

        // 돈이 모자라지 않는지 확인
        if (playerInfo.GetMoney() < tower.GetComponent<Tower>().GetCost())
            return;

        currentTower = Instantiate(tower);

        // Raycast에서 자기 자신을 인식하는 것을 막기 위해 임시로 레이어를 바꿈
        ChangeTowerLayer(LayerMask.NameToLayer("Ignore Raycast"));
        SetCursorState(CursorState.installTower);
        Tower towerComponent = currentTower.GetComponent<Tower>();
        towerComponent.UpdateRadiusSphere(); // 반경을 나타내는 구 설정
        towerComponent.enabled = false; // Tower 컴포넌트를 끔으로써 공격 안 하도록 만듦
    }

    public void OnInstallTower() // 타워 설치하는 순간
    {
        if (currentTower == null || !currentTower.activeSelf) return;
        Tower towerComponent = currentTower.GetComponent<Tower>();
        currentTower.transform.GetChild(1).gameObject.SetActive(false);
        towerComponent.enabled = true;
        ChangeTowerLayer(LayerMask.NameToLayer("Tower"));
        SetCursorState(CursorState.idle);
        playerInfo.SubtractMoney(towerComponent.GetCost());
        towerComponent.OnInstallTower();
    }

    public void OnSellTower() // 타워 판매하는 순간
    {
        SetCursorState(CursorState.idle);
    }

    public void ChangeTowerLayer(int layer)
    {
        currentTower.layer = layer;
    }

    private void SetCursorState(CursorState s)
    {
        if (s == CursorState.idle) currentTower = null;
        if (s == CursorState.selectTower) towerStatus.SetTowerStatusUI(true);
        else towerStatus.SetTowerStatusUI(false);
        cursorState = s;
    }
}
