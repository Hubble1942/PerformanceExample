#pragma once

#include "Box.h"
#include "Point3D.h"
#include <memory>
#include <vector>

class Polyline
{
private:
    std::vector<Point3D> _points;
    std::unique_ptr<Box> _boundingBox;

public:
    Polyline() {}

    explicit Polyline(const std::vector<Point3D>& points)
        : _points{ points }
    {
        if (!_points.empty())
        {
            _boundingBox = std::make_unique<Box>(points.front());

            for (const auto& point : _points)
            {
                _boundingBox->Enclose(point);
            }
        }
    }

    const std::vector<Point3D>& Points() const { return _points; }
    const std::unique_ptr<Box>& BoundingBox() const { return _boundingBox; }

    void Add(const Point3D& point)
    {
        _points.push_back(point);

        if (_boundingBox)
        {
            _boundingBox->Enclose(point);
        }
        else
        {
            _boundingBox = std::make_unique<Box>(point);
        }
    }

    void Reserve(std::size_t size)
    {
        _points.reserve(size);
    }
};
