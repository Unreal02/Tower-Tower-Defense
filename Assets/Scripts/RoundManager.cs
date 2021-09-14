using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
	[Serializable]
	public class Spawn
	{
		public float time;
		public GameObject enemy;
	}

	[Serializable]
	public class Round
	{
		public Spawn[] spawns;
	}

	public Round[] rounds;

	private int next = 0;
	private int currentRound = 0;
	private float currentTime = 0;
	private bool onRound = false;

	void Start()
	{
		
	}

	void Update()
	{
		if (onRound)
		{
			currentTime += Time.deltaTime;
			if (next < rounds[currentRound].spawns.Length && currentTime >= rounds[currentRound].spawns[next].time)
			{
				Instantiate(rounds[currentRound].spawns[next].enemy, Vector3.zero, Quaternion.identity, transform);
				next++;
			}
			else if (next >= rounds[currentRound].spawns.Length)
			{
				if (transform.childCount == 0)
				{
					onRound = false;
					currentRound++;
				}
			}
		}
	}

	public void OnClickNextRound()
	{
		if (currentRound >= rounds.Length) return;
		if (onRound) return;
		currentTime = 0;
		next = 0;
		onRound = true;
	}
}
