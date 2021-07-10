#ifndef EEPROM_HPP
#define EEPROM_HPP

namespace Probo::Memory::NonVolatile
{
	class EEPROM
	{
	private:
		static bool Initialized;

		static void Initialize();

	public:
		static unsigned int CurrentTop;

		static unsigned char Register(void *adr, unsigned int size);

		static unsigned char Unregister(void *adr, unsigned int size);
	};
}


#endif //EEPROM_HPP
