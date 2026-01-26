using System;
using System.Diagnostics;

namespace BasicSearchAlgorithmsApp
{
    public class Program
    {
        static int LinearSearch(int[] array, int target)
        {
            if (array == null || array.Length == 0)
                return -1;

            for (int i = 0; i < array.Length; i++)
                if (array[i] == target)
                    return i;

            return -1;
        }

        static int BinarySearch(int[] array, int target)
        {
            if (array == null || array.Length == 0)
                return -1;

            int left = 0;
            int right = array.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid] == target)
                    return mid;
                else if (array[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }

        static int InterpolationSearch(int[] array, int target)
        {
            if (array == null || array.Length == 0)
                return -1;

            int low = 0;
            int high = array.Length - 1;

            while (low <= high && target >= array[low] && target <= array[high])
            {
                if (array[low] == array[high])
                {
                    if (array[low] == target)
                        return low;
                    return -1;
                }

                int pos = low + (int)((double)(high - low) * (target - array[low]) / (array[high] - array[low]));

                if (pos < low || pos > high)
                    return -1;

                if (array[pos] == target)
                    return pos;

                if (array[pos] < target)
                    low = pos + 1;
                else
                    high = pos - 1;
            }

            return -1;
        }

        static int ExponentialSearch(int[] array, int target)
        {
            if (array == null || array.Length == 0)
                return -1;

            if (array[0] == target)
                return 0;

            int bound = 1;

            while (bound < array.Length && array[bound] < target)
                bound *= 2;

            int left = bound / 2;
            int right = Math.Min(bound, array.Length - 1);

            return BinarySearchRange(array, left, right, target);
        }

        static int BinarySearchRange(int[] array, int left, int right, int target)
        {
            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid] == target)
                    return mid;
                else if (array[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }

        static int HybridSearch(int[] array, int target)
        {
            if (array == null || array.Length == 0)
                return -1;

            if (array.Length < 1000)
                return LinearSearch(array, target);

            bool sorted = IsSorted(array);
            bool uniform = IsUniformlyDistributed(array);

            if (!sorted)
                return LinearSearch(array, target);

            if (uniform)
                return InterpolationSearch(array, target);

            return ExponentialSearch(array, target);
        }

        static bool IsSorted(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
                if (array[i] < array[i - 1])
                    return false;
            return true;
        }

        static bool IsUniformlyDistributed(int[] array)
        {
            if (array.Length < 2)
                return true;

            double avgStep = (array[array.Length - 1] - array[0]) / (double)(array.Length - 1);

            int checkCount = 1000;
            if (array.Length < checkCount)
                checkCount = array.Length;

            int step = array.Length / checkCount;
            if (step == 0)
                step = 1;

            int violations = 0;

            for (int i = 1; i < array.Length; i += step)
            {
                double curStep = array[i] - array[i - 1];
                if (Math.Abs(curStep - avgStep) > avgStep * 0.3)
                    violations++;
            }

            return violations < checkCount * 0.2;
        }

        static int[] GenerateUniformData(int size)
        {
            int[] arr = new int[size];
            int start = 0;
            for (int i = 0; i < size; i++)
                arr[i] = start + i * 2;
            return arr;
        }

        static int[] GenerateClusteredData(int size)
        {
            int[] arr = new int[size];
            Random rnd = new Random();

            int index = 0;
            int value = 0;

            while (index < size)
            {
                int clusterSize = rnd.Next(1, 100);
                for (int i = 0; i < clusterSize && index < size; i++)
                    arr[index++] = value;

                value += rnd.Next(1, 50);
            }

            Array.Sort(arr);
            return arr;
        }

        static int[] GeneratePartiallySortedData(int size)
        {
            int[] arr = GenerateUniformData(size);
            Random rnd = new Random();

            for (int i = 0; i < size / 10; i++)
            {
                int a = rnd.Next(0, size);
                int b = rnd.Next(0, size);
                int temp = arr[a];
                arr[a] = arr[b];
                arr[b] = temp;
            }

            return arr;
        }

        static void Measure(Action action, string name)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            Console.WriteLine($"{name}: {sw.Elapsed.TotalMilliseconds} ms");
        }

        static void Main()
        {
            int[] uniformData = GenerateUniformData(1000000);
            int[] clusteredData = GenerateClusteredData(1000000);
            int[] partialData = GeneratePartiallySortedData(1000000);

            int targetUniform = uniformData[uniformData.Length - 1];
            int targetCluster = clusteredData[clusteredData.Length - 1];
            int targetPartial = partialData[partialData.Length - 1];

            Console.WriteLine("Равномерные данные:");
            Measure(() => LinearSearch(uniformData, targetUniform), "  Линейный");
            Measure(() => BinarySearch(uniformData, targetUniform), "  Бинарный");
            Measure(() => InterpolationSearch(uniformData, targetUniform), "  Интерполяционный");
            Measure(() => HybridSearch(uniformData, targetUniform), "  Гибридный");

            Console.WriteLine("\nКластерные данные:");
            Measure(() => ExponentialSearch(clusteredData, targetCluster), "  Экспоненциальный");
            Measure(() => HybridSearch(clusteredData, targetCluster), "  Гибридный");

            Console.WriteLine("\nЧастично отсортированные данные:");
            Measure(() => HybridSearch(partialData, targetPartial), "  Гибридный");
        }
    }
}
