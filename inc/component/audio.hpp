#ifndef AUDIO_HPP
#define AUDIO_HPP

#include "internal.hpp"

using namespace Probo::Internal;

namespace Probo::Audio
{
	template<const unsigned char port>
	class SoundBoard
	{
	public:
		static void Play(const unsigned char data, const bool loop)
		{
			sound(port, data, loop);
		}

		static unsigned char Record()
		{
			return sound_read(port);
		}
	};

	class Buzzer
	{
	public:
		static void Play(WaveType type, unsigned int octave, unsigned int duration);
	};
}


#endif //AUDIO_HPP
