﻿using UnityEngine;
using System.Collections;

public class ObjectMovementController : MonoBehaviour {
	public static ObjectMovementController objectMovementController;

	public Transform target;
	public Transform pov; // Player's pov (CenterEyeAnchor)
	Vector3 forward;
	Vector3 horizontal;
	public bool isSelected = false;
	float translationSpeed = 1.5f;
	float rotationSpeed = 3.0f; 
	int rotationSnapFactor = 15; // Snaps rotation to some multiple of this on button press
	float deadzone = .5f; // Use deadzone to ensure player intentionally moved axes.

	void Awake()
	{
		if (objectMovementController == null){
			objectMovementController = this;
			DontDestroyOnLoad(gameObject);
		} else if (objectMovementController != this){
			Destroy(this);
		}
	}

	void Start()
	{
		// TODO: This breaks if we change the name/hierarchy of game objects
		pov = GameObject.Find ("OVRPlayerController/OVRCameraRig/CenterEyeAnchor").transform;
	}

	void Update()
	{
		forward =  pov.forward;
		horizontal = Vector3.Cross(forward, Vector3.up);	        
	}

	/* Movement */
	public void DoTranslation (float leftX, float leftY, float rightY)
	{
		if (target != null && isSelected) {
			// Move speed is proportional to object size so that object placement is easier.
			float moveCoefficient = Time.deltaTime * translationSpeed * Mathf.Sqrt(target.localScale.magnitude); 
			if (Mathf.Abs(leftX) > deadzone){
				target.position += (-horizontal * leftX * moveCoefficient); 
			}
			if (Mathf.Abs(leftY) > deadzone){
				target.position += (forward * leftY * moveCoefficient);
			}
			
			if (Mathf.Abs (rightY) > deadzone){
				target.position += (Vector3.up * rightY * moveCoefficient);
			}
		}
	}

	/* Rotation */
	public void DoRotation (bool x = false, bool b = false, 
	                        bool a = false, bool y = false, 
	                        bool lb = false, bool rb = false,
	                        bool snapRotation = false)
	{
		if (target != null && isSelected) {
			if (x) target.Rotate (target.InverseTransformDirection(Vector3.up * rotationSpeed));
			if (b) target.Rotate (target.InverseTransformDirection(Vector3.up * -rotationSpeed));
			if (a) target.Rotate (target.InverseTransformDirection(horizontal * rotationSpeed));
			if (y) target.Rotate (target.InverseTransformDirection(horizontal * -rotationSpeed));
			if (lb) target.Rotate (target.InverseTransformDirection(forward * rotationSpeed));
			if (rb) target.Rotate (target.InverseTransformDirection(forward * -rotationSpeed));
			
			/* Snap to multiples of incremental rotations */
			if (snapRotation){
				target.eulerAngles = RoundToSnapMultiple(target.eulerAngles, rotationSnapFactor); 
			}
		}

	}

	/* Scaling */
	public void DoScaling(float triggers)
	{
		if (target != null && isSelected) {
			if (triggers < -deadzone)
				target.localScale += new Vector3(.1f,.1f,.1f) * target.localScale.magnitude/10.0f;
			if (triggers > deadzone){
				if (target.localScale.magnitude < .01) // TODO: minimum size is currently arbitrary.
					// TODO: should objects be collected in inventory before being destroyed?
					Destroy(target.gameObject);
				else 
					target.localScale -= new Vector3(.1f,.1f,.1f) * target.localScale.magnitude/10.0f;
			}
		}
	}

	// Helper method for rotation
	Vector3 RoundToSnapMultiple(Vector3 roundMe, int snap)
	{
		roundMe.x = RoundToSnapMultiple((int)roundMe.x, snap);
		roundMe.y = RoundToSnapMultiple((int)roundMe.y, snap);
		roundMe.z = RoundToSnapMultiple((int)roundMe.z, snap);
		return roundMe;
	}

	// Helper method for rotation
	int RoundToSnapMultiple(int roundMe, int snap)
	{
		if (roundMe % snap < snap/2){
			roundMe = (roundMe / snap) * snap; // round down (discard remainder)
		} else {
			roundMe = (roundMe / snap) * (snap+1); // round up
		}
		return roundMe;
	}
}
