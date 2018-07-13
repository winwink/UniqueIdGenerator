using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniqueIdGenerator.Net.MsTests
{
    [TestClass]
    public class SimpleGeneratorTests
    {
        [TestMethod]
        public void SimpleGeneratorOrderedAndUniqueTest()
        {
            GetIds(100000);

            var stopwatch = Stopwatch.StartNew();
            var ids = GetIds(100000);
            stopwatch.Stop();
            Console.WriteLine("all done, times:" + stopwatch.ElapsedMilliseconds);

            var unique = new HashSet<long>();
            long current = 0;
            foreach (var id in ids)
            {
                if (!(id.CompareTo(current) == 1))
                    throw new Exception("not ordered");
                if (unique.Contains(id))
                    throw new Exception("not unique");

                unique.Add(id);
                current = id;
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(ids[i]);
            }
        }

        [TestMethod]
        public void SimpleGeneratorMultiThreadTest()
        {
            var count = 100;
            Thread[] threads = new Thread[count];
            List<GeneTester> list = new List<GeneTester>(count);

            for (int i = 0; i < count; i++)
            {
                GeneTester test = new GeneTester();
                list.Add(test);
                threads[i] = new Thread(() => test.DoGenerator());
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            var unique = new HashSet<long>();
            for (int i = 0; i < count; i++)
            {
                if (unique.Contains(list[i].UId))
                    throw new Exception("not unique");
                unique.Add(list[i].UId);
            }
        }

        private IList<long> GetIds(int number)
        {
            var ids = new List<long>(number);

            for (int i = 0; i < number; i++)
            {
                ids.Add(SimpleGenerator.NextLong());
            }

            return ids;
        }
    }

    public class GeneTester
    {
        public GeneTester()
        {
        }

        public long UId { get; set; }

        public void DoGenerator()
        {
            UId = SimpleGenerator.NextLong();
        }
    }


}
