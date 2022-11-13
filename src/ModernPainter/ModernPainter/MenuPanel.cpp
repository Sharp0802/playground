#include "MenuPanel.h"

HRESULT MenuPanel::Render(const int width, const int height, Palette* palette, ID2D1HwndRenderTarget* renderTarget)
{
    float widthF = static_cast<float>(width);
    float heightF = static_cast<float>(height);

    HRESULT hr = S_OK;

    ID2D1SolidColorBrush* darker = nullptr;
    ID2D1SolidColorBrush* lighter = nullptr;
    ID2D1SolidColorBrush* background = nullptr;

    if (SUCCEEDED(hr))
    {
        hr = palette->Get(D2D1::ColorF(0.13f, 0.13f, 0.13f), &darker);
    }

    if (SUCCEEDED(hr))
    {
        hr = palette->Get(D2D1::ColorF(0.17f, 0.17f, 0.17f), &lighter);
    }

    if (SUCCEEDED(hr))
    {
        hr = palette->Get(D2D1::ColorF(0.20f, 0.20f, 0.20f), &background);
    }

    if (SUCCEEDED(hr))
    {
        D2D1_RECT_F rect{ 0, 0, widthF, heightF };
        renderTarget->FillRectangle(rect, darker);
    }

    if (SUCCEEDED(hr))
    {
        D2D1_ROUNDED_RECT rect{ { 250, 40, widthF - 250, heightF - 5 }, 5, 5 };
        renderTarget->FillRoundedRectangle(rect, background);
    }

    return hr;
}
