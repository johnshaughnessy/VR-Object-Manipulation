using UnityEngine;
using System.Collections;

public class ObjectMenu : MonoBehaviour {


	bool isActive; // Menu can be opened/closed if it is active.
	bool isOpen; // Menu will be displayed only if it is open.

	// Use this for initialization
	void Start () 
	{
		isActive = false;
		isOpen = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void SetActive(bool b)
	{
		isActive = b;
	}

	void OpenMenu()
	{
	}

	void CloseMenu()
	{
	}
}
