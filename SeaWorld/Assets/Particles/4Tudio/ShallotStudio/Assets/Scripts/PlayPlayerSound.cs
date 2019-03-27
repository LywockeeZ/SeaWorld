using UnityEngine;
using System.Collections;

public class PlayPlayerSound : MonoBehaviour {

	public float[] myVolume = {1.0f};
	public AudioClip[] myClips;
	public Vector3 camPos = new Vector3 (3.0f, 6.5f, -3.0f);



	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void playAudio01()
	{
		AudioSource.PlayClipAtPoint (myClips[0], camPos, myVolume[0]);
	}

	void playAudio02()
	{
		AudioSource.PlayClipAtPoint (myClips[1], camPos, myVolume[1]);
	}

	void playAudio03()
	{
		AudioSource.PlayClipAtPoint (myClips[2], camPos, myVolume[2]);
	}

	void playAudio04()
	{
		AudioSource.PlayClipAtPoint (myClips[3], camPos, myVolume[3]);
	}

	void playAudio05()
	{
		AudioSource.PlayClipAtPoint (myClips[4], camPos, myVolume[4]);
	}

	void playAudio06()
	{
		AudioSource.PlayClipAtPoint (myClips[5], camPos, myVolume[5]);
	}

	void playAudio07()
	{
		AudioSource.PlayClipAtPoint (myClips[6], camPos, myVolume[6]);
	}

	void playAudio08()
	{
		AudioSource.PlayClipAtPoint (myClips[7], camPos, myVolume[7]);
	}

	void playAudio09()
	{
		AudioSource.PlayClipAtPoint (myClips[8], camPos, myVolume[8]);
	}

	void playAudio10()
	{
		AudioSource.PlayClipAtPoint (myClips[9], camPos, myVolume[9]);
	}

	void playAudio11()
	{
		AudioSource.PlayClipAtPoint (myClips[10], camPos, myVolume[10]);
	}

	void playAudio12()
	{
		AudioSource.PlayClipAtPoint (myClips[11], camPos, myVolume[11]);
	}

	void playAudio13()
	{
		AudioSource.PlayClipAtPoint (myClips[12], camPos, myVolume[12]);
	}

	void playAudio14()
	{
		AudioSource.PlayClipAtPoint (myClips[13], camPos, myVolume[13]);
	}

	void playAudio15()
	{
		AudioSource.PlayClipAtPoint (myClips[14], camPos, myVolume[14]);
	}

	void playAudio16()
	{
		AudioSource.PlayClipAtPoint (myClips[15], camPos, myVolume[15]);
	}

}
