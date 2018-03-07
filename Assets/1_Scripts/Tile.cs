using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool isBlocked;

	private GameObject obstacle;

	private void OnValidate() {
		ToggleObstacle(true);
	}

	public void ToggleObstacle(bool updateOnly = false) {
		if (!updateOnly) isBlocked = !isBlocked;
		transform.Find("Obstacle").gameObject.SetActive(isBlocked);
	}
}
