
#include "component/eeprom.hpp"
#include "api.h"

#include <stdint.h>

using namespace Probo::Memory::NonVolatile;

void EEPROM::Initialize()
{
	if (!Initialized)
		CurrentTop = EEPROM_read(UINT8_MAX - 1);
}

unsigned char EEPROM::Register(void *adr, unsigned int size)
{
	Initialize();
	for (unsigned int i = 0; i < size; ++i)
		EEPROM_write(CurrentTop + i, reinterpret_cast<unsigned char *>(adr)[i]);
	CurrentTop += size;
	EEPROM_write(UINT8_MAX - 1, CurrentTop);
	return CurrentTop;
}

unsigned char EEPROM::Unregister(void *adr, unsigned int size)
{
	Initialize();
	for (unsigned int i = 0; i < size; ++i)
	{
		reinterpret_cast<unsigned char *>(adr)[i] = EEPROM_read(CurrentTop - size + i);
		EEPROM_write(CurrentTop - size + i, 0b0);
	}
	CurrentTop -= size;
	return CurrentTop;
}
