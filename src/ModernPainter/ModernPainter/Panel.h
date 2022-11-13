#pragma once

#include "framework.h"
#include "Palette.h"

class Panel
{
public:
	virtual ~Panel() {}

	virtual HRESULT Render(const int width, const int height, Palette* palette, ID2D1HwndRenderTarget* renderTarget) = 0;
};
