using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniqueIdGenerator.Net;
using System.Collections.Generic;
using System.Diagnostics;

namespace UniqueIdGenerator.Net.MsTests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void When_generating_hundred_thousand_ids_with_one_single_generator_then_every_id_is_unique()
        {
            const int number = 100000;

            // warm-up
            GetIds(0, number);

            var stopwatch = Stopwatch.StartNew();
            var ids = GetIds(0, number);
            stopwatch.Stop();
            Console.WriteLine("Duration: {0}ms", stopwatch.ElapsedMilliseconds);

            var unique = new HashSet<string>(StringComparer.Ordinal);
            string current = "AAAAAAAAAAA=";
            foreach (var id in ids)
            {
                Assert.IsTrue(IdConverter.ToLong(id).CompareTo(IdConverter.ToLong(current)) == 1);
                //Assert.IsTrue(id.CompareTo(current) == 1);
                Assert.IsFalse(unique.Contains(id), id);


                unique.Add(id);
                current = id;
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(ids[i]);
            }
        }

        [TestMethod]
        public void When_generating_hundred_thousand_ids_with_one_single_generator_then_every_id_is_ordered()
        {
            const int number = 100000;

            // warm-up
            GetIds(0, number);

            var stopwatch = Stopwatch.StartNew();
            var ids = GetLongIds(0, number);
            stopwatch.Stop();
            Console.WriteLine("Duration: {0}ms", stopwatch.ElapsedMilliseconds);

            var unique = new HashSet<ulong>();
            ulong current = 0;
            foreach (var id in ids)
            {
                Assert.IsTrue(id.CompareTo(current) == 1);
                Assert.IsFalse(unique.Contains(id));
                
                unique.Add(id);
                current = id;
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(ids[i]);
            }
        }
        
        private IList<string> GetIds(short machineId, int number)
        {
            var generator = new Generator(machineId, DateTime.Today);
            var ids = new List<string>(number);

            for (int i = 0; i < number; i++)
            {
                ids.Add(generator.Next());
            }

            return ids;
        }

        private IList<ulong> GetLongIds(short machineId, int number)
        {
            var generator = new Generator(machineId, DateTime.Today);
            var ids = new List<ulong>(number);

            for (int i = 0; i < number; i++)
            {
                ids.Add(generator.NextLong());
            }

            return ids;
        }

    }
}
