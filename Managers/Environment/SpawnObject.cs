using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

	public Transform[] spawnPoints;
	//spawnpoints
	//	public float speed = 30f;

	[System.Serializable]
	public class entity
	{
 
		public string name;
		public GameObject prefab;
		public float delayTime;
		//a delay before they can start spawning
 
	}

	public entity[] entities;
	public float spawnDelay = 1.0f;
 
	entity chosenEntity;
 
	float random;
 
	float cumulative;
	public string selectedEntity;
	//debug
 
  
 
	void Start ()
	{
		SpawnObjects ();
	}

	void SpawnObjects ()
	{
		Debug.Log ("Spawning Object", this);
		chosenEntity = entities [0];
		int spawnPointIndex1 = Random.Range (0, spawnPoints.Length);
		Instantiate (entities [0].prefab, spawnPoints [spawnPointIndex1].position + transform.forward, spawnPoints [spawnPointIndex1].rotation);
	}
}