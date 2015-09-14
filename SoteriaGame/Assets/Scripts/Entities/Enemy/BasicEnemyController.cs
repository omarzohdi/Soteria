﻿using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {

	public GameObject player;
	private NavMeshAgent _agent;
	private float _distance = 0.0f;
	private Animator _anim;
	private bool _dead = false;
	private bool _stunned = false;
	private int _opCounter = 1;

	public Transform[] patrolLocations;
	private int _patrolIndex = 0;
	public float chaseSpeed = 10.0f;
	public float patrolSpeed = 5.0f;
	private float _patrolTimer = 0.0f;
	public float waitTime = 5.0f;
	private float _stunDuration;
	public float stunTimer = 5.0f;

	private bool _playerVisible;
	public float fieldOfVision = 90.0f;
	private SphereCollider _sphereCollider;
	
	public float lookAtDistance = 45.0f;
	public float attackRange = 35.0f;
	public float overwhelmRange = 15.0f;
	
	// Use this for initialization
	public void Initialize()
	{
		if (player == null)
		{
			player = GameDirector.instance.GetPlayer();
		}
		_agent = GetComponent<NavMeshAgent> ();
		_anim = GetComponent<Animator> ();
		this._sphereCollider = GetComponent<SphereCollider> ();
		this._stunDuration = this.stunTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_distance = Vector3.Distance(this.transform.position, player.transform.position);
		if (this._stunned)
		{
			this._agent.Stop();
			this.Stunned();
		}
		else if (this._playerVisible)
		{
//			if (this._distance <= this.overwhelmRange)
//			{
//				GameDirector.instance.Encounter(this.gameObject);
//				this.OverwhelmPlayer();
//			}
//			else if (this._distance <= this.attackRange)
//			{
//				this.ChasePlayer();
//			}
//			else if (this._distance <= this.lookAtDistance)
//			{
//				this.LookAtPlayer();
//			}
//			else
//			{
//				this.Unaware();
//			}
		}
		else
		{
			this.Unaware();
		}
	}

	void OnTriggerStay(Collider player)
	{
		if (player.gameObject.tag == "Player")
		{
			this._playerVisible = false;
			
			Vector3 direction = player.transform.position - this.gameObject.transform.position;
			float angle = Vector3.Angle(direction, this.gameObject.transform.forward);
			
			if(angle < fieldOfVision * 0.5f)
			{
				RaycastHit hit;
				
				if(Physics.Raycast(this.gameObject.transform.position + this.gameObject.transform.up, direction, out hit, this._sphereCollider.radius))
				{
					if(hit.collider.gameObject.Equals(player.gameObject))
					{
						this._playerVisible = true;
						if (this._distance <= this.overwhelmRange)
						{
							GameDirector.instance.Encounter(this.gameObject);
							this.OverwhelmPlayer();
						}
						else if (this._distance <= this.attackRange)
						{
							this.ChasePlayer();
						}
						else if (this._distance <= this.lookAtDistance)
						{
							this.LookAtPlayer();
						}
						else
						{
							this.Unaware();
						}
					}
				}
				//Debug.DrawRay(this.gameObject.transform.position + this.gameObject.transform.up, direction, Color.white, 200, false);
			}
		}
	}
	
	public void LookAtPlayer()
	{
		_anim.SetBool ("Alert", true);
		this._agent.Stop();
		this.transform.LookAt(player.transform.position);
	}
	
	public void ChasePlayer()
	{
		_agent.Resume();
		_agent.speed = chaseSpeed;
		_anim.SetBool ("Aggro", true);
		_anim.SetBool ("Alert", false);
		_anim.SetBool ("Moving", false);
		_agent.SetDestination (player.transform.position);
		//Debug.Log("Enemy chasing");
	}
	
	public void OverwhelmPlayer()
	{
		_agent.Stop();
	}
	
	public void Unaware()
	{
		_agent.Resume();
		_anim.SetBool ("Aggro", false);
		_anim.SetBool ("Alert", false);
		_anim.SetBool ("Overpower", false);
		_anim.SetBool ("Cower", false);
		_opCounter = 1;
		if (patrolLocations.Length > 0)
		{
			_anim.SetBool ("Moving", true);
			_agent.speed = patrolSpeed;
			if (_agent.remainingDistance < _agent.stoppingDistance)
			{
				_anim.SetBool ("Moving", false);
				_patrolTimer += Time.deltaTime;
				if (_patrolTimer >= waitTime)
				{
					if (_patrolIndex == patrolLocations.Length - 1)
					{
						_patrolIndex = 0;
					}
					else
					{
						_patrolIndex++;
					}
					_patrolTimer = 0.0f;
				}
			}

			_agent.destination = patrolLocations [_patrolIndex].position;
		}
		else
		{
			_anim.SetBool ("Moving", false);
		}
	}
	
	public float GetDistance()
	{
		return this._distance;
	}

	public void Stun()
	{
		this._stunned = true;
	}

	public void Overpower()
	{
		switch (_opCounter)
		{
		case 1:
			_anim.SetBool ("Overpower", true);
			break;
		case 2:
			_anim.SetBool ("OP 2", true);
			break;
		case 3:
			_anim.SetBool ("OP 3", true);
			break;
		}
	}

	public void ResetOverpower()
	{
		_anim.SetBool ("Overpower", false);
		_anim.SetBool ("OP 2", false);
		_anim.SetBool ("OP 3", false);
	}

	public void Cower()
	{
		_anim.SetBool ("Cower", true);
		_dead = true;
		_opCounter = 1;
	}

	public void DestroyMe()
	{
		GameDirector.instance.KillEnemy();
	}

	public void NextOPStage()
	{
		_opCounter++;
	}

	private void Stunned()
	{
		this._stunDuration -= Time.deltaTime;
		if (this._stunDuration <= 0)
		{
			this._stunned = false;
			this._stunDuration = stunTimer;
			_agent.Resume();
		}
	}

	public NavMeshAgent GetAgent()
	{
		return this._agent;
	}
}