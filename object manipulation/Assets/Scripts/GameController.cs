using UnityEngine;
using System.Collections;

using System;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	SceneSerializer sceneSerializer;
	HeadCursor headCursor;

	enum inputModes {PLAYER, OBJECT};
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
		objectMoveController = GetComponent<ObjectMovementController> ();
		sceneSerializer = GetComponent<SceneSerializer> ();
	}

	void Update()
	{
		ListenForXboxInput ();
	}

	void ListenForXboxInput()
	{
		/* Collect input */
		float leftX = Input.GetAxis("LJoyHorizontal");
		float leftY = -Input.GetAxis ("LJoyVertical");
		float rightY = -Input.GetAxis ("RJoyVertical");
		float triggers = Input.GetAxis ("RT");
		bool x = Input.GetButtonDown ("X");
		bool b = Input.GetButtonDown ("B");
		bool a = Input.GetButtonDown ("A");
		bool y = Input.GetButtonDown ("Y");
		bool lb = Input.GetButtonDown ("LB");
		bool rb = Input.GetButtonDown ("RB");
		bool lStickPress = Input.GetButton("LStickPress");
		bool rStickPress = Input.GetButton ("RStickPress");

		if (inputMode == inputModes.OBJECT) {
			objectMoveController.DoTranslation(leftX, leftY, rightY);
			objectMoveController.DoRotation(x,b,a,y,lb,rb,lStickPress);
			objectMoveController.DoScaling(triggers); 
		}

		if (rStickPress){
			ChangeInputMode ();
		}

		// Nothing to do if inputMode == inputModes.PLAYER
		// because the OVRPlayerController takes care of this
	}

	void ChangeInputMode ()
	{
		if (inputMode == inputModes.PLAYER){
			SwitchToObjectInputMode();
		} else if (inputMode == inputModes.OBJECT){
			sceneSerializer.PrepareTargetObjectForSave(objectMoveController.target.gameObject);
			SwitchToPlayerInputMode();
		}
	}

	void SwitchToObjectInputMode ()
	{
		Transform target = headCursor.GetTargetObjectTransform ();
		if (target != null){
			objectMoveController.target = target;
			objectMoveController.isSelected = true;
			SetPlayerMovementEnabled(false);
			inputMode = inputModes.OBJECT;
		}
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
