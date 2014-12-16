using UnityEngine;
using System.Collections;

using System;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	SceneSerializer sceneSerializer;
	ObjectMenu objectMenu;
	HeadCursor headCursor;

	enum inputModes {PLAYER, OBJECT, MENU};
	inputModes inputMode;

	void Awake()
	{
	    if (_singleton == null) {
			DontDestroyOnLoad(gameObject);
			_singleton = this;
		} else if (_singleton != this){
			Destroy(this);
		}
		inputMode = inputModes.PLAYER;
	}

	void Start()
	{
		//TODO: This breaks if we change the name/hierarchy of our game objects
	    headCursor = GameObject.Find("OVRPlayerController/OVRCameraRig/CenterEyeAnchor")
			                   .GetComponent<HeadCursor>();
		objectMenu = GameObject.Find ("Menu").GetComponent<ObjectMenu> ();
		objectMoveController = GetComponent<ObjectMovementController> ();
		sceneSerializer = GetComponent<SceneSerializer> ();
	}

	void Update()
	{
		ListenForXboxInput ();
	}

	void ListenForXboxInput()
	{
		//TODO: There ought to be a buttonInputManager class with public variables that we query here.
		//      Collecting button presses is NOT the job of the GameController, especially since we want 
		//      to support multiple input devices (not just an xbox controller).
		/* Collect input */
		float leftX = Input.GetAxis("LJoyHorizontal");
		float leftY = -Input.GetAxis ("LJoyVertical");
		float rightY = -Input.GetAxis ("RJoyVertical");
		float triggers = Input.GetAxis ("RT");
		float LDPadHorizontal = Input.GetAxis ("LDPadHorizontal");
		bool x = Input.GetButton ("X");
		bool b = Input.GetButton ("B");
		bool a = Input.GetButton ("A");
		bool y = Input.GetButton ("Y");
		bool lb = Input.GetButton ("LB");
		bool rb = Input.GetButton ("RB");
		bool lStickPress = Input.GetButtonDown("LStickPress");
		bool rStickPress = Input.GetButtonDown ("RStickPress");
		bool start = Input.GetButtonDown ("Start");

		/* Respond to input depending on inputMode */
		// Object mode lets the player manipulate the selected 3D object
		if (inputMode == inputModes.OBJECT) {
			objectMoveController.DoTranslation(leftX, leftY, rightY);
			objectMoveController.DoRotation(x, b, a, y, lb, rb, lStickPress);
			objectMoveController.DoScaling(triggers); 
			if (rStickPress)
				ChangeInputMode(inputModes.PLAYER);
			else if (start)
				ChangeInputMode(inputModes.MENU);

		// Player mode lets the player move around the scene (and interact with things normally)
		} else if (inputMode == inputModes.PLAYER) {
		    // Movement taken care of by OVRPlayerController
			if (rStickPress)
				ChangeInputMode(inputModes.OBJECT);
			else if (start)
				ChangeInputMode(inputModes.MENU);

		// Menu mode lets the player choose between objects to add to the scene, or to inspect objects
		} else if (inputMode == inputModes.MENU) {
		    if (a) {
				ChangeInputMode(inputModes.OBJECT);
			} else if (start) {
				ChangeInputMode(inputModes.PLAYER);
			} else if (rb){ 
				objectMenu.nextItem();
			} else if (lb){
				objectMenu.previousItem();
			}
		}

	}

	//TODO: This code is too dense, confusing, and difficult to change or correct (should bugs occur).
	//      If I had more time, I would write an inputModeManager that lets the gamecontroller forget 
	//      about inputModes and just be in charge of delegating tasks 

	// First we check the current inputMode, then we respond accordingly to the targetMode.
	void ChangeInputMode (inputModes targetMode)
	{
		if (inputMode == inputModes.PLAYER) {
			if (targetMode == inputModes.OBJECT){
				SwitchToObjectInputMode();
			} else if (targetMode == inputModes.MENU){
				SetPlayerMovementEnabled(false);
				objectMenu.OpenMenu();
			} 
		} else if (inputMode == inputModes.OBJECT){
		    if (targetMode == inputModes.PLAYER){
				sceneSerializer.PrepareTargetObjectForSave(objectMoveController.target.gameObject);
				SwitchToPlayerInputMode();
			} else if (targetMode == inputModes.MENU){
				sceneSerializer.PrepareTargetObjectForSave(objectMoveController.target.gameObject);
				objectMoveController.isSelected = false;
				objectMenu.OpenMenu();
			}
		} else if (inputMode == inputModes.MENU){
		    if (targetMode == inputModes.PLAYER){
				objectMenu.CloseMenu();
				SwitchToPlayerInputMode();
			} else if (targetMode == inputModes.OBJECT){
				SwitchToObjectInputMode (objectMenu.menuItem.transform);
				objectMenu.CloseMenu(false); // WARNING: Closing the menu destroys the menuItem
			}
		}

		inputMode = targetMode; 
		// Redundant except for menu modes. This is evidence that the code structure could be better.

	}

	void SwitchToObjectInputMode (Transform target = null)
	{
		if (target == null){
			// Get target from the direction player is looking
			target = headCursor.GetTargetObjectTransform ();
		} else {
			// Teleport menuItem 2 meters in front of player
			target.position = headCursor.transform.position + (headCursor.transform.forward * 2);
		}

		if (target == null){
			return;
		}

		objectMoveController.target = target;
		objectMoveController.isSelected = true;
		SetPlayerMovementEnabled(false);
		inputMode = inputModes.OBJECT;
	}



	void SwitchToPlayerInputMode ()
	{
		objectMoveController.isSelected = false;
		SetPlayerMovementEnabled (true);
		inputMode = inputModes.PLAYER;
	}

	void SetPlayerMovementEnabled(bool b)
	{
		GameObject.Find ("OVRPlayerController")
			      .GetComponent<OVRPlayerController> ()
				  .enabled = b;
	}

}
