using UnityEngine;
using System.Collections;

public class ObjectMovementController : MonoBehaviour {

	public static ObjectMovementController objectMovementController;

	float speed = 2.0f;
	float snapDegrees = 15f;
	public GameObject target;
	public bool isSelected = false;

	void Awake(){
		if (objectMovementController == null){
			objectMovementController = this;
			DontDestroyOnLoad(gameObject);
		} else if (objectMovementController != this){
			Destroy(this);
		}
	}

	void Update(){
		if (target != null && isSelected) ListenForXboxInput();		        
	}
	
	void ListenForXboxInput(){
		/* Collect input */
		float leftX = Input.GetAxis("LJoyHorizontal");
		float leftY = -Input.GetAxis ("LJoyVertical");
		float rightY = -Input.GetAxis ("RJoyVertical");
		float rTrigger = Input.GetAxis ("RT");
		float deadzone = .5f; // Use deadzone to make sure player meant to move axes.
		bool x = Input.GetButtonDown ("X");
		bool b = Input.GetButtonDown ("B");
		bool a = Input.GetButtonDown ("A");
		bool y = Input.GetButtonDown ("Y");
		bool lb = Input.GetButtonDown ("LB");
		bool rb = Input.GetButtonDown ("RB");
		bool snapRotation = Input.GetButton("LStickPress");
		// TODO: This breaks if we change the name/hierarchy of game objects
		Vector3 forward =  GameObject.Find("OVRPlayerController/OVRCameraRig/CenterEyeAnchor").transform.forward;
		Vector3 horizontal = Vector3.Cross(forward, Vector3.up);
		
		
		/* Movement */
		// Move speed is proportional to object size so that object placement is easier.
		float moveCoefficient = Time.deltaTime * speed * Mathf.Sqrt(target.transform.localScale.magnitude); 
		if (Mathf.Abs(leftX) > deadzone){
			target.transform.position += (-horizontal * leftX * moveCoefficient); 
		}
		if (Mathf.Abs(leftY) > deadzone){
			target.transform.position += (forward * leftY * moveCoefficient);
		}
		
		if (Mathf.Abs (rightY) > deadzone){
			target.transform.position += (Vector3.up * rightY * moveCoefficient);
		}
		
		/* Rotation */
		if (x) target.transform.Rotate (target.transform.InverseTransformDirection(Vector3.up * snapDegrees));
		if (b) target.transform.Rotate (target.transform.InverseTransformDirection(Vector3.up * -snapDegrees));
		if (a) target.transform.Rotate (target.transform.InverseTransformDirection(horizontal * snapDegrees));
		if (y) target.transform.Rotate (target.transform.InverseTransformDirection(horizontal * -snapDegrees));
		if (lb) target.transform.Rotate (target.transform.InverseTransformDirection(forward * snapDegrees));
		if (rb) target.transform.Rotate (target.transform.InverseTransformDirection(forward * -snapDegrees));
		
		/* Scaling */
		if (rTrigger < -deadzone)
			target.transform.localScale += new Vector3(.1f,.1f,.1f) * target.transform.localScale.magnitude/10.0f;
		if (rTrigger > deadzone){
			if (target.transform.localScale.magnitude < .2) // TODO: minimum size is currently arbitrary.
				// TODO: should objects be collected before being destroyed?
				Destroy(target);
			else 
				target.transform.localScale -= new Vector3(.1f,.1f,.1f) * target.transform.localScale.magnitude/10.0f;
		}
		
		/* Snap to multiples of incremental rotations */
		if (snapRotation){
			target.transform.eulerAngles = RoundToSnapMultiple(target.transform.eulerAngles, (int) snapDegrees);
		}
	}
	
	Vector3 RoundToSnapMultiple(Vector3 roundMe, int snap){
		roundMe.x = RoundToSnapMultiple((int)roundMe.x, snap);
		roundMe.y = RoundToSnapMultiple((int)roundMe.y, snap);
		roundMe.z = RoundToSnapMultiple((int)roundMe.z, snap);
		return roundMe;
	}
	
	int RoundToSnapMultiple(int roundMe, int snap){
		if (roundMe % snap < snap/2){
			roundMe = (roundMe / snap) * snap; // round down (discard remainder)
		} else {
			roundMe = (roundMe / snap) * (snap+1); // round up
		}
		return roundMe;
	}
}
