using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEcho : MonoBehaviour {

	public Color LIGHT_COLOR = new Color(0.8f, 0.8f, 0.91f, 1.0f);
	public float LIGHT_RANGE = 30;
	public float LIGHT_INTENSITY_MAX = 2.0f; 
	public float PULSE_SPEED = 0.066f;
	public int PULSE_LIMIT = 3;

	public AudioSource Echo_Location_01;
	public AudioSource Echo_Location_02;
	public AudioSource Echo_Location_03;
	private int lastPlayed = 0;

	// Collection of echo-location pulse lights
	List<GameObject> lights = new List<GameObject>();
	List<int> vals = new List<int>();
	// index of a val in vals matches a light in lights 
	// tracks increasing (1) or decreasing (-1) intensity
	
	void FixedUpdate() {
		// On key press (space)
		if ( Input.GetKeyDown( KeyCode.Space) && lightSpawnAllowed() ) {
			Vector3 pos = GameObject.FindWithTag("Player").transform.position;
			pos.y += 1;
			echo(pos);
		}
		int toDel = -1;

		// pulse lights (foreach light)
		for (int i=0; i<lights.Count; i++) {
			Light lgt = lights[i].GetComponent<Light>();
			int val = vals[i];

			if (val == 1) { 
			// intensity increasing
				if (lgt.intensity >= LIGHT_INTENSITY_MAX) {
					vals[i] = -1;
				} else {
					lgt.intensity += (PULSE_SPEED * val);
				}
			} else { 
			// intensity decreasing
				if (lgt.intensity <= PULSE_SPEED) {
					toDel = i;
				} else {
					lgt.intensity += (PULSE_SPEED * val);
				}
			}	
		}

		// clear lights that have dimmed to intensity 0
		if (toDel >= 0) {
			cleanUp(toDel);
		}
	}

	bool lightSpawnAllowed() {
		// Conditions for allowing a light to spawn
		if (lights.Count > PULSE_LIMIT) return false;
		if (lights.Count > 0) {
			// most recently spawned light was created too recently
			float intensityThreshhold = LIGHT_INTENSITY_MAX * 0.25f;
			if (lights[lights.Count - 1].GetComponent<Light>().intensity < intensityThreshhold 
				&& vals[lights.Count - 1] == 1) {
				return false;
			}
		}
		return true;
	}

	void cleanUp(int toDel) {
		// Removes light from lights and destroys light object
		GameObject light = lights[toDel];
		vals.RemoveAt(toDel);
		lights.RemoveAt(toDel);
		Destroy(light);
	}

	void echo(Vector3 lightLoc) { 
		// Create a new light at given location
		GameObject echoLight = new GameObject("echoSource");
		echoLight.transform.position = lightLoc;
		Light lightComp = echoLight.AddComponent<Light>();
		lightComp.color = LIGHT_COLOR; 
		lightComp.range = LIGHT_RANGE;
		lightComp.intensity = 0.001f;
		lightComp.shadows = LightShadows.Soft;
		lights.Add(echoLight);
		playPulseSound();
		vals.Add(1); 
	}

	void playPulseSound() {
		if (lastPlayed == 2) {
			Echo_Location_01.Play();
			lastPlayed = 0;
			return;
		}
		if (lastPlayed == 0) {
			Echo_Location_02.Play();
			lastPlayed = 1;
			return;
		}
		if (lastPlayed == 1) {
			Echo_Location_03.Play();
			lastPlayed = 2;
			return;
		}
	}


}