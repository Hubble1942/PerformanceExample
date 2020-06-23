using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Program started.");

            List<List<Point3D>> inputData = CreateInputData();

            var stopwatch = new Stopwatch();
            Point3D result;
            long usedMilliseconds;

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
            Console.ReadKey();
        }

        private static List<List<Point3D>> CreateInputData()
        {
            const int numberOfPolylines = 50000;
            const int numberOfPoints = 1000;

            List<List<Point3D>> returnValue = new List<List<Point3D>>();

            for (int iPolyline = 0; iPolyline < numberOfPolylines; iPolyline++)
            {
                var newPolyline = new List<Point3D>();
                
                for (int iPoint = 0; iPoint < numberOfPoints; iPoint++)
                {
                    newPolyline.Add(new Point3D(iPolyline + iPoint, iPolyline + iPoint, iPolyline + iPoint));
                }

                returnValue.Add(newPolyline);
            }

            return returnValue;
        }

        private static void SleepABit(Object theObject)
        {
            Thread.Sleep(10000);
        }
    }
}
