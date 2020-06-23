using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    class Program
    {
        private static readonly Random Random = new Random();

        static void Main()
        {
            Console.WriteLine("Program started.");

            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            var inputData = CreateInputData();
            var usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"InputData created in {usedMilliseconds} milliseconds.");

            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());

            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());
            ThreadPool.QueueUserWorkItem(SleepABit, new Object());

            TestRun("Original calculation", BoundingBoxCalculator.CalculateOriginal, inputData);
            TestRun("Optimized calculation", BoundingBoxCalculator.CalculatePerformanceOptimized, inputData);
            TestRun("Calculation V3", BoundingBoxCalculator.CalculateV3, inputData);
            TestRun("Calculation V4", BoundingBoxCalculator.CalculateV4, inputData);

            Console.WriteLine("Program finished.");
        }

        private static void TestRun(string name, Func<List<Polyline>, Point3D> candidate, List<Polyline> testData)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            var result = candidate(testData);
            var usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Cache warmed in {usedMilliseconds} milliseconds. Result: ({result}).");

            stopwatch.Restart();
            result = candidate(testData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{name} of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = candidate(testData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{name} of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = candidate(testData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{name} of {result} used {usedMilliseconds} milliseconds.");

        }

        private static List<Polyline> CreateInputData()
        {
            const int numberOfPolylines = 50000;
            const int numberOfPoints = 1000;

            return Enumerable.Range(0, numberOfPolylines)
                .Select(_ => new Polyline(Enumerable.Range(0, numberOfPoints).Select(__ => RandomPoint())))
                .ToList();
        }

        private static Point3D RandomPoint() => new Point3D(Random.NextDouble(), Random.NextDouble(), Random.NextDouble());

        private static void SleepABit(Object theObject)
        {
            Thread.Sleep(10000);
        }
    }
}
