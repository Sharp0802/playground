#include "fw.h"

#define CLASS_NAME "lwman"
#define APP_NAME "lwman"

LRESULT CALLBACK WndProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

int main(void)
{
    HINSTANCE hInstance = GetModuleHandle(NULL);

    WNDCLASS wc;
    MEMSET0(wc);
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = TEXT(CLASS_NAME);

    RegisterClass(&wc);

    HWND hwnd = CreateWindowEx(
        0,
        TEXT(CLASS_NAME),
        TEXT(APP_NAME),
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
        NULL,
        NULL,
        hInstance,
        NULL);

    if (hwnd == NULL)
    {
        MessageBox(NULL, TEXT("failed to create window."), TEXT("ERROR"), MB_ICONERROR);
        return -1;
    }

    ShowWindow(hwnd, SW_NORMAL);

    MSG msg;
    MEMSET0(msg);
    while (GetMessage(&msg, NULL, 0, 0) > 0)
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    switch (uMsg)
    {
    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;

    case WM_PAINT:
        return 0;

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
}
