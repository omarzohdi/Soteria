using UnityEngine;
using System.Collections;

public class  PTFirstEncounter: MonoBehaviour
{
	void OnTriggerEnter(Collider player)
	{
		if (player.gameObject.tag == "Player")
		{
			GameDirector.instance.GetPlayer().PlayerActionPause();
			GameDirector.instance.SetupDialogue("PTFirstEncounter");
			GameDirector.instance.StartDialogue();
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
		}
	}
}