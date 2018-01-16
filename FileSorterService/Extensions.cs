using System;

namespace FileSorterService.Extensions
{
    public static class Extensions
    {
        public static int IndexOf(this string[] arr, string input)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == input)
                    return i;
            }
            return -1;
        }
    }
}
