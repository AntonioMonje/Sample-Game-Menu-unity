using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization

	void OnGUI() {
		if (GUI.Button (new Rect (910, 500, 100, 50), "Menu")) 
		{
			Application.LoadLevel ("MainMenu");
		}

		if (GUI.Button (new Rect (910, 400, 100, 50), "Restart")) 
		{
			Application.LoadLevel ("Level1");
		}

	// Update is called once per frame
	
}
}