public static class MyMathf {
	public static int Sign(int num) {
		if (num == 0) return 0;
		return num > 0 ? 1 : -1;
	}

	public static bool IsInt(float num) {
		return num == (int)num;
	}
}