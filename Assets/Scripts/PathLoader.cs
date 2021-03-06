﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PathLoader : MonoBehaviour {

	public string url = "http://fox-gieg.com/stuff/test.csv";
	public GameObject prefab;
	public bool is3d = false;
	public float checkTime = 1f;
	public AudioClip spawnClip;

	//[HideInInspector] public List<TrailFollower> trailFollowers;
	[HideInInspector] public List<Vector3> path;

	private HashSet<string> seenIds;

	void Start() {
		seenIds = new HashSet<string>();
		StartCoroutine(loadPaths());
	}

	IEnumerator loadPaths() {
		while(true) {
			Debug.Log("Loading paths...");
			yield return StartCoroutine(loadPath());
			yield return new WaitForSeconds(checkTime);
		}
	}

	IEnumerator loadPath() {
		Debug.Log(url);
		WWW www = new WWW(url);
		yield return www;

		string[] pathLines = www.text.Split("\n"[0]);

		for (int i = 0; i < pathLines.Length; i++) {
//			 try {
				string[] pathLine = pathLines[i].Split(","[0]);

				string newId = pathLine[0];

				if (!seenIds.Contains(newId)) {
					seenIds.Add(newId);

					path = new List<Vector3>();

					if (is3d) {
						for (int j = 1; j < pathLine.Length; j += 3) {
							float x = float.Parse(pathLine[j]);
							float y = float.Parse(pathLine[j + 1]);
							float z = float.Parse(pathLine[j + 2]);

							addPathPoint(x, y, z);
						}
					} else {
						for (int j = 1; j < pathLine.Length; j += 2) {
							if(pathLine[j] != "") {
								float x = float.Parse(pathLine[j]);
								float y = float.Parse(pathLine[j + 1]);
								float z = 0f;

								addPathPoint(x, y, z); 
							}
						}					
					}

					GameObject g = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity); 
					Tone.SpawnClip(spawnClip);
					Debug.Log("Spawning");
					TrailFollower t = g.GetComponent<TrailFollower>();
					t.path = path;
					t.go = true;
					//trailFollowers.Add(t);
				}
//			} catch(System.Exception e) {
//				Debug.LogError(e.Message);
//				continue;
//			}
		}

		/*
		for (int i=0; i<trailFollowers.Count; i++) {
			if (!trailFollowers[i].alive) {
				Destroy(trailFollowers[i].gameObject);
				trailFollowers.RemoveAt(i);
			}
		}
		*/
	}

	void addPathPoint(float x, float y, float z) {
		if (x != null && y != null && z!= null) {
			Vector3 v = new Vector3(x, y, z);
//			Debug.Log(v);
			path.Add(v); 
		}	
	}

}
