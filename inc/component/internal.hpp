#ifndef INTERNAL_HPP
#define INTERNAL_HPP

#include <stdint.h>

#include "api.h"

#define PURE __attribute__((pure))
#define CONST __attribute__((const))
#define NODISCARD [[nodiscard]]

namespace Probo::Internal
{
    enum class WaveType : unsigned short
    {
        Do   = 35391,
        XDo  = 33405,
        Re   = 31530,
        XRe  = 29760,
        Mi   = 28090,
        Pa   = 26513,
        XPa  = 25025,
        Sol  = 23621,
        XSol = 22295,
        Ra   = 21044,
        XRa  = 19863,
        Si   = 18748,
        Ci   = 18748
    };

    enum class ColorType : unsigned char
    {
        Off     = UINT8_MAX,
        Red     = 0,
        Yellow  = 1,
        Green   = 2,
        Cyan    = 3,
        Blue    = 4,
        Magenta = 5,
        White   = 6
    };

    NODISCARD PURE unsigned char GetServoMotorSetting(unsigned char port, unsigned char num);

    void SetServoMotorSetting(unsigned char port, unsigned char num, unsigned char value);

    NODISCARD PURE unsigned char GetRemoteFrontData(unsigned char port);

    NODISCARD PURE unsigned char GetRemoteDigitalData(unsigned char port);

    NODISCARD PURE unsigned char GetRemoteAnalogData(unsigned char port);

    NODISCARD PURE unsigned char GetInputData(unsigned char port);

    NODISCARD CONST unsigned char GetOutputPort(unsigned char port);

    NODISCARD PURE unsigned char GetKeyCount(unsigned char port);

    NODISCARD PURE unsigned char GetAnalogToDigitalConverterData(unsigned char port);

    NODISCARD CONST unsigned short GetWave(WaveType type, unsigned char octave);

    NODISCARD CONST unsigned char GetColor(ColorType type, bool dimming = false);

    NODISCARD unsigned char GetFallingEdge(unsigned char port);

    NODISCARD unsigned char GetRisingEdge(unsigned char port);

    NODISCARD unsigned char GetBothEdge(unsigned char port);
}

#undef PURE
#undef CONST
#undef NODISCARD

#endif
