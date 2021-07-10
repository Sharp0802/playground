
#ifndef DIAGNOSTICS_HPP
#define DIAGNOSTICS_HPP

#define PURE __attribute__((pure))
#define CONST __attribute__((const))
#define NODISCARD [[nodiscard]]

namespace Probo::Diagnostics
{
    class Stopwatch
    {
    private:
        bool         UseAutoInterruptFlash = false;
        bool         HasStopped            = false;
        unsigned int StartedTime           = 0;
        unsigned int StoppedTime           = 0;
        static int   InstanceCount;
    public:
        Stopwatch(Stopwatch &) = delete;

        explicit Stopwatch(bool autoInterruptFlash);

        void Start();

        void Stop();

        NODISCARD unsigned int GetElapsedMs() const;

        void Restart();

        void Reset();

        ~Stopwatch();
    };
}

#undef PURE
#undef CONST
#undef NODISCARD

#endif
