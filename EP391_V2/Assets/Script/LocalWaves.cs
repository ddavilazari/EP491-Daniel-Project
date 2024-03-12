    using UnityEngine;
using System.Collections;

public class LocalWaves : MonoBehaviour {

	public AudioClip[] sounds;
	public int currentClip;

	// Use this for initialization
	void Start () {
		sounds = Resources.LoadAll<AudioClip> ("");
		playNext ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.GetComponent<AudioSource> ().isPlaying) {
			playNext ();
		}
	}

	void playNext () {
		currentClip = (int)Random.Range (0F, sounds.Length);

		this.GetComponent<AudioSource> ().clip = sounds [currentClip];

		float pitch = Random.Range(1F, 1.5F);
		this.GetComponent<AudioSource> ().volume = 1;
		this.GetComponent<AudioSource> ().pitch = pitch;
		this.GetComponent<AudioSource> ().Play ();
		this.GetComponent<AudioSource> ().loop = false;
		print ("NEW CLIP: I'm playing"+ currentClip);
	}
}
