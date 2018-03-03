using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static IList<T> AsList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as IList<T> ?? enumerable.ToList();
        }

        public static T[] AsArray<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as T[] ?? enumerable.ToArray();
        }

        public static IEnumerable<IList<T>> Paginate<T>(this IEnumerable<T> enumerable, int pageSize)
        {
            var buffer = new T[pageSize];
            var count = 0;
            
            foreach (var item in enumerable)
            {
                buffer[count++] = item;
                if (count < pageSize) 
                    continue;
                
                count = 0;
                yield return buffer.ToArray();
            }
            
            if (count > 0)
                yield return buffer.Take(count).ToArray();
        }

        public static IEnumerable<T> AsParallelIfNotDebug<T>(this IEnumerable<T> enumerable)
        {
            return Debugger.IsAttached 
                ? enumerable 
                : enumerable.AsParallel();
        }
    }
}