using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
	public float moveSpeed;

	public void FollowPath(List<Vector3> path) {
		StopCoroutine("FollowPathCo");
		StartCoroutine("FollowPathCo", path);
	}

	private IEnumerator FollowPathCo(List<Vector3> path) {
		foreach (var pos in path) {
			Vector3 checkPoint = pos;
			checkPoint.y = transform.position.y;

			while (transform.position != checkPoint) {
				transform.position = Vector3.MoveTowards(transform.position, checkPoint, moveSpeed);
				yield return null;
			}
		}
	}
}
