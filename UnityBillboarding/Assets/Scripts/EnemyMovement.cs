using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {


	//Transform m_Target;
	Vector3 m_TargetPosition;
	Transform m_MyLocation;

	public float m_MoveSpeed = 3.0f;
	public float m_RotationSpeed = 3.0f;

	//public int m_ChaseTimer = 5;

	bool m_PlayerDetected = true;
	bool m_AtTarget = true;

	void Awake(){

		m_MyLocation = transform; //cache transform data for easy access/preformance
    }


	// Use this for initialization
	void Start () {

		//m_Target = GameObject.FindWithTag("Player").transform; //target the player

	}
	
	// Update is called once per frame
	void Update () {
		

		// if player pings enemy
		// m_PlayerDetected = true;
		// attack sound
		// chase music
		// start chase timer

		//Pulse function on pulse_timer

		// if chase timer <= 0 and player is hidden
		// m_PlayerDetected = false;


		Move();
		PlaySound();
		
	}

	void Move()
	{

		if (m_PlayerDetected)
		{
			
			if (m_AtTarget)
			{
				m_TargetPosition = GameObject.FindWithTag("Player").transform.position;
				m_AtTarget = false;
			}

			Quaternion rot_from = m_MyLocation.rotation;
			Quaternion rot_to = Quaternion.LookRotation(m_TargetPosition - m_MyLocation.position);
	
			m_MyLocation.rotation = Quaternion.Slerp(rot_from, rot_to, m_RotationSpeed*Time.deltaTime); // rotate to face player
			m_MyLocation.position += m_MyLocation.forward * m_MoveSpeed * Time.deltaTime; // move towards player
		}

		float target_distance = (m_MyLocation.position - m_TargetPosition).magnitude;

		if (m_PlayerDetected && m_TargetPosition != null && target_distance < 1)
		{

			//m_PlayerDetected = false;
			m_AtTarget = true;
			Pulse();
		}
		else 
		{
			// sniff around for player
			// wander around, aimlessly
		}


	}

	void PlaySound()
	{
		if (m_PlayerDetected)
		{
			//play chase music
		}

		else
		{
			Transform player_transform = GameObject.FindWithTag("Player").transform; //target the player

			float player_distance = (m_MyLocation.position - player_transform.position).magnitude;
			//play with delay						
		}

	}

	void Pulse () 
	{
		m_PlayerDetected = false;

		// if enemy pings player
		// m_PlayerDetected = true;
	}


}
