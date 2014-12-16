using UnityEngine;
using System.Collections;

public class ObjectMenu : MonoBehaviour {
	public bool isOpen; // Menu will be displayed only if it is open.

	string[] prefabNames;
	int index;
	public GameObject menuItem;
	MeshRenderer menuDisplayRenderer;
	Transform menuItemSpawnLocation;

	// Use this for initialization
	void Start () 
	{
		isOpen = false;
		menuDisplayRenderer = GameObject.Find ("OVRPlayerController/Menu Display")
			                            .GetComponent<MeshRenderer>();
		// TODO: This should be done programatically because game objects could be pulled in from anywhere, 
		//       and we already make the assumption that we save those objects as prefabs in Resources/Prefabs
		prefabNames = new string[] {"Beut Tree", "Floor", "Fun Box"};
		int index = 0;
		menuItemSpawnLocation = transform.FindChild ("Menu Item Spawn Location").transform;
	}
	

	public void ToggleMenu()
	{
		if (isOpen) 
			CloseMenu();
		else 
			OpenMenu();
	}

	public void OpenMenu()
	{
		menuItem = (GameObject) Instantiate (Resources.Load ("Prefabs/" + prefabNames[index]),
		                                     menuItemSpawnLocation.position,
		                                     menuItemSpawnLocation.rotation);
		menuItem.name = prefabNames [index];
		menuDisplayRenderer.enabled = true;
		isOpen = true;
	}

	public void CloseMenu(bool destroyObject = true)
	{
		menuDisplayRenderer.enabled = false;
		if (destroyObject){
			Destroy (menuItem);
		}

		isOpen = false;
	}

	public void nextItem()
	{
		Destroy (menuItem);
		index = (index + 1) % prefabNames.Length;
		OpenMenu ();
	}

	public void previousItem()
	{
		Destroy (menuItem);
		if (index == 0)
			index = prefabNames.Length; // We don't want to go negative
		index = (index - 1) % prefabNames.Length;
		OpenMenu ();
	}
}
