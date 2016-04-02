using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailFollower : MonoBehaviour {

	public float ease = 10f;
	public float bounds = 0.1f;
	public bool startAtPos = false;
	public float rot = 1;
	public Vector3 rotAxis = Vector3.up;
	public float targetOffset = 1;
	public float lifeSpan = 10f;

	[HideInInspector] public string id;
	[HideInInspector] public List<Vector3> path;
	[HideInInspector] public int counter = 0;
	[HideInInspector] public bool go = false;
	[HideInInspector] public Transform target;
	[HideInInspector] public float markTime = 0f;

	private Vector3 easeVec;
	private Vector3 boundsVec;
	private GameObject player;
	private bool firstRun = true;
	private LineRenderer lineRen;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");

		transform.position = player.transform.position;
		transform.SetParent(player.transform);

		target = transform.FindChild("TrailObject");
		target.localPosition = Random.onUnitSphere * targetOffset;

		lineRen = target.GetComponent<LineRenderer>();

		easeVec = new Vector3(ease, ease, ease);
		boundsVec = new Vector3(bounds, bounds, bounds);

		StartCoroutine(lifeCycle());
	}

	void Update() {
		if (go) {
			if (firstRun) {
				markTime = Time.realtimeSinceStartup;

				lineRen.SetVertexCount(path.Count);
				for (int i=0; i<path.Count; i++) {
					Vector3 v = path[i];
					v.y *= -1f;
					lineRen.SetPosition(i, v);
				}
				firstRun = false;
			}

			/*
			if (path[counter] != null) {
				target.transform.position = tween(target.transform.position, path[counter], easeVec);

				if (hitDetect(target.transform.position, boundsVec, path[counter], boundsVec)) {
					counter++;
					if (counter >= path.Count) counter = 0;
				}
			}
			*/

			target.transform.RotateAround(transform.position, rotAxis, rot * Time.deltaTime);
			// transform.Rotate(rot);
		}
	}

	IEnumerator lifeCycle() {
		yield return new WaitForSeconds(lifeSpan);
		var lineRendererMaterial = target.gameObject.GetComponent<LineRenderer>().material;
		var a = lineRendererMaterial.color.a;
		while(lineRendererMaterial.color.a > 0.01f) {
			a -= Time.deltaTime;
			lineRendererMaterial.color = new Color(1, 1, 1, a);
			yield return null;
		}
		Destroy(gameObject);
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
