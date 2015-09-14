﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudManager : MonoBehaviour {

	private Image _fadeinout;
	public float fadeSpeed = 1.5f;
	public bool isToClear = true;

	private GameObject _hudinterface;

	private Image _onIndicator;
	private Image _offIndicator;
	private bool _isonscreen;
	public GameObject Objective;

	private GameObject [] Buttons;
	
	// Use this for initialization
	void Awake () 
	{

	}

	public void Initialize()
	{
		this._hudinterface = GameObject.Find ("HUDInterface");
		this._fadeinout = _hudinterface.transform.FindChild("FadeTexture").gameObject.GetComponent<Image>();
		this._fadeinout.color = Color.clear;

		this._onIndicator = _hudinterface.transform.FindChild("OnIndicator").gameObject.GetComponent<Image> ();
		this._offIndicator = _hudinterface.transform.FindChild("OffIndicator").gameObject.GetComponent<Image> ();

		this._offIndicator.gameObject.SetActive(false);
		this._onIndicator.gameObject.SetActive(false);

		this.Buttons = new GameObject [6];
		for (int i=0; i< 6; ++i)
			this.Buttons[i] = _hudinterface.transform.FindChild("Button_"+i).gameObject;
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateFade ();
		UpdateIndicator ();
	}

	private void UpdateIndicator ()
	{
		if (Objective == null)
			return;
		///Convert position of Objective to Screen Coordinates.
		/// Create A Camera Manager!!!
		Vector3 ObjectPosition = Camera.main.WorldToScreenPoint (Objective.transform.position);
		
		if (InSideScreen (ObjectPosition))
		{
			this._onIndicator.gameObject.SetActive(true);
			this._offIndicator.gameObject.SetActive(false);
			
			this._onIndicator.transform.position = new Vector3( ObjectPosition.x , ObjectPosition.y , 0);
		}
		else
		{
			this._offIndicator.gameObject.SetActive(true);
			this._onIndicator.gameObject.SetActive(false);
			
			///Calc the Offscreen Position///
			float offset = 25.0f;
			float x = ObjectPosition.x , y = ObjectPosition.y;
			//Vector3 BorderPosition = new Vector3(0.0f, 0.0f, 0.0f);
			
			//Check if Set Z to infront of camera if it's behind set it infront for the sake of calculations//
			//Shouldn't be a problem unless we move the camera can turn in all 3 Axis.
			//if (ObjectPosition.z < 0)
			//	ObjectPosition = - ObjectPosition;
			
			//Check if X is out of bounds//
			if (ObjectPosition.x > Screen.width)
				x = Screen.width - offset;
			else if (ObjectPosition.x < 0)
				x = offset;
			
			//Check if Y is out of bounds//
			if (ObjectPosition.y > Screen.height)
				y = Screen.height - offset;
			else if (ObjectPosition.y < 0)
				y = offset;
			
			_offIndicator.transform.position = new Vector3 (x, y , 0);
			//Should also add rotation for the arrow to point at,
			//Add a second check if it's in the corner than rotate the arrow correctly. 
		}
	}

	private void UpdateFade()
	{
		if (isToClear)
			FadeToClear ();
		else
			FadeToBlack ();
	}

	private bool InSideScreen( Vector3 inObjPos)
	{
		//Nested IFs for the sake of readability. (checks that the object is within the screen )
		if (inObjPos.z > 0)
		{
			if (inObjPos.x < Screen.width && inObjPos.x > 0)
			{
				if (inObjPos.y < Screen.height && inObjPos.y > 0 )
				{
					return true;
				}
			}
		}
		
		return false;
	}
	
	///To swap components the only method I found is by using reflection,
	//No worth the trouble. hmmm, I'll check for another method for now it works.
	public void RemoveItemFromInventory (int itemindex)
	{
		
		for (int i=0; i<6; ++i)
		{
			if (Buttons[i].activeSelf)
			{				
				Buttons[i].SetActive(false);
				break;
			}	
		}
	}

	public void RemoveIndicator()
	{
		Objective = null;
	}

	private void FadeToClear()
	{
		_fadeinout.color = Color.Lerp(_fadeinout.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	private void FadeToBlack()
	{
		_fadeinout.color = Color.Lerp(_fadeinout.color, Color.black, fadeSpeed * Time.deltaTime);
	}
}