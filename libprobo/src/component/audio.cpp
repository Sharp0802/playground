
#include "component/audio.hpp"

void Probo::Audio::Buzzer::Play(const WaveType type, const unsigned int octave, const unsigned int duration)
{
	play(GetWave(type, octave), duration);
}
