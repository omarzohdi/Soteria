﻿using UnityEngine;
using System.Collections;

public class TheaterPass1Done : MonoBehaviour
{
	void OnTriggerEnter(Collider player)
	{
		if (player.gameObject.tag == "Player")
		{
			GameDirector.instance.TheaterPass1Done();
			GameDirector.instance.FirstTimeTheaterPuzzle();
		}
	}
}