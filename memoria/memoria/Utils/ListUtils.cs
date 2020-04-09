using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace memoria.Utils
{
    class ListUtils<T>
    {
        public static List<T> shuffle(List<T> list)
        {
            Random randomizer = new Random(); 

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = randomizer.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static List<T> getRandomElements(List<T> list, int num)
        {
            list.RemoveRange(num, list.Count - num);
            List<T> randomized = shuffle(list);
            
            return randomized;
        }
    }
}
