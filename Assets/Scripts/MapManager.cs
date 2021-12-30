using System;
using UnityEngine;

// 맵 및 맵을 구성하는 벽돌에 관련된 작업을 처리합니다.
public class MapManager : MonoBehaviour
{
	public Vector3 size;
	public GameObject block;

	private Vector3[] path;
	private Vector3 pathStart;
	private Vector3 pathEnd;
	private GameObject bottom;
	private GameObject wallXPlus;
	private GameObject wallZPlus;

	void Start()
	{
		// 맵 블록 그리기
		LineRenderer pathManager = FindObjectOfType<LineRenderer>();
		path = new Vector3[pathManager.positionCount];
		pathManager.GetPositions(path);
		pathStart = path[0];
		pathEnd = path[path.Length - 1];

		bottom = new GameObject();
		wallXPlus = new GameObject();
		wallZPlus = new GameObject();

		bottom.transform.parent = transform;
		wallXPlus.transform.parent = transform;
		wallZPlus.transform.parent = transform;

		GameObject temp;
		for (int x = 0; x < size.x; x++)
			for (int z = 0; z < size.z; z++)
			{
				temp = Instantiate(block, new Vector3(x, -1, z), Quaternion.identity, bottom.transform);
				if ((x + z) % 2 == 0) temp.GetComponent<MeshRenderer>().material.color = Color.gray;
			}

		for (int x = (int)pathStart.x - 1; x <= pathStart.x + 1; x++)
			for (int y = (int)pathStart.y - 1; y <= pathStart.y + 1; y++)
				for (int z = (int)pathStart.z - 1; z <= pathStart.z + 1; z++)
				{
					if (pathStart.x >= size.x && x != (int)pathStart.x) continue;
					if (pathStart.z >= size.z && z != (int)pathStart.z) continue;
					temp = Instantiate(block, new Vector3(x, y, z), Quaternion.identity, wallXPlus.transform);
					if (new Vector3(x, y, z) != pathStart) temp.GetComponent<MeshRenderer>().material.color = Color.black;
				}

		for (int x = (int)pathEnd.x - 1; x <= pathEnd.x + 1; x++)
			for (int y = (int)pathEnd.y - 1; y <= pathEnd.y + 1; y++)
				for (int z = (int)pathEnd.z - 1; z <= pathEnd.z + 1; z++)
				{
					if (pathEnd.x >= size.x && x != (int)pathEnd.x) continue;
					if (pathEnd.z >= size.z && z != (int)pathEnd.z) continue;
					temp = Instantiate(block, new Vector3(x, y, z), Quaternion.identity, wallXPlus.transform);
					if (new Vector3(x, y, z) != pathEnd) temp.GetComponent<MeshRenderer>().material.color = Color.black;
				}
	}

	void Update()
	{

	}

	public Vector3 GetSize()
	{
		return size;
	}
}
