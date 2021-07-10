#ifndef MOTOR_HPP
#define MOTOR_HPP

#include "api.h"

namespace Probo::Motor
{
    class IMotor
    {
    public:
        virtual void SetDegree(unsigned char degree) = 0;

        virtual void SetSpeed(signed char speed) = 0;
    };

    template<const unsigned char> struct DCMotorMethodGetter
    {
    };

    template<> struct DCMotorMethodGetter<1>
    {
        static constexpr void (*Setter)(signed char) = motor1;
    };

    template<> struct DCMotorMethodGetter<2>
    {
        static constexpr void (*Setter)(signed char) = motor2;
    };

    template<> struct DCMotorMethodGetter<3>
    {
        static constexpr void (*Setter)(signed char) = motor3;
    };

    template<> struct DCMotorMethodGetter<4>
    {
        static constexpr void (*Setter)(signed char) = motor4;
    };

    template<const unsigned char port> class DirectCurrentMotor : private IMotor
    {
    private:
        static constexpr void (*Setter)(signed char) = DCMotorMethodGetter<port>::Setter;

        void SetDegree(const unsigned char) override {}

    public:
        void SetSpeed(const signed char speed) override
        {
            Setter(speed);
        }
    };

    template<const unsigned char port> class ServoMotor : private IMotor
    {
    private:
        void SetSpeed(const signed char) override {}

    public:
        void SetDegree(const unsigned char degree) override
        {
            servo(port, degree);
        }
    };
}

#endif
