#pragma once

#include "framework.h"


class SpinLock
{
private:
	std::atomic<bool> _lock = { false };

public:
	SpinLock();
	SpinLock(const SpinLock&) = delete;
	SpinLock& operator =(const SpinLock&) = delete;

public:
	void Lock();
	void Unlock();
};

