using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class options : MonoBehaviour {
	public AudioMixer audioMixer;
	public void setv (float volume)
	{
		audioMixer.SetFloat ("volume", volume);
	}
}
