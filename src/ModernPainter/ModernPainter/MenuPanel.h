#pragma once

#include "Panel.h"

class MenuPanel : public Panel
{
public:
	virtual HRESULT Render(const int width, const int height, Palette* palette, ID2D1HwndRenderTarget* renderTarget) override;
};
