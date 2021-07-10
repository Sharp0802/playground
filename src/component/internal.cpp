#include "component/internal.hpp"

using namespace Probo;

unsigned char Internal::GetServoMotorSetting(const unsigned char port, const unsigned char num)
{
    return const_cast<unsigned char *>(_S1234)[(port - 1) * 4 - 1 + num];
}

void Internal::SetServoMotorSetting(unsigned char port, unsigned char num, unsigned char value)
{
    const_cast<unsigned char *>(_S1234)[(port - 1) * 4 - 1 + num] = value;
}

unsigned char Internal::GetRemoteFrontData(const unsigned char port)
{
    return PSDATA[1] & (port - 1);
}

unsigned char Internal::GetRemoteDigitalData(const unsigned char port)
{
    return PSDATA[(port - 1) / 4] & (1 << ((port - 1) % 4));
}

unsigned char Internal::GetRemoteAnalogData(const unsigned char port)
{
    return PSDATA[2] & ((0x10 << ((port - 1) % 4)) >> (0x0F << ((port - 1) % 4)));
}

unsigned char Internal::GetInputData(const unsigned char port)
{
    return INPUT & (1 << (port - 1));
}

unsigned char Internal::GetOutputPort(const unsigned char port)
{
    return 1 << (port - 1);
}

unsigned char Internal::GetKeyCount(const unsigned char port)
{
    return KEYCOUNT[port];
}

unsigned char Internal::GetAnalogToDigitalConverterData(const unsigned char port)
{
    return ADCDATA[port - 1];
}

unsigned short Internal::GetWave(const WaveType type, const unsigned char octave)
{
    return static_cast<unsigned short>(type) >> (octave - 4); // type / pow(2, octave);
}

unsigned char Internal::GetColor(const ColorType type, const bool dimming)
{
    if (type == ColorType::Off)
        return 100;
    auto chType = static_cast<unsigned char>(type);
    return dimming ? (90 + chType) : (11 * chType + 1);
}

unsigned char Internal::GetFallingEdge(const unsigned char port)
{
    return (FALLING_EDGE & 0b0001 & ~__fea[port - 1])
           ? (const_cast<unsigned char *>(__fea)[port - 1] = 0xff)
           : (const_cast<unsigned char *>(__fea)[port - 1] = 0);
}

unsigned char Internal::GetRisingEdge(const unsigned char port)
{
    return (RISING_EDGE & 0b0001 & ~__rea[port - 1])
           ? (const_cast<unsigned char *>(__rea)[port - 1] = 0xff)
           : (const_cast<unsigned char *>(__rea)[port - 1] = 0);
}

unsigned char Internal::GetBothEdge(const unsigned char port)
{
    return (BOTH_EDGE & 0b0001 & ~__bea[port - 1])
           ? (const_cast<unsigned char *>(__bea)[port - 1] = 0xff)
           : (const_cast<unsigned char *>(__bea)[port - 1] = 0);
}
