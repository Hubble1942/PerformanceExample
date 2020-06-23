
#include "Polyline.h"
#include "Stopwatch.h"
#include "ctpl_stl.h"

#include <cstdlib>
#include <ctime>
#include <chrono>
#include <iostream>
#include <thread>
#include <future>

double Random() { return (double)rand() / RAND_MAX; }

std::vector<Polyline> CreateTestData()
{
    static const int numberOfPolylines = 50000;
    static const int numberOfPoints = 1000;

    Stopwatch stopwatch{ "Creating test data" };

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

Box CachedBoxesPerLine(const std::vector<Polyline>& polylines)
{
    Stopwatch stopwatch{ "CachedBoxesPerLine" };
    Box box{ *polylines[0].BoundingBox() };

    for (const auto& polyline : polylines)
    {
        box.Enclose(*polyline.BoundingBox());
    }

    return box;
}

void Worker(std::vector<Polyline>::const_iterator begin, std::vector<Polyline>::const_iterator end, std::promise<Box> result)
{
    Box box{ begin->Points()[0] };
    while (begin != end)
    {
        for (const auto& point : begin->Points())
        {
            box.Enclose(point);
        }

        ++begin;
    }

    result.set_value(box);
}

Box Multithreaded(const std::vector<Polyline>& polylines)
{
    static const int numberOfThreads = 8;
    Stopwatch stopwatch{ "Multithreaded" };

    std::vector<std::future<Box>> futures;
    futures.reserve(numberOfThreads);

    std::vector<std::thread> threads;
    threads.reserve(numberOfThreads);

    const auto junkSize = polylines.size() / numberOfThreads;
    for (int i = 0; i < numberOfThreads; ++i)
    {
        std::promise<Box> promise;
        futures.push_back(promise.get_future());

        const auto begin = polylines.cbegin() + i * junkSize;
        const auto end = begin + junkSize;
        threads.emplace_back(Worker, begin, end, std::move(promise));
    }

    Box box{ polylines[0].Points()[0] };

    for (auto& thread : threads)
    {
        thread.join();
    }

    for (auto& future : futures)
    {
        box.Enclose(future.get());
    }

    return box;
}

Box ThreadPooled(const std::vector<Polyline>& polylines, ctpl::thread_pool& pool)
{
    Stopwatch stopwatch{ "ThreadPooled" };

    std::vector<std::future<Box>> results(pool.size());
    const auto junkSize = polylines.size() / pool.size();
    for (int i = 0; i < pool.size(); ++i)
    {
        auto begin = polylines.cbegin() + i * junkSize;
        const auto end = begin + junkSize;

        results[i] = pool.push([begin, end](int)
            {
                Box box{ begin->Points()[0] };
                for (auto i = begin; i != end; ++i)
                {
                    for (const auto& point : i->Points())
                    {
                        box.Enclose(point);
                    }
                }

                return box;
            });
    }

    Box box{ polylines[0].Points()[0] };
    for (auto& result : results)
    {
        box.Enclose(result.get());
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

    std::cout << " - " << CachedBoxesPerLine(testData).Center() << "\n";
    std::cout << " - " << CachedBoxesPerLine(testData).Center() << "\n";
    std::cout << " - " << CachedBoxesPerLine(testData).Center() << "\n";
    std::cout << " - " << CachedBoxesPerLine(testData).Center() << "\n";

    std::cout << " - " << Multithreaded(testData).Center() << "\n";
    std::cout << " - " << Multithreaded(testData).Center() << "\n";
    std::cout << " - " << Multithreaded(testData).Center() << "\n";
    std::cout << " - " << Multithreaded(testData).Center() << "\n";

    ctpl::thread_pool pool(8);
    std::cout << " - " << ThreadPooled(testData, pool).Center() << "\n";
    std::cout << " - " << ThreadPooled(testData, pool).Center() << "\n";
    std::cout << " - " << ThreadPooled(testData, pool).Center() << "\n";
    std::cout << " - " << ThreadPooled(testData, pool).Center() << "\n";

    return 0;
}
