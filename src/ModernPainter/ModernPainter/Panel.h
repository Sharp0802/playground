#pragma once

#include "framework.h"
#include "Palette.h"
#include "Theme.h"

class Panel
{
public:
	virtual ~Panel() {}

	virtual HRESULT Render(
		const int width, 
		const int height, 
		Palette* palette, 
		Theme* theme,
		ID2D1HwndRenderTarget* renderTarget) = 0;
};
