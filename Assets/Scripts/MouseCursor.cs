using System;
using UnityEngine;
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
	private GameObject currentTowerStack = null;
	private Vector3 size;

	void Start()
	{
		playerInfo = FindObjectOfType<PlayerInfo>();
		cameraManager = FindObjectOfType<CameraManager>();
		cam = Camera.main;
		size = FindObjectOfType<MapManager>().GetSize();
		cursorState = CursorState.idle;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;
			int layerMask = (1 << LayerMask.NameToLayer("Block")) + (1 << LayerMask.NameToLayer("Tower")) + (1 << LayerMask.NameToLayer("UI"));

			// 빈 공간을 좌클릭하면 idle 상태로 전환
			if (!Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane)), cam.transform.forward, out hit, Mathf.Infinity, layerMask))
			{
				if (cursorState == CursorState.installTower) Destroy(currentTower);
				if (cursorState == CursorState.selectTower) currentTower.GetComponent<Tower>().SetSelect(false);
				cursorState = CursorState.idle;
				currentTower = null;
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
					currentTowerStack.SetActive(hit.transform.GetComponent<Tower>());
					currentTower.transform.position = hit.transform.position + Vector3.up;
				}
				else
				{
					currentTower.SetActive(false);
					currentTowerStack.SetActive(false);
				}
			}
			else
			{
				currentTower.SetActive(false);
				currentTowerStack.SetActive(false);
			}
		}
	}

	public void OnClickBlock()
	{
		if (cursorState == CursorState.installTower) OnClickInstallTower();
		else 
		{
			cursorState = CursorState.idle;
			if (currentTower) currentTower.GetComponent<Tower>().SetSelect(false);
			currentTower = null;
		}
	}

	public void OnClickTower(GameObject tower)
	{
		switch (cursorState)
		{
			case CursorState.installTower:
				OnClickInstallTower();
				break;
			case CursorState.selectTower:
				if (currentTower == tower)
				{
					currentTower.GetComponent<Tower>().SetSelect(false);
					currentTower = null;
					cursorState = CursorState.idle;
				}
				else
				{
					currentTower.GetComponent<Tower>().SetSelect(false);
					currentTower = tower;
					currentTower.GetComponent<Tower>().SetSelect(true);
				}
				break;
			case CursorState.idle:
				cursorState = CursorState.selectTower;
				currentTower = tower;
				currentTower.GetComponent<Tower>().SetSelect(true);
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
		currentTowerStack = Instantiate(towerStack, new Vector3(0, -1, 0), Quaternion.identity, currentTower.transform);

		// Raycast에서 자기 자신을 인식하는 것을 막기 위해 임시로 레이어를 바꿈
		ChangeTowerLayer(LayerMask.NameToLayer("Ignore Raycast"));
		Tower towerComponent = currentTower.GetComponent<Tower>();
		cursorState = CursorState.installTower;
		towerComponent.enabled = false; // Tower 컴포넌트를 끔으로써 공격 안 하도록 만듦
        currentTower.transform.GetChild(1).transform.localScale = 2 * towerComponent.GetRadius() * new Vector3(1, 1, 1); // 반경을 나타내는 구 설정
	}

	public void OnClickInstallTower() // 타워 설치하는 순간
	{
		if (currentTower == null || !currentTower.activeSelf) return;
		Tower towerComponent = currentTower.GetComponent<Tower>();
		currentTower.transform.GetChild(1).gameObject.SetActive(false);
		towerComponent.enabled = true;
		ChangeTowerLayer(LayerMask.NameToLayer("Tower"));
		currentTower = null;
		cursorState = CursorState.idle;
		playerInfo.SubtractMoney(towerComponent.GetCost());
	}

	public void ChangeTowerLayer(int layer)
	{
		currentTower.layer = layer;
	}
}
