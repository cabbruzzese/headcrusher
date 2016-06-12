using UnityEngine;
using System.Collections;

public class SquishyTarget : MonoBehaviour {

	private const double DeathTimeLimit = 5;
	private double _timeAlive;
	private bool _dead;

	public GameObject Owner;
	public ParticleSystem Particles;

	// Use this for initialization
	void Start () {
		_dead = false;
		_timeAlive = 0;

		Particles.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (_dead) 
		{
			_timeAlive += Time.deltaTime;

			if (_timeAlive > DeathTimeLimit)
			{
				Destroy(Owner);
			}
		}
	}

	public void SquishHead()
	{
		_dead = true;

		Particles.Play();

		HumanAlwaysWalk logic = Owner.GetComponent<HumanAlwaysWalk> ();
		if (logic != null)
		{
			logic.Kill();
		}
	}

	public bool IsDead
	{
		get { return _dead; }
	}
}
