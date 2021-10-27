using System;
using UnityEngine;

// 타워 설치에 대한 작업을 처리합니다.
public class MouseCursor : MonoBehaviour
{
	private enum CursorState { idle, installTower, selectTower }

	public GameObject towerStack;

	private CursorState cursorState;
	private CameraManager cameraManager;
	private Camera cam;
	private GameObject currentTower = null;
	private GameObject currentTowerStack = null;
	private Vector3 size;

	void Start()
	{
		cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
		cam = Camera.main;
		size = GameObject.Find("Map Manager").GetComponent<MapManager>().GetSize();
		cursorState = CursorState.idle;
	}

	void Update()
	{
		if (cursorState == CursorState.installTower)
		{
			RaycastHit hit, _hit;
			Vector3 camDir = cameraManager.transform.position - cam.transform.position;
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;

			int layerMask = (1 << LayerMask.NameToLayer("Block")) + (1 << LayerMask.NameToLayer("Tower"));
			// 커서가 블록 또는 타워에 닿아 있고
			// 윗 칸이 비어 있고
			// 경로를 건드리지 않는 경우 (todo)
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
		if (currentTower) currentTower.GetComponent<Tower>().SetSelect(false);
		currentTower = Instantiate(tower);
		currentTowerStack = Instantiate(towerStack, new Vector3(0, -1, 0), Quaternion.identity, currentTower.transform);
		// Raycast에서 자기 자신을 인식하는 것을 막기 위해 임시로 레이어를 바꿈
		ChangeTowerLayer(LayerMask.NameToLayer("Ignore Raycast"));
		cursorState = CursorState.installTower;
	}

	public void OnClickInstallTower()
	{
		if (currentTower == null || !currentTower.activeSelf) return;
		currentTower.transform.GetChild(1).gameObject.SetActive(false);
		ChangeTowerLayer(LayerMask.NameToLayer("Tower"));
		currentTower = null;
		cursorState = CursorState.idle;
	}

	public void ChangeTowerLayer(int layer)
	{
		currentTower.layer = layer;
		// foreach (Transform child in currentTower.transform)
		// {
		// 	child.gameObject.layer = layer;
		// }
	}
}
