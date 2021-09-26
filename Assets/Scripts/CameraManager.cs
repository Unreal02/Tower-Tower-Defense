using System;
using UnityEngine;

// 카메라 시점 관련된 작업을 처리합니다.
public class CameraManager : MonoBehaviour
{
	public float rotateSpeed;
	public float moveSpeed;

	private Vector3 prevMousePosition;
	private Vector3 currMousePosition;
	private Camera myCamera;

	void Start()
	{
		myCamera = Camera.main;
		MapManager mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
		Vector3 size = mapManager.GetSize();
		Vector3 position = new Vector3((size.x - 1) / 2, (size.y - 1) / 2, (size.z - 1) / 2);
		transform.position = position;
	}

	void Update()
	{
		// 왼쪽 버튼
		if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(2))
		{
			prevMousePosition = currMousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButton(0) && !Input.GetMouseButton(2))
		{
			prevMousePosition = currMousePosition;
			currMousePosition = Input.mousePosition;
			Vector3 deltaMousePosition = currMousePosition - prevMousePosition;
			Rotate(-deltaMousePosition.y, deltaMousePosition.x);
		}

		// 스크롤
		if (Input.mouseScrollDelta.y != 0)
		{
			myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - Input.mouseScrollDelta.y, 1, 20);
		}

		// 가운데 버튼
		if (Input.GetMouseButtonDown(2) && !Input.GetMouseButton(0))
		{
			prevMousePosition = currMousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButton(2) && !Input.GetMouseButton(0))
		{
			prevMousePosition = currMousePosition;
			currMousePosition = Input.mousePosition;
			Vector3 deltaMousePosition = currMousePosition - prevMousePosition;
			Move(deltaMousePosition.x, deltaMousePosition.y);
		}
	}

	private void Rotate(float x, float y)
	{
		Vector3 vector = transform.rotation.eulerAngles;
		vector.x = Mathf.Clamp(vector.x + x * Time.deltaTime * rotateSpeed, 0, 90);
		vector.y = vector.y + y * Time.deltaTime * rotateSpeed;
		transform.rotation = Quaternion.Euler(vector);
	}

	private void Move(float x, float y)
	{
		Vector3 cameraDirection = transform.position - myCamera.transform.position;
		Vector3 X = Vector3.Cross(cameraDirection, Vector3.up).normalized;
		if (X.magnitude < 0.0001)
		{
			float angle = transform.eulerAngles.y / 180 * Mathf.PI;
			X = new Vector3(-Mathf.Cos(angle), 0, Mathf.Sin(angle));
		}
		Vector3 Y = Vector3.Cross(cameraDirection, X).normalized;

		Vector3 position = transform.position;
		position += (X * x + Y * y) * Time.deltaTime * myCamera.orthographicSize * moveSpeed;
		transform.position = position;
	}
}
