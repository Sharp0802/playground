#include "Theme.h"

HRESULT Theme::Update(Palette* palette)
{
#define GetBrush(name) if(SUCCEEDED(hr)) { hr = palette->Get( name , & name ## Brush); }
	HRESULT hr = S_OK;
	GetBrush(Accent0);
	GetBrush(Accent1);
	GetBrush(Text);
	GetBrush(GrayText);
	GetBrush(Background0_0);
	GetBrush(Background0_1);
	GetBrush(Background0_2);
	GetBrush(Background1_0);
	GetBrush(Background1_1);
	GetBrush(Background1_2);
	return hr;
#undef GetBrush
}
