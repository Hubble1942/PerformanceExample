using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    internal static class BoundingBoxCalculator
    {
        // Lock for the multithreaded loop.
        private static object theLock = new object();

        /// <summary>
        /// Original code.
        /// </summary>
        public static Point3D CalculateOriginal(IEnumerable<Polyline> polylines)
        {
            var minValues = new Point3D(double.MaxValue, double.MaxValue, double.MaxValue);
            var maxValues = new Point3D(double.MinValue, double.MinValue, double.MinValue);

            foreach (var line in polylines)
            {
                foreach (var point in line)
                {
                    minValues.X = Math.Min(minValues.X, point.X);
                    minValues.Y = Math.Min(minValues.Y, point.Y);
                    minValues.Z = Math.Min(minValues.Z, point.Z);

                    maxValues.X = Math.Max(maxValues.X, point.X);
                    maxValues.Y = Math.Max(maxValues.Y, point.Y);
                    maxValues.Z = Math.Max(maxValues.Z, point.Z);
                }
            }

            return (Point3D)(((Vector3D)maxValues + (Vector3D)minValues) / 2.0);
        }

        /// <summary>
        /// Performance optimized to allow fluent, interactive positioning.
        /// Release mode: 1749 ms => 113 ms.
        /// </summary>
        /// <remarks>
        /// This remark has just been added, because this example is used in a blog. In practice, 
        /// I would just add the summary above.
        /// - Replaced IEnumerable with List: 1749 ms => 1229 ms.
        /// - Replaced Math.Min and Math.Max with "if smaller/larger => assign". 1229 ms => 984 ms.
        /// - Use double instead of Point3D for intermediate results. 984 ms => 934 ms.
        /// - Use for loops instead of foreach loops. 934 ms => 334 ms.
        /// - Use Parallel.For instead of for. 334 ms => 113 ms. With 4 (8) cores, ProcessorCount = 8. 
        ///   When all thread pool threads are in use (sleeping), without SetMinThreads => 423 ms (!).
        /// </remarks>
        public static Point3D CalculatePerformanceOptimized(List<Polyline> polylines)
        {
            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            int sufficientWorkerThreads = workerThreads + 2 * Environment.ProcessorCount;
            ThreadPool.SetMinThreads(sufficientWorkerThreads, completionPortThreads);

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double minZ = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double maxZ = double.MinValue;

            Parallel.For(0, polylines.Count, polylineIndex =>
            {
                double innerMinX = double.MaxValue;
                double innerMinY = double.MaxValue;
                double innerMinZ = double.MaxValue;
                double innerMaxX = double.MinValue;
                double innerMaxY = double.MinValue;
                double innerMaxZ = double.MinValue;

                var line = polylines[polylineIndex];
                for (int pointIndex = 0; pointIndex < line.Count; pointIndex++)
                {
                    Point3D point = line[pointIndex];
                    if (point.X < innerMinX) { innerMinX = point.X; }
                    if (point.Y < innerMinY) { innerMinY = point.Y; }
                    if (point.Z < innerMinZ) { innerMinZ = point.Z; }

                    if (point.X > innerMaxX) { innerMaxX = point.X; }
                    if (point.Y > innerMaxY) { innerMaxY = point.Y; }
                    if (point.Z > innerMaxZ) { innerMaxZ = point.Z; }
                }

                lock (theLock)
                {
                    if (innerMinX < minX) { minX = innerMinX; }
                    if (innerMinY < minY) { minY = innerMinY; }
                    if (innerMinZ < minZ) { minZ = innerMinZ; }

                    if (innerMaxX > maxX) { maxX = innerMaxX; }
                    if (innerMaxY > maxY) { maxY = innerMaxY; }
                    if (innerMaxZ > maxZ) { maxZ = innerMaxZ; }
                }
            });

            ThreadPool.SetMinThreads(workerThreads, completionPortThreads);

            return new Point3D(minX + maxX / 2.0, minY + maxY / 2.0, minZ + maxZ / 2.0);
        }

        public static Point3D CalculateV3(IEnumerable<Polyline> polylines)
        {
            var initialPoint = polylines.SelectMany(line => line).FirstOrDefault();
            return polylines.Aggregate(new Box(initialPoint), (box, line) => box.Enclose(line.BoundingBox)).Center;
        }

        public static Point3D CalculateV4(IEnumerable<Polyline> polylines)
        {
            var initialPoint = polylines.SelectMany(line => line).FirstOrDefault();

            return polylines
                .AsParallel()
                .Select(line => line.Aggregate(new Box(line[0]), (box, point) => box.Enclose(point)))
                .Aggregate(new Box(initialPoint), (b1, b2) => b1.Enclose(b2)).Center;
        }
    }
}
