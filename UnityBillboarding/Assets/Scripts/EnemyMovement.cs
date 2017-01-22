using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {


	//Transform m_Target;
	Vector3 m_TargetPosition;
	Transform m_MyLocation;

	public float m_MoveSpeed = 3.0f;
	public float m_RotationSpeed = 3.0f;
	public float m_PulseTimer = 5.0f;
	private float m_PulseTimerCurrent;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Rigidbody rigidBody;
	private double prev_x;
	// pulse inputs
	public Color m_PulseColor = new Color(1.0f, 0.7f, 0.7f, 1.0f);
	public float m_PulseRange = 15f;
	public float m_PulseIntensityMax = 2.1f; 
	public float m_PulseSpeed = 0.066f;


	GameObject pulse;
	bool pulseIncreasing;

	public AudioSource m_ChaseMusic;
	public AudioSource m_EchoMonster;


	bool m_PlayerDetected = true;
	bool m_AtTarget = true;

	void Awake(){

		m_MyLocation = transform; //cache transform data for easy access/preformance
		m_PulseTimerCurrent = m_PulseTimer;

		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody>();
    }


	// Use this for initialization
	void Start () {
		prev_x = transform.position.x;
		//m_Target = GameObject.FindWithTag("Player").transform; //target the player

	}
	
	// Update is called once per frame
	void Update () {
		
		Move();
		PlayAnimation();
		PlayChaseSound();

		if (m_PlayerDetected == false)
		{
			m_PulseTimerCurrent -= Time.deltaTime;
		}

		if (m_PulseTimerCurrent <= 0.0f)
		{
			Vector3 pos = GameObject.FindWithTag("Monster").transform.position;
			Pulse(pos);
			m_PulseTimerCurrent = m_PulseTimer;
		}

		if (pulse != null) {
			handlePulse();
		}	


	}

	void PlayAnimation () 
	{
		animator.speed = 0.1f;
		/*
 		if (rigidBody.velocity.x > 1) {
 			spriteRenderer.flipX = false;
 		} else if (rigidBody.velocity.x < -1) {
 			spriteRenderer.flipX = true;
 		}*/

 		if (prev_x < transform.position.x) {
 			spriteRenderer.flipX = false;
 		} else if (prev_x > transform.position.x) {
 			spriteRenderer.flipX = true;
 		}

 		prev_x = transform.position.x;
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
			Vector3 pos = GameObject.FindWithTag("Monster").transform.position;
			m_PlayerDetected = false;
			m_AtTarget = true;
			Pulse(pos);
		}
		else 
		{
			// sniff around for player
			// wander around, aimlessly
		}


	}

	void PlayChaseSound()
	{
		if (m_PlayerDetected && !m_ChaseMusic.isPlaying)
		{
			m_ChaseMusic.Play();
		}

		if(!m_PlayerDetected)
		{
			m_ChaseMusic.Pause();
		}

	}

	void Pulse(Vector3 lightLoc) 
	{
		m_PlayerDetected = false;

		GameObject mPulse = new GameObject("mPulse");
		mPulse.transform.position = lightLoc;
		Light lightComp = mPulse.AddComponent<Light>();
		lightComp.color = m_PulseColor; 
		lightComp.range = m_PulseRange;
		lightComp.intensity = 0.001f;
		lightComp.shadows = LightShadows.Soft;
		pulse = mPulse;
		pulseIncreasing = true;
		m_EchoMonster.Play();

		// Player detection
		Collider[] hitColliders = Physics.OverlapSphere(lightLoc, m_PulseRange);
		Debug.Log("Generating hit sphere");
		foreach (Collider collider in hitColliders) {
			if (collider.gameObject.tag == "Player") {
				Debug.Log("Player detected");
				m_PlayerDetected = true;
			}
		}
	}

	void handlePulse() {
		Light li = pulse.GetComponent<Light>();
		if (pulseIncreasing) {
			if (li.intensity >= m_PulseIntensityMax) {
					pulseIncreasing = false;
				} else {
					li.intensity += (m_PulseSpeed);
				}
		} else {
			if (li.intensity <= m_PulseSpeed) {
					Destroy(pulse);
				} else {
					li.intensity -= m_PulseSpeed;
				}
		}
	}




}
