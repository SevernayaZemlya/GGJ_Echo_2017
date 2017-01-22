using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEcho : MonoBehaviour {

	public Color LIGHT_COLOR = new Color(0.8f, 0.8f, 0.91f, 1.0f);
	public const float LIGHT_RANGE = 20;
	public const float PULSE_SPEED = 0.022f;
	public const int PULSE_LIMIT = 3;

	// Collection of echo-location pulse lights
	List<Light> lights = new List<Light>();
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
			Light lgt = lights[i];
			int val = vals[i];

			if (val == 1) { 
			// intensity increasing
				if (lgt.intensity >= 1.0f) {
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
			if (lights[lights.Count - 1].intensity < 0.25f && vals[lights.Count - 1] == 1) {
				return false;
			}
		}
		return true;
	}

	void cleanUp(int toDel) {
		// Removes light from lights and destroys light object
		Light light = lights[toDel];
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
		lightComp.intensity = 0.01f;
		lightComp.shadows = LightShadows.Soft;
		lights.Add(lightComp);
		vals.Add(1); 
	}


}