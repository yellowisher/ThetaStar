using System;

/*
 * Specific data structure for A* pathfinding
 * Basically binary heap but has Contains() method with O(1)
 * Could have used another HashSet for Contains()
 */
public class NodeHeap {
	private Node[] items;
	private int tail = 0;
	private int capacity;

	public int Count {
		get {
			return tail;
		}
	}

	public NodeHeap(int capacity = 4) {
		this.capacity = capacity;
		items = new Node[capacity];
	}

	private void Grow() {
		int newCapacity = capacity * 2;
		Node[] newItems = new Node[newCapacity];
		Array.Copy(items, newItems, capacity);
		items = newItems;
		capacity = newCapacity;
	}

	public void Add(Node item) {
		if (Count == capacity) Grow();

		item.lastHeapIndex = tail;
		items[tail++] = item;
		SortUp(item);
	}

	public Node Pop() {
		if (Count == 0) throw new InvalidOperationException("NodeHeap is empty");

		Node firstItem = items[0];
		items[0] = items[--tail];
		items[0].lastHeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public void UpdateItem(Node item) {
		SortUp(item);
	}

	public bool Contains(Node item) {
		if (item.lastHeapIndex >= tail) return false;
		return items[item.lastHeapIndex] == item;
	}

	void SortDown(Node item) {
		while (true) {
			int childIndexLeft = item.lastHeapIndex * 2 + 1;
			int childIndexRight = item.lastHeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < tail) {
				swapIndex = childIndexLeft;

				if (childIndexRight < tail) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap(item, items[swapIndex]);
				}
				else {
					return;
				}

			}
			else {
				return;
			}

		}
	}

	private void SortUp(Node item) {
		int parentIndex = (item.lastHeapIndex - 1) / 2;

		while (true) {
			Node parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				Swap(item, parentItem);
			}
			else {
				break;
			}

			parentIndex = (item.lastHeapIndex - 1) / 2;
		}
	}

	private void Swap(Node a, Node b) {
		items[a.lastHeapIndex] = b;
		items[b.lastHeapIndex] = a;
		int itemAIndex = a.lastHeapIndex;
		a.lastHeapIndex = b.lastHeapIndex;
		b.lastHeapIndex = itemAIndex;
	}
}