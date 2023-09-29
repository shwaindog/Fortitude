using System.Collections.Generic;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;

namespace FortitudeCommon.DataStructures
{
    public static class ZeroAllocationArrayTools
    {
        public static void Quicksort(decimal[] elements, int left, int right)
        {
            if (elements.Length > 1)
            {
                left = left < 0 ? 0 : left >= elements.Length ? elements.Length - 1 : left;
                right = right < 0 ? 0 : right >= elements.Length ? elements.Length - 1 : right;
                QuicksortRec(elements, left, right);
            }
        }

        private static void QuicksortRec(decimal[] elements, int left, int right)
        {
            int i = left, j = right;
            var pivot = elements[(left + right)/2];

            while (i <= j)
            {
                while (i < elements.Length && elements[i] - pivot < 0)
                {
                    i++;
                }

                while (j >= 0 && elements[j] - pivot > 0)
                {
                    j--;
                }

                if (i < j)
                {
                    var tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;
                    i++;
                    j--;
                }
                else if (i == j)
                {
                    i++;
                    j--;
                }
            }
            if (left < j)
            {
                QuicksortRec(elements, left, j);
            }

            if (i < right)
            {
                QuicksortRec(elements, i, right);
            }
        }

        public static int ZeroAllocLastIndexOf(this decimal[] elements, decimal findThis)
        {
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                if (elements[i] == findThis)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void QuicksortCallStatTimeSpan(List<CallStat> elements, int left, int right)
        {
            if (elements.Count > 1)
            {
                left = left < 0 ? 0 : left >= elements.Count ? elements.Count - 1 : left;
                right = right < 0 ? 0 : right >= elements.Count ? elements.Count - 1 : right;
                QuicksortRecTimeSpan(elements, left, right);
            }
        }

        private static void QuicksortRecTimeSpan(List<CallStat> elements, int left, int right)
        {
            int i = left, j = right;
            var pivot = elements[(left + right)/2].ExecutionTime;

            while (i <= j)
            {
                while (i < elements.Count && elements[i].ExecutionTime.Ticks - pivot.Ticks < 0)
                {
                    i++;
                }

                while (j >= 0 && elements[j].ExecutionTime.Ticks - pivot.Ticks > 0)
                {
                    j--;
                }

                if (i < j)
                {
                    var tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;
                    i++;
                    j--;
                }
                else if (i == j)
                {
                    i++;
                    j--;
                }
            }
            if (left < j)
            {
                QuicksortRecTimeSpan(elements, left, j);
            }

            if (i < right)
            {
                QuicksortRecTimeSpan(elements, i, right);
            }
        }
    }
}