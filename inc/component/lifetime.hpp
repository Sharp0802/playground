
#ifndef LIFETIME_HPP
#define LIFETIME_HPP

namespace Probo::Lifetime
{
	class MainInitializer
	{
	private:
		void (*Init)();
		void (*Main)();
		void (*Dispose)();
		int Activate();
	public:
		explicit MainInitializer(void (*init)(), void (*main)(), void (*dispose)());
	};
}

#endif //LIFETIME_HPP
