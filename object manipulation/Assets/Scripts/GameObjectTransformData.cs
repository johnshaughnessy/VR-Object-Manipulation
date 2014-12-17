using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameObjectTransformData
{
	string name;
	int instanceID;

	float positionX;
	float positionY;
	float positionZ;

	float rotationW;
	float rotationX;
	float rotationY;
	float rotationZ;

	float scaleX;
	float scaleY;
	float scaleZ;

	public GameObjectTransformData(GameObject obj)
	{
		if (obj.transform.parent != null){
			name = obj.transform.parent.name + "/" + obj.name;
		} else {
			name = obj.name;
		}

		instanceID = obj.GetInstanceID ();

		positionX = obj.transform.position.x;
		positionY = obj.transform.position.y;
		positionZ = obj.transform.position.z;
		rotationW = obj.transform.rotation.w;
		rotationX = obj.transform.rotation.x;
		rotationY = obj.transform.rotation.y;
		rotationZ = obj.transform.rotation.z;

		scaleX = obj.transform.localScale.x;
		scaleY = obj.transform.localScale.y;
		scaleZ = obj.transform.localScale.z;
	}

	public string getName()
	{
		return name;
	}

	public int getInstanceID()
	{
		return instanceID;
	}

	public Vector3 getPosition()
	{
		return new Vector3(positionX, positionY, positionZ);
	}

	public Quaternion getRotation()
	{
		return new Quaternion(rotationX, rotationY, rotationZ, rotationW);
	}

	public Vector3 getScale()
	{
		return new Vector3(scaleX, scaleY, scaleZ);
	}


}

