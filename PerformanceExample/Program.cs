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

            long usedMilliseconds;
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            var inputData = CreateInputData();
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"InputData created in {usedMilliseconds} milliseconds.");

            Point3D result;

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

            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculateOriginal(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Cache warmed in {usedMilliseconds} milliseconds. Result: ({result}).");

            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculateOriginal(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Original calculation of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculateOriginal(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Original calculation of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculateOriginal(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Original calculation of {result} used {usedMilliseconds} milliseconds.");

            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculatePerformanceOptimized(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Cache warmed in {usedMilliseconds} milliseconds. Result: ({result}).");

            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculatePerformanceOptimized(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Optimized calculation of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculatePerformanceOptimized(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Optimized calculation of {result} used {usedMilliseconds} milliseconds.");
            stopwatch.Restart();
            result = BoundingBoxCalculator.CalculatePerformanceOptimized(inputData);
            usedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Optimized calculation of {result} used {usedMilliseconds} milliseconds.");

            Console.WriteLine("Program finished.");
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
