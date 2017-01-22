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
	
	void Awake ()
	{
		
		// Set up references.
		playerRigidbody = GetComponent <Rigidbody> ();
		m_MovementSound.Pause();

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


		if (playerRigidbody.velocity.magnitude > 0 && m_MovementSound.isPlaying == false)
		{
			m_MovementSound.UnPause();
		}

		if (playerRigidbody.velocity.magnitude <= 0.1 && m_MovementSound.isPlaying == true)
		{
			m_MovementSound.Pause();
		}

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
