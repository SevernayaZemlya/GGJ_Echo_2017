using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public GUITexture startScreen;

	void Update () {
		if ( Input.GetKeyDown( KeyCode.Space) ) {
			Destroy(startScreen);
		}
	}

}