using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	HeadCursor headCursor;

	enum inputModes {PLAYER, OBJECT};
	inputModes inputMode;

	// Serialize gameobjects to save changes to scene made by user
	GameObjectTransformData[] saveMe;
	List<GameObjectTransformData> objectsToSave;

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
	}

	void Update(){
		ListenForInputModeChange ();
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
			int numChecked = 0;
			for (int i=0; i<objectsToSave.Count; i++){
			    if (objectsToSave[i].getInstanceID() == data.getInstanceID){
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
