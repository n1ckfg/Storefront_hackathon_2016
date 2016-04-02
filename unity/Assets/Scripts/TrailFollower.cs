using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailFollower : MonoBehaviour {

	public float ease = 10f;
	public float bounds = 0.1f;
	public bool startAtPos = false;
	public Vector3 rot = Vector3.zero;

	[HideInInspector] public string id;
	[HideInInspector] public List<Vector3> path;
	[HideInInspector] public int counter = 0;
	[HideInInspector] public bool go = false;
	[HideInInspector] public Transform target;

	private Vector3 easeVec;
	private Vector3 boundsVec;
	private GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		transform.position = player.transform.position;
		transform.SetParent(player.transform);

		target = transform.FindChild("TrailObject");

		easeVec = new Vector3(ease, ease, ease);
		boundsVec = new Vector3(bounds, bounds, bounds);
	}

	void Update() {
		if (go) {
			if (path[counter] != null) {
				target.transform.position = tween(target.transform.position, player.transform.position + path[counter], easeVec);

				if (hitDetect(target.transform.position, boundsVec, path[counter], boundsVec)) {
					counter++;
					if (counter >= path.Count) counter = 0;
				}
			}

			transform.Rotate(rot);
		}
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
		if (p1.x + s1.x >= p2.x - s2.x && 
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
