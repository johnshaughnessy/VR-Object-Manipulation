﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	HeadCursor headCursor;

	enum inputModes {PLAYER, OBJECT};
	inputModes inputMode;

	// Serialize gameobjects to save changes to scene made by user
	GameObjectTransformData[] saveMe; // Serializable
	List<GameObjectTransformData> objectsToSave; // Not serializable
	string dataPath;

	void Awake(){
	    if (_singleton == null) {
			DontDestroyOnLoad(gameObject);
			_singleton = this;
		} else if (_singleton != this){
			Destroy(this);
		}
		inputMode = inputModes.PLAYER;
	}

	void Start(){
	    headCursor = GameObject.Find("OVRPlayerController/OVRCameraRig/CenterEyeAnchor")
			                   .GetComponent<HeadCursor>();
		objectMoveController = GetComponent<ObjectMovementController> ();
		dataPath = Application.persistentDataPath + "/objectTransforms.dat";
	}

	void Update(){
		ListenForInputModeChange ();
	}

	void Save(){
		saveMe = objectsToSave.ToArray (); // Convert to serializable container type
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (dataPath);
		bf.Serialize (file, saveMe);
		file.Close ();
	}

	void Load(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (dataPath, FileMode.Create);
		saveMe = (GameObjectTransformData[]) bf.Deserialize (file);
		objectsToSave = new List<GameObjectTransformData> ();
		// Instantiate each saved object
		for (int i=0; i<saveMe.Length; i++){
			GameObjectTransformData data = saveMe[i];
			objectsToSave.Add(data); // Make sure we save this for next time, too
			GameObject obj = Instantiate(Resources.Load ("Prefabs/" + data.getName(), typeof(GameObject)),
			                             data.getPosition(),
			                             data.getRotation());
			obj.name = data.getName(); // To avoid unwanted "(Clone)" appended to name of objects.
			obj.transform.localScale = data.getScale();
		}

	}

	void ListenForInputModeChange ()
	{
		if (Input.GetButtonDown ("RStickPress")) {
		    if (inputMode == inputModes.PLAYER){
				SwitchToObjectInputMode();
			} else if (inputMode == inputModes.OBJECT){
				SaveTargetObject();
				SwitchToPlayerInputMode();
			}
		}
	}

	void SwitchToObjectInputMode ()
	{
		GameObject target = headCursor.GetTargetObject ();
		if (target != null){
			objectMoveController.target = target;
			objectMoveController.isSelected = true;
			SetPlayerMovementEnabled(false);
			inputMode = inputModes.OBJECT;
		}
	}

	void SaveTargetObject ()
	{
		if (objectMoveController.target != null){
			GameObjectTransformData data = new GameObjectTransformData(objectMoveController.target);
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

	void SwitchToPlayerInputMode ()
	{
		objectMoveController.isSelected = false;
		SetPlayerMovementEnabled (true);
		inputMode = inputModes.PLAYER;
	}

	void SetPlayerMovementEnabled(bool b){
		GameObject.Find ("OVRPlayerController")
			      .GetComponent<OVRPlayerController> ()
				  .enabled = b;
	}

}
