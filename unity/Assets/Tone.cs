using UnityEngine;
using System.Collections;

public class Tone : MonoBehaviour {
	static void ShiftAudio(AudioSource audio)
	{
		var twelfthRootOfTwo = Mathf.Pow(2f, 1.0f/12);
		var randomShift = Random.Range(1, 12);
		audio.pitch = Mathf.Pow(twelfthRootOfTwo, randomShift);
	}

	public static void SpawnClip (AudioClip sample)
	{
		var go = new GameObject("tone", typeof(AudioSource));
		var audio = go.GetComponent<AudioSource>();

		audio.playOnAwake = false;
		audio.loop = false;
		audio.clip = sample;
		ShiftAudio(audio);
		audio.Play();
	}
}
