using System;

public class Node : IComparable<Node> {
	public Node(int x, int y, Tile tile) {
		this.x = x;
		this.y = y;
		this.tile = tile;
	}

	// Let PathfindingManager check line of sight?
	public static Node[,] nodes;
	public int x;
	public int y;

	public int toCost;
	public int fromCost;

	public Tile tile;
	public Node from;

	public int lastHeapIndex;

	public int cost {
		get {
			return toCost + fromCost;
		}
	}

	public bool isBlocked {
		get {
			return tile.isBlocked;
		}
	}

	// :O
	public int CompareTo(Node other) {
		if (other == null) return -1;
		int result = cost.CompareTo(other.cost);
		if (result == 0) result = fromCost.CompareTo(other.fromCost);
		return -result;
	}

	public Point4 GetContainingPoints() {
		Point4 points = new Point4();
		points.point0 = new Point(x, y);
		points.point1 = new Point(x + 1, y);
		points.point2 = new Point(x, y + 1);
		points.point3 = new Point(x + 1, y + 1);
		return points;
	}

	public bool IsLineOfSight(Node end) {
		bool inSight;

		if (x == end.x) {
			int dy = MyMathf.Sign(end.y - y);
			if (dy == 0) return !tile.isBlocked;

			inSight = true;
			for (int sy = y; sy != end.y + dy; sy += dy) {
				if (nodes[x, sy].tile.isBlocked) {
					inSight = false;
					break;
				}
			}
			return inSight;
		}
		else if (y == end.y) {
			int dx = MyMathf.Sign(end.x - x);
			//if (dx == 0) return !tile.isBlocked;

			inSight = true;
			for (int sx = x; sx != end.x + dx; sx += dx) {
				if (nodes[sx, y].tile.isBlocked) {
					inSight = false;
					break;
				}
			}
			return inSight;
		}

		Point4 startPoints = GetContainingPoints();
		Point4 endPoints = end.GetContainingPoints();

		inSight = true;
		for (int i = 0; i < 4; i++) {
			if (!startPoints[i].IsLineOfSight(endPoints[i])) {
				inSight = false;
				break;
			}
		}
		return inSight;
	}
}