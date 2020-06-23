#pragma once

#include <chrono>
#include <iostream>
#include <string>

class Stopwatch
{
private:
    std::string _message;
    std::chrono::steady_clock::time_point _start;

public:
    explicit Stopwatch(const std::string& message)
        : _message{ message }, _start{ std::chrono::high_resolution_clock::now() }
    {}

    ~Stopwatch()
    {
        const auto end = std::chrono::high_resolution_clock::now();
        const auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(end - _start).count();
        std::cout << _message << ": " << duration << " ms";
    }

};
