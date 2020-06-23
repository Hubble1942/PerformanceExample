using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    /// <summary>
    /// A line defined by multiple points.
    /// </summary>
    public sealed class Polyline : IEnumerable<Point3D>
    {
        private readonly List<Point3D> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline"/> class.
        /// </summary>
        public Polyline()
            : this(new List<Point3D>())
        {
        }

        public Polyline(IEnumerable<Point3D> points)
        {
            this.points = points.ToList();
            this.BoundingBox = this.points.Aggregate(new Box(), (box, p) => box.Enclose(p));
        }

        /// <summary>
        /// Gets the <see cref="Point3D"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public Point3D this[int index] => this.points[index];

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count => this.points.Count;

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        public Box BoundingBox { get; private set; }

        /// <summary>
        /// Adds the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        public void Add(Point3D point)
        {
            this.points.Add(point);
            this.BoundingBox.Enclose(point);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Point3D> GetEnumerator() => this.points.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => this.points.GetEnumerator();
    }
}
