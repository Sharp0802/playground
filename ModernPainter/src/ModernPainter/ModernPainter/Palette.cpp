#include "Palette.h"
#include "ErrorDialog.h"

Palette::Palette(ID2D1HwndRenderTarget* renderTarget) :
	_d2d1RenderTarget(renderTarget)
{
	_palette = new Map<__int128_t, ID2D1SolidColorBrush*>();
}

Palette::~Palette()
{
	List<ID2D1SolidColorBrush**> vec;
	_palette->GetAll(&vec);
	for (ID2D1SolidColorBrush** p : vec)
	{
		SafeRelease(p);
	}
	delete _palette;
}

__int128_t Palette::GetKey(const D2D1::ColorF& color)
{
	return *reinterpret_cast<__int128_t*>(const_cast<D2D1::ColorF*>(&color));
}

void Palette::EnsureRenderTarget(ID2D1HwndRenderTarget* renderTarget)
{
	_d2d1RenderTarget = renderTarget;
}

HRESULT Palette::Get(const D2D1::ColorF& color, ID2D1SolidColorBrush** brush)
{
	HRESULT hr = S_OK;

	__int128_t key = GetKey(color);
	if (!_palette->ContainsKey(key))
	{
		hr = _d2d1RenderTarget->CreateSolidColorBrush(color, brush);
		DialogWhenError(hr, TEXT("Failed to create ID2D1SolidColorBrush."));
		if (SUCCEEDED(hr))
		{
			_palette->TryAdd(key, *brush);
		}
	}
	else
	{
		if (!_palette->TryGet(key, brush))
			hr = E_FAIL;
	}

	return hr;
}
