using System;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	public Vector3[] path;
	public float width;
	public Color color;

	private LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = path.Length;
		lineRenderer.SetPositions(path);
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
	}

	void Update()
	{
		
	}

	public Vector3[] GetPath()
	{
		return path;
	}
}
