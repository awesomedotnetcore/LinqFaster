﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqFaster
{
    public static class SelectFuncs
    {

        // --------------------------  ARRAYS  --------------------------------------------

        public static TResult[] Select<T,TResult>(this T[] a, Func<T,TResult> selector)
        {                        
            var r = new TResult[a.Length];
            for (int i = 0; i < a.Length;i++)
            {
                r[i] = selector.Invoke(a[i]);
            }
            return r;
        }

        public static TResult[] Select<T, TResult>(this T[] a, Func<T,int,TResult> selector)
        {
            var r = new TResult[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = selector.Invoke(a[i],i);
            }
            return r;
        }

        // --------------------------  LISTS --------------------------------------------

        public static List<TResult> Select<T, TResult>(this List<T> a, Func<T, TResult> selector)
        {
            var r = new List<TResult>(a.Count);
            for (int i = 0; i < a.Count; i++)
            {
                r[i] = selector.Invoke(a[i]);
            }
            return r;
        }

        public static List<TResult> Select<T, TResult>(this List<T> a, Func<T, int, TResult> selector)
        {
            var r = new List<TResult>(a.Count);
            for (int i = 0; i < a.Count; i++)
            {
                r[i] = selector.Invoke(a[i], i);
            }
            return r;
        }






    }
}