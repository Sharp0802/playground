#pragma once

#pragma comment(lib, "d2d1.lib")
#pragma comment(lib, "dwmapi.lib")

#include <windows.h>
#include <uxtheme.h>
#include <dwmapi.h>
#include <comdef.h>

#include <atomic>
#include <cstdlib>
#include <cstdint>
#include <cstring>

#include <intrin.h>

#include <d2d1.h>
#include <d2d1helper.h>
#include <dwrite.h>

#ifndef _tWinMain
#	ifdef _UNICODE
#		define _tWinMain wWinMain
#	else
#		define _tWinMain WinMain
#	endif
#endif

template<class Interface>
void SafeRelease(Interface** ppInterface)
{
	if ((*ppInterface) != nullptr)
	{
		(*ppInterface)->Release();
		(*ppInterface) = nullptr;
	}
}
