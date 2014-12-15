using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	HeadCursor headCursor;

	enum inputModes {PLAYER, OBJECT};
	inputModes inputMode;

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
			} else if (inputModes == inputModes.OBJECT){
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
		}
	}

	void SetPlayerMovementEnabled(bool b){
		GameObject.Find ("OVRPlayerController")
			      .GetComponent<OVRPlayerController> ()
				  .enabled = b;
	}

}
