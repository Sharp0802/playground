
#include "component/threading.hpp"
#include "component/lifetime.hpp"
#include "component/audio.hpp"

using namespace Probo::Audio;
using namespace Probo::Lifetime;
using namespace Probo::Threading;

int MainInitializer::Activate()
{
	try
	{
		Init();
		start();
		Main();
		end();
		Dispose();
		return 0;
	}
	catch (...)
	{
		for (unsigned char i = 0; i < 3; ++i)
		{
			Buzzer::Play(WaveType::Do, 4, 100);
			Thread::Sleep(100)
		}
		return -1;
	}
}

MainInitializer::MainInitializer(void (*init)(), void (*main)(), void (*dispose)())
		: Init(init), Main(main), Dispose(dispose)
{
	Activate();
}
