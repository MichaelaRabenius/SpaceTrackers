using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsInScreenCircle : MonoBehaviour {
	//Array to store hit elements
	private Collider[] hitColliders;

	//Radius of collision sphere
	private float radius = 70f;

	//Name that differentiates enemies from other objects
	private string enemyName = "Cube";

	//The centerposition of screen
	Vector3 groundCenter;

	//Array containing the lockon status for the other players
	private bool[] alliedStatus;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Get all objects inside the OverlapSphere
		getObjectsInCenter (transform.position, radius);

		//Loop through them
		for (int i = 0; i < hitColliders.Length; i++) {
			//If the object is of the right type
			if(hitColliders[i].gameObject.name.Contains(enemyName))
				print (hitColliders [i].gameObject.name);
		}
			
	}

	//Gets all the objects inside the radius of the sphere
	void getObjectsInCenter(Vector3 center, float radius){
		groundCenter = new Vector3 (center.x, 0f, center.z);
		hitColliders = Physics.OverlapSphere (groundCenter, radius);
	}
		
	//Debug, shows the overlapSphere
	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
			//Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.

			Gizmos.DrawWireSphere (groundCenter, radius);
	}
}
