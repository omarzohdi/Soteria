﻿using UnityEngine;
using System.Collections;

public class AddGamePhase : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			Application.LoadLevel ("FullModelHub");
			GameDirector.instance.AddGamePhase ();
		}
	}
}
