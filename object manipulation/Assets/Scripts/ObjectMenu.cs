using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectMenu : MonoBehaviour {
	public bool isOpen; // Menu will be displayed only if it is open.
	public GameObject menuItem;
	MeshRenderer menuDisplayRenderer;
	Transform menuItemSpawnLocation;

	// Each category (GameObject) is mapped to a list of prefabs that fit into that category.
	// These categories are wrapped in a list, so that we can iterate over them as well.
	List<KeyValuePair<GameObject, List<string>>> prefabs;
	int categoryIndex;
	int prefabIndex;
	/* For clarity, the prefabs list looks like this:
	 *  {
	 *     {Beds(GameObject) : {bed1(string), bed2, bed3} },
	 *     {Tables(GameObject) : {table1, table2, table3, table4} },
	 *     {etc...}
	 *  }
	 */

	// Use this for initialization
	void Start () 
	{
		isOpen = false;
		menuDisplayRenderer = GameObject.Find ("OVRPlayerController/Menu Display")
			                            .GetComponent<MeshRenderer>();
		menuItemSpawnLocation = transform.FindChild ("Menu Item Spawn Location").transform;
		GetPrefabNamesAndCategories ();

	}

	void GetPrefabNamesAndCategories(){
		prefabs = new List<KeyValuePair<GameObject, List<string>>> ();
		Transform root = GameObject.Find ("Furniture").transform;
		for (int i=0; i<root.childCount; i++){
			Transform category = root.GetChild(i);
			List<string> prefabsInThisCategory = new List<string>();
			for (int j=0; j<category.childCount; j++){
				prefabsInThisCategory.Add(category.GetChild(j).name);
			}
			prefabs.Add (new KeyValuePair<GameObject, List<string>>(category.gameObject, prefabsInThisCategory));
		}
		// Start browsing at the "beginning" by setting indices to 0.
		categoryIndex = 0;
		prefabIndex = 0;
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

		string prefabPath = prefabs [categoryIndex].Key.name + "/" + prefabs [categoryIndex].Value [prefabIndex];
		Debug.Log ("ObjectMenu: prefabPath is " + prefabPath);
		menuItem = (GameObject) Instantiate (Resources.Load ("Prefabs/" + prefabPath),
		                                     menuItemSpawnLocation.position,
		                                     menuItemSpawnLocation.rotation);
		menuItem.name = prefabs [categoryIndex].Value [prefabIndex];
		menuItem.transform.parent = prefabs [categoryIndex].Key.transform;

		// TODO: This is just a hack to fix a bug last minute. A better solution should be written.
		menuItem.isStatic = false;
		for (int i=0; i < menuItem.transform.childCount; i++) {
			menuItem.transform.GetChild(i).gameObject.isStatic = false;
		}

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
		prefabIndex = (prefabIndex + 1) % prefabs[categoryIndex].Value.Count;
		OpenMenu ();
	}

	public void previousItem()
	{
		Destroy (menuItem);
		if (prefabIndex == 0){
			prefabIndex = prefabs[categoryIndex].Value.Count;
		}
		prefabIndex = (prefabIndex -1) % prefabs[categoryIndex].Value.Count;
		OpenMenu ();
	}

	public void nextCategory()
	{
		Destroy (menuItem);
		prefabIndex = 0;
		categoryIndex = (categoryIndex + 1) % prefabs.Count;
		OpenMenu ();
	}

	public void previousCategory()
	{
		Destroy (menuItem);
		prefabIndex = 0;
		if (categoryIndex == -1){
			categoryIndex = prefabs.Count; // don't overflow negative
		}
		categoryIndex = (categoryIndex -1) % prefabs.Count;
		OpenMenu ();
	}
}
