using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace PerformanceExample
{
    /// <summary>
    /// A box defined by two corner points.
    /// </summary>
    public struct Box
    {
        public bool IsValid { get; private set; }

        public Point3D Min { get; private set; }

        public Point3D Max { get; private set; }

        public Point3D Center => (Point3D)(((Vector3D)this.Min + (Vector3D)this.Max) / 2.0);

        public Box Enclose(Point3D point)
        {
            if (this.IsValid)
            {
                return new Box
                {
                    Min = new Point3D(
                        Math.Min(this.Min.X, point.X),
                        Math.Min(this.Min.Y, point.Y),
                        Math.Min(this.Min.Z, point.Z)),
                    Max = new Point3D(
                        Math.Max(this.Max.X, point.X),
                        Math.Max(this.Max.Y, point.Y),
                        Math.Max(this.Max.Z, point.Z)),
                    IsValid = true,
                };
            }
            else
            {
                return new Box
                {
                    Min = point,
                    Max = point,
                    IsValid = true,
                };
            }
        }

        public Box Enclose(Box other)
        {
            if (other.IsValid)
            {
                return this.Enclose(other.Min).Enclose(other.Max);
            }
            else
            {
                return this;
            }
        }
    }
}
