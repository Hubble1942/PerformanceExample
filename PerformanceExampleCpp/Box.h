#pragma once

#include "Point3D.h"
#include <algorithm>

class Box
{
private:
    Point3D _min;
    Point3D _max;

public:
    explicit Box(const Point3D& point)
        : _min{ point }, _max{ point }
    {}

    Point3D Min() const { return _min; }
    Point3D Max() const { return _max; }

    Point3D Center() const
    {
        return {
            (_min.X() + _max.X()) / 2.0,
            (_min.Y() + _max.Y()) / 2.0,
            (_min.Z() + _max.Z()) / 2.0,
        };
    }

    void Enclose(const Point3D& point)
    {
        _min.SetX(std::min(_min.X(), point.X()));
        _min.SetY(std::min(_min.Y(), point.Y()));
        _min.SetZ(std::min(_min.Z(), point.Z()));

        _max.SetX(std::max(_max.X(), point.X()));
        _max.SetY(std::max(_max.Y(), point.Y()));
        _max.SetZ(std::max(_max.Z(), point.Z()));
    }

    void Enclose(const Box& other)
    {
        Enclose(other.Min());
        Enclose(other.Max());
    }
};
