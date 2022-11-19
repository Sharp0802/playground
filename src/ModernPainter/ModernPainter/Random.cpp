#include "Random.h"


static uint32_t s0 = 1;
static uint32_t s1 = 0;

uint32_t rand_u32()
{
	s1 ^= s0;
	s0 = _rotl(s0, 26) ^ s1 ^ (s1 << 9);
	s1 = _rotl(s1, 13);
	return _rotl(s0 + s1, 17) + s0;
}
