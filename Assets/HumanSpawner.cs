using UnityEngine;
using System.Collections;

public class HumanSpawner : MonoBehaviour {
	private const int TotalHumans = 5;
	private const double SpawnTime = 10;
	private int _humansSpawned;
	private double _time;

	public Object MalePrefab;
	public Object FemalePrefab;
	public GameObject TargetObject;

	// Use this for initialization
	void Start () {
		_humansSpawned = 0;
		_time = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_humansSpawned < TotalHumans)
		{
			_time += Time.deltaTime;
			if (_time > SpawnTime)
			{
				_time = 0 + Random.Range(0, (int)(SpawnTime / 2));
				SpawnHuman();
			}
		}
	
	}

	void SpawnHuman()
	{
		//print ("Spawning Human");

		Object copyPrefab = MalePrefab;
		//Object copyPrefab = FemalePrefab;
		//if (Random.Range(0, 9) < 5)
			//copyPrefab = MalePrefab;

		GameObject obj = (GameObject)Instantiate (copyPrefab, transform.position, new Quaternion());

		HumanAlwaysWalk comp = obj.GetComponent<HumanAlwaysWalk> ();
		comp.Target = TargetObject;

		obj.transform.LookAt(TargetObject.transform.position);

		comp.StartAnimation();
	}
}
