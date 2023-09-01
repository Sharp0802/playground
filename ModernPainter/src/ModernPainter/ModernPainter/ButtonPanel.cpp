#include "ButtonPanel.h"
#include "Application.h"

REF_APP_INST;

ButtonPanel::ButtonPanel(const float x, const float y) :
	_x(x),
	_y(y),
	_handler(reinterpret_cast<void*>(this), std::function(MouseEventRaised))
{
	ApplicationInstance->OnMouseEvnetRaised += _handler;
}

ButtonPanel::~ButtonPanel()
{
	ApplicationInstance->OnMouseEvnetRaised -= _handler;
}

HRESULT ButtonPanel::Render(
	const int width, 
	const int height, 
	Palette* palette, 
	Theme* theme, 
	ID2D1HwndRenderTarget* renderTarget)
{
	HRESULT hr = S_OK;

	D2D1_ROUNDED_RECT rc;
	rc.radiusX = rc.radiusY = 5;
	rc.rect = { _x, _y, _x + 50, _y + 50 };

	switch (_state)
	{
	case 2:
		renderTarget->DrawRoundedRectangle(rc, theme->Background1_2Brush);
		break;

	case 1:
		renderTarget->DrawRoundedRectangle(rc, theme->Background1_1Brush);
		break;

	case 0:
		renderTarget->DrawRoundedRectangle(rc, theme->Background1_0Brush);
		break;
	}

	return hr;
}

void ButtonPanel::MouseEventRaised(void* sender, WPARAM w, LPARAM l)
{
	ButtonPanel* panel = reinterpret_cast<ButtonPanel*>(sender);

	int32_t x = GET_X_LPARAM(l);
	int32_t y = GET_Y_LPARAM(l);

	if ((w & MK_LBUTTON) != 0) // click
	{
		if (panel->_state != 2)
		{
			panel->OnClicked(panel);
		}
		panel->_state = 2;
	}
	else if (
		panel->_x <= x && x <= panel->_x + 50 &&
		panel->_y <= y && y <= panel->_y + 50) // hover
	{
		panel->_state = 1;
	}
	else
	{
		panel->_state = 0;
	}
}
