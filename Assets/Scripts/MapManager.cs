using System;
using UnityEngine;

// 맵 및 맵을 구성하는 벽돌에 관련된 작업을 처리합니다.
public class MapManager : MonoBehaviour
{
	public Vector3 size;
	public GameObject block;
	public Material material1; // 테마 1
	public Material material2; // 테마 2
	public Material material3; // 입구 및 출구

	private Vector3[] path;
	private Vector3 pathStart;
	private Vector3 pathEnd;

	void Start()
	{
		// 맵 블록 그리기
		LineRenderer pathManager = FindObjectOfType<LineRenderer>();
		path = new Vector3[pathManager.positionCount];
		pathManager.GetPositions(path);
		pathStart = path[0];
		pathEnd = path[path.Length - 1];

		GameObject temp;
		for (int x = 0; x < size.x; x++)
			for (int z = 0; z < size.z; z++)
			{
				temp = Instantiate(block, new Vector3(x, -1, z), Quaternion.identity, transform);
				if ((x + z) % 2 == 0) temp.GetComponent<MeshRenderer>().material = material1;
				else temp.GetComponent<MeshRenderer>().material = material2;
			}

		for (int x = (int)pathStart.x - 1; x <= (int)pathStart.x + 1; x++)
			for (int y = (int)pathStart.y - 1; y <= (int)pathStart.y + 1; y++)
				for (int z = (int)pathStart.z - 1; z <= (int)pathStart.z + 1; z++)
				{
					if (pathStart.x >= size.x && x != (int)pathStart.x) continue;
					if (pathStart.z >= size.z && z != (int)pathStart.z) continue;
					temp = Instantiate(block, new Vector3(x, y, z), Quaternion.identity, transform);
					if (new Vector3(x, y, z) == pathStart) temp.GetComponent<MeshRenderer>().material = material1;
					else temp.GetComponent<MeshRenderer>().material = material3;
					temp.layer = LayerMask.NameToLayer("Ignore Raycast");
				}

		for (int x = (int)pathEnd.x - 1; x <= (int)pathEnd.x + 1; x++)
			for (int y = (int)pathEnd.y - 1; y <= (int)pathEnd.y + 1; y++)
				for (int z = (int)pathEnd.z - 1; z <= (int)pathEnd.z + 1; z++)
				{
					if (pathEnd.x >= size.x && x != (int)pathEnd.x) continue;
					if (pathEnd.z >= size.z && z != (int)pathEnd.z) continue;
					temp = Instantiate(block, new Vector3(x, y, z), Quaternion.identity, transform);
					if (new Vector3(x, y, z) == pathEnd) temp.GetComponent<MeshRenderer>().material = material1;
					else temp.GetComponent<MeshRenderer>().material = material3;
					temp.layer = LayerMask.NameToLayer("Ignore Raycast");
				}
	}

	void Update()
	{

	}

	public Vector3 GetSize() { return size; }
}
