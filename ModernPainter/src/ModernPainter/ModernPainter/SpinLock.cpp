#include "SpinLock.h"

SpinLock::SpinLock()
{
}

void SpinLock::Lock()
{
	while (true)
	{
		if (!_lock.exchange(true, std::memory_order_acquire))
			break;
		while (_lock.load(std::memory_order_relaxed))
			_mm_pause();
	}
}

void SpinLock::Unlock()
{
	_lock.store(false, std::memory_order_release);
}
