using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public Vector3 size;
	public GameObject block;

	private GameObject bottom;
	private GameObject wallXPlus;
	private GameObject wallZPlus;

	void Start()
	{
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

		for (int y = 0; y < size.y; y++)
			for (int z = 0; z < size.z; z++)
			{
				temp = Instantiate(block, new Vector3(size.x, y, z), Quaternion.identity, wallXPlus.transform);
				if ((size.x + y + z) % 2 == 0) temp.GetComponent<MeshRenderer>().material.color = Color.gray;
			}

		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
			{
				temp = Instantiate(block, new Vector3(x, y, size.z), Quaternion.identity, wallZPlus.transform);
				if ((x + y + size.z) % 2 == 0) temp.GetComponent<MeshRenderer>().material.color = Color.gray;
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
