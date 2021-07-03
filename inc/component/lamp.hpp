#ifndef LAMP_HPP
#define LAMP_HPP

#include "api.h"
#include "internal.hpp"

using namespace Probo::Internal;

namespace Probo::Lamp
{
	class ILamp
	{
	public:
		virtual void SetColor(ColorType type, bool dimming) = 0;

		virtual void SetState(bool) = 0;
	};

	template<const unsigned char port> class SolidLamp : ILamp
	{
	private:
		void SetColor(ColorType type, bool dimming) override {}

	public:
		void SetState(const bool state) override
		{
			if (state)
				on(port);
			else
				off(port);
		}
	};

	template<const unsigned char port> class DynamicLamp : ILamp
	{
	private:
		void SetState(const bool state) override {}

	public:
		void SetColor(ColorType type, bool dimming) override
		{
			servo(port, GetColor(type, dimming));
		}
	};
}


#endif
