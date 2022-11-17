#pragma once

#include "framework.h"
#include "Panel.h"
#include "Theme.h"


class ToolPanel : public Panel
{
public:
	virtual HRESULT Render(
		const int width, 
		const int height, 
		Palette* palette, 
		Theme* theme, 
		ID2D1HwndRenderTarget* renderTarget) override;
};

