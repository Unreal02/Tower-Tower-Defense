using System;
using UnityEngine;

// 타워 설치에 대한 작업을 처리합니다.
public class TowerManager : MonoBehaviour
{
	private CameraManager cameraManager;
	private Camera cam;
	private GameObject currentTower = null;
	private Vector3 size;

	void Start()
	{
		cameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
		cam = Camera.main;
		size = GameObject.Find("Map Manager").GetComponent<MapManager>().GetSize();
	}

	void Update()
	{
		if (currentTower != null)
		{
			RaycastHit hit, _hit;
			Vector3 camDir = cameraManager.transform.position - cam.transform.position;
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;

			int layerMask = (1 << LayerMask.NameToLayer("Block")) + (1 << LayerMask.NameToLayer("Tower"));
			// 커서가 블록 또는 타워에 닿아 있고
			// 윗 칸이 비어 있고
			// 경로를 건드리지 않는 경우
			if (Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane)), cam.transform.forward, out hit, Mathf.Infinity, layerMask)
				&& !Physics.Raycast(hit.transform.position, new Vector3(0, 1, 0), out _hit, 1, layerMask))
			{
				float x = hit.transform.position.x;
				float z = hit.transform.position.z;
				if (0 <= x && x < size.x && 0 <= z && z < size.z)
				{
					currentTower.SetActive(true);
					currentTower.transform.position = hit.transform.position + Vector3.up;
				}
				else currentTower.SetActive(false);
			}
			else currentTower.SetActive(false);
		}
	}

	public void OnClickTowerButton(GameObject tower)
	{
		currentTower = Instantiate(tower);
		// Raycast에서 자기 자신을 인식하는 것을 막기 위해 임시로 레이어를 바꿈
		ChangeTowerLayer(LayerMask.NameToLayer("Ignore Raycast"));
	}

	public void OnClickInstallTower()
	{
		if (currentTower == null || !currentTower.activeSelf) return;
		currentTower.transform.GetChild(1).gameObject.SetActive(false);
		ChangeTowerLayer(LayerMask.NameToLayer("Tower"));
		currentTower = null;
	}

	public void ChangeTowerLayer(int layer)
	{
		currentTower.layer = layer;
		foreach (Transform child in currentTower.transform)
		{
			child.gameObject.layer = layer;
		}
	}
}
