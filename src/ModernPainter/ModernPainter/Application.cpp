#include "Application.h"
#include "ErrorDialog.h"
#include "MenuPanel.h"

Application::Application(const HINSTANCE hInst) :
	_hInst(hInst),
	_hWnd(nullptr),
	_d2dFactory(nullptr),
	_d2dRenderTarget(nullptr),
    _theme(nullptr),
    _palette(nullptr)
{
}

Application::~Application()
{
	SafeRelease(&_d2dFactory);
	SafeRelease(&_d2dRenderTarget);
    delete _theme;
    delete _palette;
    _palette = nullptr;
    for (size_t i = 0; i < _panels.Size; ++i)
        delete _panels[i];
}

HRESULT Application::Initialize()
{
    HRESULT hr = S_OK;

    InitializeComponents();

    WNDCLASSEX wcex = { sizeof(WNDCLASSEX) };
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = Application::WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = sizeof(LONG_PTR);
    wcex.hInstance = _hInst;
    wcex.hbrBackground = NULL;
    wcex.lpszMenuName = NULL;
    wcex.hCursor = LoadCursor(NULL, IDI_APPLICATION);
    wcex.lpszClassName = TEXT("D2DModernPainter");

    RegisterClassEx(&wcex);

    _hWnd = CreateWindow(
        TEXT("D2DModernPainter"),
        TEXT("Modern Painter"),
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        0,
        0,
        NULL,
        NULL,
        _hInst,
        this);

    if (_hWnd)
    {
        hr = CreateDeviceIndependentResources();

        float_t dpi = GetDpiForWindow(_hWnd);

        SetWindowPos(
            _hWnd,
            NULL,
            NULL,
            NULL,
            static_cast<int>(ceilf(640.f * dpi / 96.f)),
            static_cast<int>(ceilf(480.f * dpi / 96.f)),
            SWP_NOMOVE);
        ShowWindow(_hWnd, SW_SHOW);
        UpdateWindow(_hWnd);
    }

    return hr;
}

void Application::RunMessageLoop()
{
    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

HRESULT Application::CreateDeviceIndependentResources()
{
    HRESULT hr;

    _theme = new Theme();

    hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_MULTI_THREADED, &_d2dFactory);
    DialogWhenError(hr, TEXT("\n\nFailed to create ID2D1Factory."));

    if (SUCCEEDED(hr))
    {
        RECT rc;
        GetClientRect(_hWnd, &rc);

        D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

        hr = _d2dFactory->CreateHwndRenderTarget(
            D2D1::RenderTargetProperties(),
            D2D1::HwndRenderTargetProperties(_hWnd, size),
            &_d2dRenderTarget);
        DialogWhenError(hr, TEXT("\n\nFailed to create ID2D1HwndRenderTarget."));
    }

    return hr;
}

HRESULT Application::CreateDeviceResources()
{
    HRESULT hr = S_OK;
    bool init = !_d2dRenderTarget;

    if (init)
    {
        RECT rc;
        GetClientRect(_hWnd, &rc);

        D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

        hr = _d2dFactory->CreateHwndRenderTarget(
            D2D1::RenderTargetProperties(), 
            D2D1::HwndRenderTargetProperties(_hWnd, size), 
            &_d2dRenderTarget);
        DialogWhenError(hr, TEXT("\n\nFailed to create ID2D1HwndRenderTarget."));
    }

    if (_palette == nullptr)
    {
        _palette = new Palette(_d2dRenderTarget);
        _theme->Update(_palette);
    }

    if (init && _palette)
    {
        _palette->EnsureRenderTarget(_d2dRenderTarget);
    }

    return hr;
}

void Application::DiscardDeviceResources()
{
    SafeRelease(&_d2dRenderTarget);
    delete _palette;
    _palette = nullptr;
}

void Application::InitializeComponents()
{
    _panels.Add(new MenuPanel());
}

HRESULT Application::OnRender()
{
    HRESULT hr = S_OK;

    hr = CreateDeviceResources();
    
    if (SUCCEEDED(hr))
    {
        _d2dRenderTarget->BeginDraw();
        _d2dRenderTarget->SetTransform(D2D1::Matrix3x2F::Identity());
        _d2dRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));
        D2D1_SIZE_F size = _d2dRenderTarget->GetSize();

        int width = static_cast<int>(size.width);
        int height = static_cast<int>(size.height);

        for (Panel* panel : _panels)
        {
            panel->Render(width, height, _palette, _theme, _d2dRenderTarget);
        }
        
        hr = _d2dRenderTarget->EndDraw();
    }

    if (hr == D2DERR_RECREATE_TARGET)
    {
        DiscardDeviceResources();
        hr = S_OK;
    }

    return hr;
}

void Application::OnResize(UINT width, UINT height)
{
    if (_d2dRenderTarget)
    {
        // Note: This method can fail, but it's okay to ignore the
        // error here, because the error will be returned again
        // the next time EndDraw is called.
        _d2dRenderTarget->Resize(D2D1::SizeU(width, height));
    }
}

LRESULT Application::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LRESULT result = 0;

    Application* app = message == WM_CREATE
        ? static_cast<Application*>(reinterpret_cast<LPCREATESTRUCT>(lParam)->lpCreateParams)
        : reinterpret_cast<Application*>(GetWindowLongPtr(hWnd, GWLP_USERDATA));

    bool handled = false;

    if (app)
    {
        switch (message)
        {
        case WM_SIZE:
            app->OnResize(LOWORD(lParam), HIWORD(lParam));
            handled = true;
            break;

        case WM_DISPLAYCHANGE:
            InvalidateRect(hWnd, NULL, FALSE);
            handled = true;
            break;

        case WM_PAINT:
            DialogWhenError(app->OnRender(), TEXT("\n\nFailed to render application."));
            ValidateRect(hWnd, NULL);
            handled = true;
            break;

        case WM_CREATE:
            SetWindowLongPtr(
                hWnd,
                GWLP_USERDATA,
                reinterpret_cast<LONG_PTR>(app));
            result = 1;
            handled = true;
            break;

        case WM_DESTROY:
            PostQuitMessage(0);
            result = 1;
            handled = true;
            break;
        }
    }

    if (!handled)
    {
        result = DefWindowProc(hWnd, message, wParam, lParam);
    }

    return result;
}
