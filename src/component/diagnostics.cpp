
#include "component/diagnostics.hpp"
#include "api.h"

using namespace Probo::Diagnostics;

Stopwatch::Stopwatch(bool autoInterruptFlash) : UseAutoInterruptFlash(autoInterruptFlash)
{
    InstanceCount++;
}

void Stopwatch::Start()
{
    Reset();
    StartedTime = timer_read();
}

void Stopwatch::Stop()
{
    if (!HasStopped)
    {
        HasStopped  = true;
        StoppedTime = timer_read();
    }
}

unsigned int Stopwatch::GetElapsedMs() const
{
    return timer_read() - StartedTime;
}

void Stopwatch::Restart()
{
    if (HasStopped)
        StartedTime += timer_read() - StoppedTime;
}

void Stopwatch::Reset()
{
    if (UseAutoInterruptFlash && (InstanceCount == 1))
        timer(0);
    StartedTime = 0;
    StoppedTime = 0;
}

Stopwatch::~Stopwatch()
{
    InstanceCount--;
}
