using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour {
	[SerializeField]
	private Transform tileHolder;
	private Node[,] nodes;

	private int width;
	private int height;
	private Vector3 min;
	private Vector3 max;
	private float gapX;
	private float gapZ;

	/*
	 * Tiles should be shape of
	 *	  /		1\
	 *	 /		  \
	 *	/0		   \
	 */
	private void Awake() {
		List<Tile> tiles = new List<Tile>();
		for (int i = 0; i < tileHolder.childCount; i++) {
			tiles.Add(tileHolder.GetChild(i).GetComponent<Tile>());
		}

		min = tiles[0].transform.position;
		max = tiles[1].transform.position;

		width = (int)(max.x - min.x) + 1;
		height = (int)(max.z - min.z) + 1;

		gapX = (max.x - min.x) / width;
		gapZ = (max.z - min.z) / height;

		nodes = new Node[width, height];
		foreach (var tile in tiles) {
			int x = (int)(tile.transform.position.x - min.x);
			int y = (int)(tile.transform.position.z - min.z);

			nodes[x, y] = new Node(x, y, tile);
		}
		Node.nodes = nodes;
		Point.nodes = nodes;
	}

	private List<Node> GetAdjacents(Node node) {
		List<Node> adjacents = new List<Node>();

		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				if (dx == 0 && dy == 0) continue;

				int x = node.x + dx;
				int y = node.y + dy;

				if (x >= 0 && x < width && y >= 0 && y < height) {
					if (nodes[x, y].isBlocked) continue;

					if (dx == 0 || dy == 0) {
						adjacents.Add(nodes[x, y]);
					}
					else {
						if (!nodes[node.x, y].isBlocked && !nodes[x, node.y].isBlocked)
							adjacents.Add(nodes[x, y]);
					}
				}
			}
		}
		return adjacents;
	}

	public bool CheckPathExist(Node deptNode, Node destNode) {
		return FindPath(deptNode, deptNode).Count > 0;
	}

	public List<Vector3> FindPath(Node deptNode, Node destNode) {
		NodeHeap openHeap = new NodeHeap();
		HashSet<Node> closedSet = new HashSet<Node>();

		openHeap.Add(deptNode);
		deptNode.fromCost = deptNode.toCost = 0;
		deptNode.from = destNode.from = null;

		while (openHeap.Count > 0) {
			Node nodeToVisit = openHeap.Pop();
			closedSet.Add(nodeToVisit);

			if (nodeToVisit == destNode) {
				break;
			}

			foreach (var adjacent in GetAdjacents(nodeToVisit)) {
				if (closedSet.Contains(adjacent)) continue;

				Node fromNode = null;
				if (nodeToVisit.from != null && nodeToVisit.from.IsLineOfSight(adjacent))
					fromNode = nodeToVisit.from;
				else
					fromNode = nodeToVisit;

				int toAdjacentCost = fromNode.toCost + PredictDistanceCost(fromNode, adjacent);
				if (!openHeap.Contains(adjacent) || toAdjacentCost < adjacent.toCost) {
					adjacent.toCost = toAdjacentCost;
					adjacent.fromCost = PredictDistanceCost(adjacent, destNode);
					adjacent.from = fromNode;

					if (openHeap.Contains(adjacent)) openHeap.UpdateItem(adjacent);
					else openHeap.Add(adjacent);
				}
			}
		}

		List<Node> path = new List<Node>();
		if (destNode.from != null) {
			Node currentNode = destNode;

			while (currentNode != null) {
				path.Add(currentNode);
				currentNode = currentNode.from;
			}
			path.Reverse();
		}

		// PS
		// SmoothingPath(path);

		List<Vector3> positionPath = new List<Vector3>();

		// Visit departure node looks wired when path is changed
		// Skip first(departure) node
		for (int i = 1; i < path.Count; i++) {
			positionPath.Add(path[i].tile.transform.position);
		}
		return positionPath;
	}

	private void SmoothingPath(List<Node> path) {
		for (int i = 1; i < path.Count - 1; i++) {
			if (path[i - 1].IsLineOfSight(path[i + 1])) {
				path.RemoveAt(i--);
			}
		}
	}

	private int PredictDistanceCost(Node dept, Node dest) {
		int dx = Mathf.Abs(dept.x - dest.x);
		int dy = Mathf.Abs(dept.y - dest.y);

		return Mathf.Abs(dx - dy) * 10 + (dx > dy ? dy : dx) * 14;
	}

	public Node PositionToNode(Vector3 position) {
		int x = Mathf.Clamp((int)((position.x - min.x) / gapX), 0, width);
		int y = Mathf.Clamp((int)((position.z - min.z) / gapZ), 0, height);
		return nodes[x, y];
	}
}