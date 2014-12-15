using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController _singleton;
	ObjectMovementController objectMoveController;
	HeadCursor headCursor;

	void Awake(){
	    if (_singleton == null) {
			DontDestroyOnLoad(gameObject);
			_singleton = this;
		} else if (_singleton != this){
			Destroy(this);
		}
	}

	void Start(){
	    headCursor = GameObject.Find("OVRPlayerController/OVRCameraRig/CenterEyeAnchor")
			                   .GetComponent<HeadCursor>();
		objectMoveController = GetComponent<ObjectMovementController> ();
	}




}
