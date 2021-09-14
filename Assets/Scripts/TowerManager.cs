using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			RaycastHit hit;
			Vector3 camDir = cameraManager.transform.position - cam.transform.position;
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;

			if (Physics.Raycast(cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane)), cam.transform.forward, out hit))
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
	}
}
