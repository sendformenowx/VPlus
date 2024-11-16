namespace NewFilter.Clientless;

internal class InitializeArrays
{
	public static string[] InitStringArray(string[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = "";
		}
		return arr;
	}

	public static int[] InitIntArray(int[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = 0;
		}
		return arr;
	}

	public static bool[] InitBoolArray(bool[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = false;
		}
		return arr;
	}
}
