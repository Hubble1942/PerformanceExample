
#include "Polyline.h"
#include "Stopwatch.h"
#include <cstdlib>
#include <ctime>
#include <chrono>
#include <iostream>

double Random() { return (double)rand() / RAND_MAX; }

std::vector<Polyline> CreateTestData()
{
    Stopwatch stopwatch{ "Creating test data" };
    const int numberOfPolylines = 50000;
    const int numberOfPoints = 1000;

    std::vector<Polyline> testData;
    testData.reserve(numberOfPolylines);

    for (int i = 0; i < numberOfPolylines; ++i)
    {
        testData.emplace_back();
        testData.back().Reserve(numberOfPoints);
        for (int j = 0; j < numberOfPoints; ++j)
        {
            testData.back().Add({ Random(), Random(), Random() });
        }
    }

    return testData;
}

Box Simple(const std::vector<Polyline>& polylines)
{
    Stopwatch stopwatch{ "Simple" };
    Box box{ polylines[0].Points()[0] };

    for (const auto& polyline : polylines)
    {
        for (const auto& point : polyline.Points())
        {
            box.Enclose(point);
        }
    }

    return box;
}

int main(int argc, char* argv[])
{
    std::srand(static_cast<unsigned int>(std::time(nullptr)));

    const auto testData = CreateTestData();
    std::cout << "\n";

    std::cout << " - " << Simple(testData).Center() << "\n";
    std::cout << " - " << Simple(testData).Center() << "\n";
    std::cout << " - " << Simple(testData).Center() << "\n";
    std::cout << " - " << Simple(testData).Center() << "\n";

    return 0;
}
