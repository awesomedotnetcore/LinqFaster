﻿using System;
using System.Threading.Tasks;
using System.Numerics;

namespace JM.LinqFaster.Utils
{
    public static class ParallelUtils
    {

        public static Vector<T> AddVectors<T>(Vector<T> a, Vector<T> b)
          where T : struct
        {
            return a + b;
        }


        public static T ApplyTaskAggregate<T>(int from, int to, int stride, T acc, Func<int, T, T> f)
        {
            for (int i = from; i < to; i += stride)
            {
                acc = f(i, acc);
            }
            return acc;
        }

        public static T ForStrideAggregate<T>(int from, int to, int stride, T acc, Func<int, T, T> f, Func<T, T, T> combiner)
        {
            int numStrides = (to - from) / stride;
            if (numStrides <= 0) return acc;

            int numTasks = Math.Min(Environment.ProcessorCount, numStrides);
            int stridesPerTask = numStrides / numTasks;
            int elementsPerTask = stridesPerTask * stride;
            int remainderStrides = numStrides - (stridesPerTask * numTasks);

            var tasks = new Task<T>[numTasks];
            int index = 0;
            for (int i = 0; i < tasks.Length; i++)
            {
                int toExc;
                if (remainderStrides == 0)
                {
                    toExc = index + elementsPerTask;
                }
                else
                {
                    remainderStrides--;
                    toExc = index + elementsPerTask + stride;
                }
                int fromClosure = index;
                tasks[i] = Task<T>.Factory.StartNew(() => ApplyTaskAggregate(fromClosure, toExc, stride, acc, f));
                index = toExc;
            }
            var result = acc;
            for (int i = 0; i < tasks.Length; i++)
            {
                result = combiner(result, tasks[i].Result);
            }
            return result;

        }
    }
}
