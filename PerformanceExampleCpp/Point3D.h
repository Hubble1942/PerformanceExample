#pragma once

#include <ostream>

class Point3D
{
private:
    double _x;
    double _y;
    double _z;

public:
    Point3D()
        : Point3D{0.0, 0.0, 0.0}
    {}

    Point3D(double x, double y, double z)
        : _x{ x }, _y{ y }, _z{ z }
    {}

    double X() const { return _x; }
    void SetX(double value) { _x = value; }

    double Y() const { return _y; }
    void SetY(double value) { _y = value; }

    double Z() const { return _z; }
    void SetZ(double value) { _z = value; }

    friend std::ostream& operator<< (std::ostream& stream, const Point3D& point)
    {
        return stream << "("
            << point.X() << ", "
            << point.Y() << ", "
            << point.Z()
            << ")";
    }
};
