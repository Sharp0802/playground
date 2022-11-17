#include "MenuPanel.h"

HRESULT MenuPanel::Render(
    const int width, 
    const int height, 
    Palette* palette, 
    Theme* theme, 
    ID2D1HwndRenderTarget* renderTarget)
{
    float widthF = static_cast<float>(width);
    float heightF = static_cast<float>(height);

    HRESULT hr = S_OK;

    if (SUCCEEDED(hr))
    {
        D2D1_RECT_F rect{ 0, 0, widthF, heightF };
        renderTarget->FillRectangle(rect, theme->Background0_0Brush);
    }

    if (SUCCEEDED(hr))
    {
        D2D1_ROUNDED_RECT rect{ { 250, 40, widthF - 250, heightF - 5 }, 5, 5 };
        renderTarget->FillRoundedRectangle(rect, theme->Background1_0Brush);
    }

    return hr;
}
