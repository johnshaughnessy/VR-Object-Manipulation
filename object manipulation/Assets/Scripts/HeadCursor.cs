using UnityEngine;
using System.Collections;

public class HeadCursor : MonoBehaviour {

	public GameObject GetTargetObject(){
		Vector3 forward = transform.forward;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, forward, out hit, 100.0f)){
			return hit.transform.gameObject;
		} 
		return null;
	}
}
