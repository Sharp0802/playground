#pragma once

#include "framework.h"
#include "Panel.h"
#include "EventSource.h"


class ButtonPanel : public Panel
{
private:
	const float _x;
	const float _y;

	uint8_t _state = 0;

	EventHandler<WPARAM, LPARAM> _handler;

public:
	EventSource<ButtonPanel*> OnClicked;

public:
	ButtonPanel(const float x, const float y);
	~ButtonPanel();

public:
	virtual HRESULT Render(
		const int width,
		const int height,
		Palette* palette,
		Theme* theme,
		ID2D1HwndRenderTarget* renderTarget) override;

private:
	static void MouseEventRaised(void* sender, WPARAM w, LPARAM l);
};

