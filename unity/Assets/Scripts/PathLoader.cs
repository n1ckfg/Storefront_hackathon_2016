using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PathLoader : MonoBehaviour {

	public string url = "http://fox-gieg.com/stuff/test.csv";
	public GameObject prefab;

	[HideInInspector] public List<TrailFollower> trailFollowers;
	[HideInInspector] public List<Vector3> path;
	[HideInInspector] public List<string> id;

	void Start() {
		StartCoroutine(loadPath()); 
	}


	IEnumerator loadPath() {
		Debug.Log(url);
		WWW www = new WWW(url);
		yield return www;

		string[] pathLines = www.text.Split("\n"[0]);

		for (int i = 0; i < pathLines.Length; i++) {
			string[] pathLine = pathLines[i].Split(","[0]);

			string newId = pathLine[0];

			bool addNewId = true;

			for (int j = 0; j < id.Count; j++) {
				if (newId == id[j]) addNewId = false;
			}

			if (addNewId) {
				id.Add(newId);

				path = new List<Vector3>();

				for (int j = 1; j < pathLine.Length; j += 3) {
					float x = float.Parse(pathLine[j]);
					float y = float.Parse(pathLine[j + 1]);
					float z = float.Parse(pathLine[j + 2]);
					Vector3 v = new Vector3(x, y, z);
					Debug.Log(v);
					path.Add(v); 
				}

				GameObject g = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity); 
				TrailFollower t = g.GetComponent<TrailFollower>();
				t.path = path;
				t.go = true;
				trailFollowers.Add(t);
			}
		}
	}

}
