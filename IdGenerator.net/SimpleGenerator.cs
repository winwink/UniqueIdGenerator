using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniqueIdGenerator.Net
{
    /// <summary>
    /// Simple Generaotor only for Sqlserver BigInt type Primary Key to replace Guid Primary Key
    /// Thread safe
    /// Only for one machine
    /// </summary>
    public class SimpleGenerator
    {
        private static object _lockObject = new object();

        private static int num = 0;

        /// <summary>
        /// Can generate up to many(>10000) different IDs per millisecond
        /// </summary>
        /// <returns></returns>
        public static long NextLong()
        {
            var result = DateTime.Now.Ticks / 10000 * 10000 ;
            lock(_lockObject)
            {
                num++;
                result += num;
            }
            return result;
        }
    }
}
