using System;
using UnityEngine;

// 카메라 시점 관련된 작업을 처리합니다.
public class CameraManager : MonoBehaviour
{
    public float mouseRotateSpeed; // 마우스 시점 회전 속도
    public float keyboardRotateSpeed; // 키보드 시점 회전 속도
    public float keyboardMoveSpeed; // 키보드 카메라 이동 속도

    private Vector3 prevMousePosition;
    private Vector3 currMousePosition;
    private Camera myCamera;

    void Start()
    {
        myCamera = Camera.main;
        MapManager mapManager = FindObjectOfType<MapManager>();
        Vector3 size = mapManager.GetSize();
        Vector3 position = new Vector3((size.x - 1) / 2, (size.y - 1) / 2, (size.z - 1) / 2);
        transform.position = position;
    }

    void Update()
    {
        // 오른쪽 버튼: 카메라 회전
        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButton(2))
        {
            prevMousePosition = currMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            prevMousePosition = currMousePosition;
            currMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 deltaMousePosition = currMousePosition - prevMousePosition;
            Rotate(-deltaMousePosition.y * mouseRotateSpeed, deltaMousePosition.x * mouseRotateSpeed);
        }

        // Q/E/C/V: 카메라 회전
        if (Input.GetKey(KeyCode.Q)) Rotate(0, keyboardRotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) Rotate(0, -keyboardRotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.C)) Rotate(keyboardRotateSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.V)) Rotate(-keyboardRotateSpeed * Time.deltaTime, 0);

        // 스크롤: 확대/축소
        if (Input.mouseScrollDelta.y != 0)
        {
            myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - Input.mouseScrollDelta.y, 1, 20);
        }

        // 가운데 버튼: 카메라 이동
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

        // W/A/S/D: 카메라 이동
        if (Input.GetKey(KeyCode.W)) Move(0, -keyboardMoveSpeed * Screen.width * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) Move(0, keyboardMoveSpeed * Screen.width * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) Move(keyboardMoveSpeed * Screen.width * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D)) Move(-keyboardMoveSpeed * Screen.width * Time.deltaTime, 0);
    }

    // x, y: degree 단위
    private void Rotate(float x, float y)
    {
        Vector3 vector = transform.rotation.eulerAngles;
        vector.x = Mathf.Clamp(vector.x + x, 0, 90);
        vector.y = vector.y + y;
        transform.rotation = Quaternion.Euler(vector);
    }

    // x, y: 픽셀 단위
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
        position += (X * x + Y * y) / Screen.height * myCamera.orthographicSize * 2;
        transform.position = position;
    }
}
