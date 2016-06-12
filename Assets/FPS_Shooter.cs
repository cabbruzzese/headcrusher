using UnityEngine;
using System.Collections;

public class FPS_Shooter : MonoBehaviour
{
	public GameObject bulletPrefab;
	private double _timer;
	private const double _timeDelay = 0.5;
	private const float _speed = 450;

	// Use this for initialization
	void Start ()
	{
		_timer = 0;
	}


	// Update is called once per frame
	void Update ()
	{
		if (_timer <= _timeDelay)
			_timer += Time.deltaTime;

		if (Input.GetMouseButtonDown(0) && _timer > _timeDelay)
	    {
			//_timer = 0;
			//ShootFireBall();
		}
	}

	void ShootFireBall()
	{
		print ("Shooting Fireball!");
		GameObject obj = (GameObject)Instantiate (bulletPrefab, transform.position, new Quaternion());

		Vector3 vel = Camera.main.transform.TransformDirection (Vector3.forward);
		Vector3 v_up = Camera.main.transform.TransformDirection (Vector3.up);

		obj.GetComponent<Rigidbody>().AddForce (vel * _speed);
		obj.GetComponent<Rigidbody>().AddForce (v_up * (_speed / 4));

		Physics.IgnoreCollision (obj.GetComponent<Collider>(), Camera.main.GetComponent<Collider>());
	}
}