using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour {
	private LayerMask tileLayer;

	[SerializeField]
	private Agent agent;

	[SerializeField]
	PathfindingManager pathfindingManager;

	private void Awake() {
		tileLayer = LayerMask.GetMask("Tile");
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit result;

			if (Physics.Raycast(ray, out result, Mathf.Infinity, tileLayer)) {
				if (Input.GetMouseButtonDown(0)) {
					Tile tile = result.collider.GetComponent<Tile>();
					if (tile != null) tile.ToggleObstacle();
				}
				if (Input.GetMouseButtonDown(1)) {
					Vector3 destPosition = result.collider.transform.position;

					Node deptNode = pathfindingManager.PositionToNode(agent.transform.position);
					Node destNode = pathfindingManager.PositionToNode(destPosition);

					agent.FollowPath(pathfindingManager.FindPath(deptNode, destNode));
				}
			}
		}
	}
}
