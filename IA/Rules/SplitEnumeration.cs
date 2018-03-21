using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    static class SplitEnumeration
    {

        public static List<int[]> GetEnumeration(int q, int minSplit, int maxSplitGroups)
        {
            return _removeTooManySplitGroups(_removeDoubles(_getEnumerationRecursive(new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, q, minSplit)), maxSplitGroups);
        }

        private static List<int[]> _getEnumerationRecursive(int[] previous, int q, int minSplit)
        {
            List<int[]> returnList = new List<int[]>();
            if (q > 0)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] L = (int[])previous.Clone();
                    L[j] = q;
                    returnList.Add(L);
                }
            }
            if (q < 2 * minSplit)
            {
                returnList.Add(previous);
                return returnList;
            }

            for (int j = 0; j < 8; j++)
            {
                for (int n = minSplit; n <= q - minSplit; n++)
                {
                    int[] L = (int[])previous.Clone();
                    L[j] += n;
                    returnList.AddRange(_getEnumerationRecursive(L, q - n, minSplit));
                }
            }
            return returnList;
        }

        private static List<int[]> _removeTooManySplitGroups(List<int[]> list, int maxSplitGroups)
        {
            List<int[]> cloneList = new List<int[]>();
            if (list.Count == 0)
                return list;
           foreach(int[] array in list)
            {
                int splitGroup = 0;
                for (int j = 0; j < 8; j++)
                    if (array[j] > 0)
                        splitGroup++;
                if (splitGroup <= maxSplitGroups)
                    cloneList.Add(array);
            }
            return cloneList;
        }

        private static List<int[]> _removeDoubles(List<int[]> list)
        {
            List<int[]> cloneList = new List<int[]>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!_containsArray(cloneList, list[i]))
                    cloneList.Add(list[i]);
            }
            return cloneList;
        }

        private static bool _containsArray(List<int[]> list, int[] testArray)
        {
            if (list.Count == 0)
                return false;
            foreach (int[] array in list)
            {
                if (Enumerable.SequenceEqual(testArray, array))
                    return true;
            }
            return false;
        }
    }
}
