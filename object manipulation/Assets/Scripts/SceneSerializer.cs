using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SceneSerializer : MonoBehaviour {
	public static SceneSerializer _singleton;

	// Serialize gameobjects to save changes to scene made by user
	GameObjectTransformData[] saveMe; // Serializable
	List<GameObjectTransformData> objectsToSave; // Not serializable
	string dataPath;


	void Awake () 
	{
		if (_singleton == null) {
			DontDestroyOnLoad(gameObject);
			_singleton = this;
		} else if (_singleton != this){
			Destroy(this);
		}
	}

	void Start()
	{
		objectsToSave = new List<GameObjectTransformData> ();
		dataPath = Application.persistentDataPath 
		                      + "/TransformsForScene" 
		                      + Application.loadedLevelName 
		                      + ".dat";
		string persistantDataMessage = "Changes you make to the scene will persist even after you restart the application!\n" +
						"To erase changes you made, turn off the application, delete " + dataPath + " and try again.";
		Debug.Log (persistantDataMessage);
		Load ();
	}

	void OnApplicationQuit()
	{
		Save ();
	}

	public void PrepareTargetObjectForSave (GameObject target)
	{
		if (target != null){
			GameObjectTransformData data = new GameObjectTransformData(target);
			// Check for duplicate before adding data
			for (int i=0; i<objectsToSave.Count; i++){
				if (objectsToSave[i].getInstanceID() == data.getInstanceID()){
					objectsToSave.RemoveAt(i);
					i--; // Decrement the counter since we're changing the list
				}
			}
			objectsToSave.Add(data);
		}
	}

	void Save()
	{
		saveMe = objectsToSave.ToArray (); // Convert to serializable container type
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (dataPath);
		bf.Serialize (file, saveMe);
		file.Close ();
	}
	
	void Load()
	{
		if (File.Exists(dataPath)){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (dataPath, FileMode.Open);
			saveMe = (GameObjectTransformData[]) bf.Deserialize (file);
			
			// Instantiate each saved object
			for (int i=0; i<saveMe.Length; i++){
				GameObjectTransformData data = saveMe[i];
				GameObject obj = (GameObject) Instantiate(Resources.Load ("Prefabs/" + data.getName(), typeof(GameObject)),
				                                          data.getPosition(),
				                                          data.getRotation());
				obj.name = data.getName(); // To avoid unwanted "(Clone)" appended to name of objects.
				obj.transform.localScale = data.getScale();
				data = new GameObjectTransformData(obj); // get a new instanceID to ensure it's unique for this playthrough
				objectsToSave.Add(data); // Make sure we save this for next time, too
			}
		}
	}

	public void SetDataPath(string path)
	{
		dataPath = path;
	}
}
