using UnityEngine;
using System.Collections;

//      This class is simple now because it doesn't manage drawing a cursor or anything.
//TODO: Does this even need its own class?
public class HeadCursor : MonoBehaviour {

	public Transform GetTargetObjectTransform()
	{
		RaycastHit hit;
		// TODO: Arbitrary raycast length of 200.0f
		if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f)){ 
			return hit.transform.gameObject.transform;
		} 
		return null;
	}
}
