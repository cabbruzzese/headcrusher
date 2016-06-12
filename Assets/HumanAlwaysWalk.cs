using UnityEngine;
using System.Collections;

public class HumanAlwaysWalk : MonoBehaviour {

	private const double WalkSpeed = 20;
	private const double MaxSpeed = 20;
	private const double AliveTimeMax = 50;
	private double _walkspeed;
	private const float InclineLimit = 0.4f;
	private double _aliveTime;
	private GameObject _target;
	private bool _dead;

	public GameObject AnimatedModel;

	// Use this for initialization
	void Start () 
	{
		_aliveTime = 0;
		_walkspeed = WalkSpeed - Random.Range (0, 10);

		_dead = false;
	}

	public void StartAnimation()
	{
        AnimatedModel.GetComponent<Animation>().Play("Action_001");
	}

	// Update is called once per frame
	void Update () 
	{
		_aliveTime += Time.deltaTime;
		if (_target != null && !_dead)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < MaxSpeed)
			{
				transform.LookAt(_target.transform.position);

				Vector3 forward = transform.TransformDirection(Vector3.forward);
				forward.Normalize();
				forward = (forward * (float)_walkspeed);
				GetComponent<Rigidbody>().AddForce(forward);

				if(GetComponent<Rigidbody>().velocity.y > InclineLimit)
				{
					Vector3 vel = GetComponent<Rigidbody>().velocity;
					vel.y = InclineLimit;
					GetComponent<Rigidbody>().velocity = vel;
				}
			}
		}

		if (_aliveTime >= AliveTimeMax)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		//print ("colliding");
		if (collision.gameObject == _target) 
		{
			//print ("hit target");
			Destroy(gameObject);
		}
		else if (!collision.gameObject.CompareTag("Floor"))
		{
			Vector3 right = transform.TransformDirection(Vector3.right);
			right.Normalize();
			right *= (float)(_walkspeed / 5);
			GetComponent<Rigidbody>().AddForce(right);
		}
		
	}

	public GameObject Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public void Kill()
	{
		_dead = true;
        AnimatedModel.GetComponent<Animation>().CrossFade("Action_002", 0.3f, PlayMode.StopAll);

        transform.LookAt(_target.transform.position);
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().freezeRotation = true;
	}
}
