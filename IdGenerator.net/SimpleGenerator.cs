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

        private static long num = 0;

        private static DateTime lastTime = DateTime.Now;

        /// <summary>
        /// Can generate up to 9999 different IDs per millisecond
        /// </summary>
        /// <returns></returns>
        public static long NextLong()
        {
            var now = DateTime.Now;
            var result = now.Ticks / 10000 * 10000 ;
            lock(_lockObject)
            {
                if (lastTime != now)
                {
                    num = 0;
                    lastTime = now;
                }
                num++;
                //num = num % 10000;
                if (num >= 10000)
                {
                    Thread.Sleep(1);
                    now = DateTime.Now;
                    result = now.Ticks / 10000 * 10000;
                    lastTime = now;
                    num = num % 10000;
                }
                result += num;
            }
            return result;
        }
    }
}
