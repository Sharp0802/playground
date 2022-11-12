#pragma once

#include "framework.h"
#include "Map.h"

class Palette
{
public:
	Palette(ID2D1HwndRenderTarget* renderTarget);
	~Palette();

	Palette(const Palette&) = delete;
	Palette operator =(const Palette&) = delete;

private:
	__int128_t GetKey(const D2D1::ColorF& color);

public:
	HRESULT Get(const D2D1::ColorF& color, ID2D1SolidColorBrush** brush);

private:
	ID2D1HwndRenderTarget* _d2d1RenderTarget;
	Map<__int128_t, ID2D1SolidColorBrush*>* _palette;
};
