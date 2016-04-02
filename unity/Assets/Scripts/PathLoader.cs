using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PathLoader : MonoBehaviour {

	public string fileName = "test.csv";
	public float ease = 10f;
	public float bounds = 0.1f;
	public bool startAtPos = false;

	[HideInInspector] public List<Vector3> path;
	[HideInInspector] public int counter = 0;

	//private string folderName = "StreamingAssets";
	private bool ready = false;
	private Vector3 easeVec;
	private Vector3 boundsVec;

	void Start() {
		easeVec = new Vector3(ease, ease, ease);
		boundsVec = new Vector3(bounds, bounds, bounds);

		StartCoroutine(loadPath()); 
	}
	
	void Update() {
		if (ready) {
			transform.position = tween(transform.position, path[counter], easeVec);
			if (hitDetect(transform.position, boundsVec, path[counter], boundsVec)) {
				counter++;
				if (counter >= path.Count) counter = 0;
			}
		}
	}

	IEnumerator loadPath() {
		string url = "";

		/*
		#if UNITY_ANDROID
			url = "file://" + Path.Combine(Path.Combine(Application.persistentDataPath, folderName), fileName); 
		#endif

		#if UNITY_EDITOR
			url = "file://" + Path.Combine(Path.Combine(Application.dataPath, folderName), fileName); 
		#endif
		*/

		url = "http://fox-gieg.com/stuff/test.csv";

		Debug.Log(url);
		WWW www = new WWW(url);
		yield return www;

		string[] pathStrings = www.text.Split(","[0]);
		for (int i=0; i<pathStrings.Length; i += 3) {
			float x = float.Parse(pathStrings[i]);
			float y = float.Parse(pathStrings[i + 1]);
			float z = float.Parse(pathStrings[i + 2]);
			Vector3 v = new Vector3(x, y, z);
			Debug.Log(v);
			path.Add(v); 
		}

		if (startAtPos) transform.position = path[counter]; 

		ready = true;
	}

	Vector3 tween(Vector3 v1, Vector3 v2, Vector3 e) {
		v1.x += (v2.x-v1.x)/e.x;
		v1.y += (v2.y-v1.y)/e.y;
		v1.z += (v2.z-v1.z)/e.z;
		return v1;
	}

	bool hitDetect(Vector3 p1, Vector3 s1, Vector3 p2, Vector3 s2) {
		s1.x /= 2f;
		s1.y /= 2f;
		s1.z /= 2f;
		s2.x /= 2f;
		s2.y /= 2f; 
		s2.z /= 2f; 
		if (  p1.x + s1.x >= p2.x - s2.x && 
			p1.x - s1.x <= p2.x + s2.x && 
			p1.y + s1.y >= p2.y - s2.y && 
			p1.y - s1.y <= p2.y + s2.y &&
			p1.z + s1.z >= p2.z - s2.z && 
			p1.z - s1.z <= p2.z + s2.z
		) {
			return true;
		} else {
			return false;
		}
	}

}
