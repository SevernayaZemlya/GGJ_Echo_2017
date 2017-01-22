using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 2f;            // The speed that the player will move at.
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.


	public AudioSource m_MovementSound;

	public Text countText;
	public Text winText;
	private int count;
	private bool isMoving;
	
	void Awake ()
	{
		
		// Set up references.
		playerRigidbody = GetComponent <Rigidbody> ();
		m_MovementSound.Pause();
		isMoving = false;

		count = 0;
		//m_MovementSound.loop = true;
	}
	
	
	void FixedUpdate ()
	{
		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		
		// Move the player around the scene.
		Move (h, v);

		PlayMovementSound(h, v);


	}

	
	void Move (float h, float v)
	{
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void PlayMovementSound (float h, float v)
	{

		if ((h != 0 || v != 0) && m_MovementSound.isPlaying == false)
		{
			m_MovementSound.Play();
		}

		if ((h == 0 && v == 0) && m_MovementSound.isPlaying == true)
		{
			m_MovementSound.Pause();
		}

	}

	void OnTriggerEnter (Collider other) 
	{
		 if (other.gameObject.tag == "Food") 
		{
			Destroy(other.gameObject);

			count = count + 1;


			//other.gameObject.SetActive (false);
			//play nom sound
			//
			//SetCountText ();
		}
	}

	void SetCountText ()
	{

		countText.text = "Count: " + count.ToString ();
		if (count >= 21) 
		{
			winText.text = "You WIN!";
		}

	}

}
