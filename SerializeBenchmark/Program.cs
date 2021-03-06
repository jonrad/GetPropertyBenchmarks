﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SerializeBenchmark
{
    public class Program
    {
        public static int Count = 100_000_000;

        public class Person
        {
            public string Name { get; set; }

            public object SomeObject { get; set; }

            public int Age { get; set; }
        }

        public static void Main(string[] args)
        {
            var propertyName = nameof(Person.Name);

            var jon = new Person { Name = "Jon", SomeObject = "Jon", Age = 10 };
            var benchmarkPropertyInfo = jon.GetType().GetProperty(propertyName);
            var valueTypePropertyInfo = jon.GetType().GetProperty(nameof(Person.Age));

            if (benchmarkPropertyInfo == null)
            {
                throw new Exception("I'm sorry Dave, I'm afraid I can't do that");
            }

            var candidates = GetCandidates().ToArray();

            Console.WriteLine("Calculating, bear with me");

            // I wanted to play with C# tuples
            // they're not as good as F#s :(
            var results = candidates
                .Select(c => (candidate: c, func: c.CreateDelegate(jon, benchmarkPropertyInfo)))
                .Select(c => (candidate: c.candidate, func: c.func, benchmark: Benchmark(c.Item2, jon, c.Item1.Divisor)))
                .OrderBy(c => c.benchmark);

            foreach (var (candidate, func, benchmarkMs) in results)
            {
                // 'Testing' to make sure value types work
                var valueTypeFunc = candidate.CreateDelegate(jon, valueTypePropertyInfo);
                Console.WriteLine($"{candidate.GetType().Name} Value Type Got: {valueTypeFunc(jon)}");

                // verify we get the right result
                Console.WriteLine($"{candidate.GetType().Name} Got: {func(jon)}");

                Console.WriteLine($"{Count} iterations would take: {benchmarkMs}ms");

                Console.WriteLine();
            }

            Console.ReadLine();
        }

        public static IEnumerable<ICandidate> GetCandidates()
        {
            // poor man's DI
            return typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => typeof(ICandidate).IsAssignableFrom(t))
                .Select(t => t.GetConstructor(new Type[0]))
                .Where(c => c != null)
                .Select(c => c.Invoke(new object[0]))
                .Cast<ICandidate>();
        }

        public static long Benchmark(Func<object, object> func, object obj, int divisor = 1)
        {
            var watch = Stopwatch.StartNew();
            for (var i = 0; i < Count / divisor; i++)
            {
                func(obj);
            }

            watch.Stop();

            return watch.ElapsedMilliseconds * divisor;
        }
    }
}
