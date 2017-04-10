using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toucher : MonoBehaviour {

    Ray ray;
    RaycastHit hit; 
	public GameObject explosion;
	public GameObject bigExplosion;
	public GameObject rocket;
	public GameObject fancyStuff;
	public float speed;
	public float lifetime;
	public float slowDownTime;




	void Update () {
		
        if(Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began)// if you've touched something
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //shoots a ray from the camera to the touch position

			if(Physics.Raycast(ray,out hit, Mathf.Infinity)) //Did we hit sometihng?
			{
				Debug.Log("Omg we hit something");

				if (hit.transform.gameObject.tag == "Ship") //Does the object have the tag Ship?
				{
					Destroy (hit.transform.gameObject);//Destroy it
					Instantiate (explosion, hit.point, Quaternion.identity); //Instantiate explosion at object pos
				}
				if (hit.transform.gameObject.tag == "Boll") 
				{
					StartCoroutine (SlowTime ());
					Destroy (hit.transform.gameObject);

				}

				if (hit.transform.gameObject.tag == "Hejsan") //Check the tag
				{
					StartCoroutine (SpawnRocket ()); //Call the function as a coroutine
					Invoke ("KillAll", lifetime);	//Call the function after a lifetime seconds delay
					Destroy (hit.transform.gameObject); //Destroy the object hit
					Instantiate(fancyStuff, hit.point, Quaternion.identity);
				}
				if (hit.transform.gameObject.tag == "Stop") 
				{
					StartCoroutine (StopTime ());
					Destroy (hit.transform.gameObject);
				}

		    }
		
        }

	}

	IEnumerator SpawnRocket()
	{
		GameObject spawn = GameObject.FindGameObjectWithTag("RSL");
		GameObject go = Instantiate (rocket, spawn.transform.position, spawn.transform.rotation) as GameObject;
		go.GetComponent<Rigidbody> ().AddForce (transform.up * speed, ForceMode.Impulse); //Shoot rocket cus why not
		Destroy (go, lifetime); //Destroy the tocket after its lifetime
		yield return new WaitForSeconds (lifetime-0.1f); //Wait for lifetime-0.1 seconds
		Instantiate (bigExplosion, go.transform.position,Quaternion.identity);//Explode

	}
	void KillAll()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Ship"); //Save all gameobjects with the tag Ship in an array
		for (int i = 0; i < enemies.Length; i++)
		{
			Destroy (enemies [i]); //Destroy all these objects	
			Instantiate (explosion,enemies[i].gameObject.transform.position , Quaternion.identity); //Spawn explosion at all enemies pos

		}
	}
	IEnumerator SlowTime()
	{
		Time.timeScale = 0.5f; //Slow down time by half
		yield return new WaitForSeconds (slowDownTime); //Wait for slowTime seconds
		Time.timeScale = 1; //Back to normal time
	}
	IEnumerator StopTime()
	{
		Time.timeScale = 0; //Stops time
		float pauseEndTime = Time.realtimeSinceStartup +3;
		while (Time.realtimeSinceStartup < pauseEndTime) //Waits for 3 seconds maybe??	
		{
			yield return 0;
		}

		Time.timeScale = 1; //Normal time 
	}
}
