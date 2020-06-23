using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    /// <summary>
    /// A box defined by two corner points.
    /// </summary>
    public sealed class Box
    {
        private Point3D min;
        private Point3D max;

        /// <summary>
        /// Returns true if this box is valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the minimum corner point.
        /// </summary>
        public Point3D Min => this.min;

        /// <summary>
        /// Gets the maximum corner point.
        /// </summary>
        public Point3D Max => this.max;

        /// <summary>
        /// Gets the center point.
        /// </summary>
        public Point3D Center => (Point3D)(((Vector3D)this.Min + (Vector3D)this.Max) / 2.0);

        /// <summary>
        /// Extends this box to enclosing the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>This instance.</returns>
        public Box Enclose(Point3D point)
        {
            if (this.IsValid)
            {
                this.min.X = Util.Min(this.min.X, point.X);
                this.min.Y = Util.Min(this.min.Y, point.Y);
                this.min.Z = Util.Min(this.min.Z, point.Z);

                this.max.X = Util.Max(this.max.X, point.X);
                this.max.Y = Util.Max(this.max.Y, point.Y);
                this.max.Z = Util.Max(this.max.Z, point.Z);
            }
            else
            {
                this.min = point;
                this.max = point;
                this.IsValid = true;
            }

            return this;
        }

        /// <summary>
        /// Extends this box to enclosing the specified other box.
        /// </summary>
        /// <param name="other">The other box.</param>
        /// <returns>
        /// This instance.
        /// </returns>
        public Box Enclose(Box other)
        {
            if (other.IsValid)
            {
                this.Enclose(other.Min);
                this.Enclose(other.Max);
            }

            return this;
        }
    }
}
