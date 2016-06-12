using UnityEngine;
using System.Collections;

public class ProjectileThink : MonoBehaviour {

	private double _aliveTime;
	private const double _timeLimit = 2;

	// Use this for initialization
	void Start () {
		_aliveTime = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_aliveTime += Time.deltaTime;
		if (_aliveTime > _timeLimit)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!collision.gameObject.CompareTag ("MainCamera")) 
		{
			Destroy(gameObject);
		}

	}
}
