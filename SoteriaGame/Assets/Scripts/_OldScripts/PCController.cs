﻿using UnityEngine;
using System.Collections;

public class PCController : MonoBehaviour
{

    StandardMovementController stdMovement;
    EncounterMovementController encMovement;

	// Use this for initialization
	void Start ()
	{
        stdMovement = gameObject.GetComponent<StandardMovementController>();
        encMovement = gameObject.GetComponent<EncounterMovementController>();
		EnableStandardMovement();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

    public void EnableEncounterMovement()
    {
		//Debug.Log ("Encounter Movement");
        stdMovement.enabled = false;
        encMovement.enabled = true;
    }

    public void EnableStandardMovement()
    {
		//Debug.Log ("Standard Movement");
        stdMovement.enabled = true;
        encMovement.enabled = false;
    }
}