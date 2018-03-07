using System;
using UnityEngine;

public struct Point {
	public int x;
	public int y;
	public static Node[,] nodes;

	public Point(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public bool IsLineOfSight(Point end) {
		int dfx = end.x - x;
		int dfy = end.y - y;

		bool inSight = true;
		if (Mathf.Abs(dfx) > Mathf.Abs(dfy)) {
			int dx = MyMathf.Sign(dfx);
			float prevY = y;

			for (int sx = x + dx; sx != end.x + dx; sx += dx) {
				float sy = y + (dfy * (sx - x)) / (float)dfx;

				int nx = dx > 0 ? sx - dx : sx;
				int ny = Mathf.Min((int)sy, (int)prevY);
				if (nodes[nx, ny].isBlocked) {
					inSight = false;
					break;
				}

				// Line is contained in two nodes; check another node(upper bound)
				if ((int)prevY != (int)sy && !MyMathf.IsInt(prevY) && !MyMathf.IsInt(sy)) {
					ny = Mathf.Max((int)sy, (int)prevY);
					if (nodes[nx, ny].isBlocked) {
						inSight = false;
						break;
					}
				}
				prevY = sy;
			}
		}
		else {
			int dy = MyMathf.Sign(dfy);
			float prevX = x;

			for (int sy = y + dy; sy != end.y + dy; sy += dy) {
				float sx = x + (dfx * (sy - y)) / (float)dfy;

				int ny = dy > 0 ? sy - dy : sy;
				int nx = Mathf.Min((int)sx, (int)prevX);
				if (nodes[nx, ny].isBlocked) {
					inSight = false;
					break;
				}

				if ((int)prevX != (int)sx && !MyMathf.IsInt(prevX) && !MyMathf.IsInt(sx)) {
					nx = Mathf.Max((int)sx, (int)prevX);
					if (nodes[nx, ny].isBlocked) {
						inSight = false;
						break;
					}
				}
				prevX = sx;
			}
		}
		return inSight;
	}
}

/*
 * Something like Point[4] array. 
 * Point array with size 4 is used a lot, so I made
 * value-type Point array. (for less GC call)
 */
public struct Point4 {
	public Point point0;
	public Point point1;
	public Point point2;
	public Point point3;

	public Point this[int idx] {
		get {
			switch (idx) {
				case 0: return point0;
				case 1: return point1;
				case 2: return point2;
				case 3: return point3;
			}
			throw new InvalidOperationException("Point index out of range" + idx);
		}
		set {
			switch (idx) {
				case 0:
					point0 = value;
					break;
				case 1:
					point1 = value;
					break;
				case 2:
					point2 = value;
					break;
				case 3:
					point3 = value;
					break;
				default:
					throw new InvalidOperationException("Point index out of range" + idx);
			}
		}
	}
}
