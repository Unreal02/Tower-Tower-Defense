using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float hp;
	public float speed;

	private PathManager pathManager;
	private Vector3[] path;
	private int currentNode = 0;
	private int nextNode = 1;

	void Start()
	{
		pathManager = GameObject.Find("Path Manager").GetComponent<PathManager>();
		path = pathManager.GetPath();
	}

	void Update()
	{
		Vector3 position = transform.position;
		Vector3 direction = (path[nextNode] - path[currentNode]).normalized;
		float distance = (path[nextNode] - position).magnitude;
		float move = Time.deltaTime * speed;
		if (move >= distance)
		{
			currentNode++; nextNode++;
			if (nextNode == path.Length)
			{
				Destroy(gameObject);
				return;
			}
			direction = (path[nextNode] - path[currentNode]).normalized;
			position = path[currentNode];
			move -= distance;
		}
		position += direction * move;
		transform.position = position;
	}
}
