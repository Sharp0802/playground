#include "fw.h"
#include "ui.h"

#define CREDIT            \
    "lwman (LWMAN) 0.1\n" \
    "This is free software; see the source for copying conditions."

#define CLASS_NAME "lwman"
#define APP_NAME "lwman"

#define WS_NONE 0

#define WIDTH 420
#define HEIGHT 620

#define MARGIN 10
#define SPACING 5

LRESULT CALLBACK WndProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

int main(void)
{
    HINSTANCE hInstance = GetModuleHandle(NULL);

    WNDCLASS wc;
    MEMSET0(wc);
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = TEXT(CLASS_NAME);
    wc.style = CS_HREDRAW | CS_VREDRAW;

    RegisterClass(&wc);

    HWND hwnd = CreateWindowEx(
        0,
        TEXT(CLASS_NAME),
        TEXT(APP_NAME),
        WS_OVERLAPPEDWINDOW & ~WS_THICKFRAME,
        CW_USEDEFAULT, CW_USEDEFAULT, WIDTH, HEIGHT,
        NULL,
        NULL,
        hInstance,
        NULL);
    if (hwnd == NULL)
        exit(-1);

    ShowWindow(hwnd, SW_SHOW);
    UpdateWindow(hwnd);

    MSG msg;
    MEMSET0(msg);
    while (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE) > 0)
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
    case WM_CREATE:
    {
        const int width = WIDTH - 2 * MARGIN;
        const int height = HEIGHT - 2 * MARGIN;
        const int btW = 60;

        struct UI_STACK ui = NewUIStack(hwnd, MARGIN, MARGIN, width, height, SPACING);

        struct NODE credit = ui.lpfnTop(&ui, C_ST, 2, 0, UI_AUTO);
        credit.lpfnStyle(&credit, WS_NONE, TEXT(CREDIT));
        credit.lpfnCreate(&credit);

        struct NODE path = ui.lpfnTop(&ui, C_IN, 2, 0, width - btW - SPACING);
        path.lpfnStyle(&path, WS_BORDER, TEXT("C:\\Users\\"));
        path.lpfnCreate(&path);

        struct NODE browse = ui.lpfnTop(&ui, C_BN, 2, 0, btW);
        browse.lpfnStyle(&browse, WS_NONE, TEXT("Browse"));
        browse.lpfnCreate(&browse);

        struct NODE load = ui.lpfnTop(&ui, C_BN, 1, 0, UI_AUTO);
        load.lpfnStyle(&load, WS_NONE, TEXT("Load"));
        load.lpfnCreate(&load);

        struct NODE preview = ui.lpfnTop(&ui, C_ST, 1, 0, UI_AUTO);
        preview.lpfnStyle(&preview, WS_NONE, TEXT("Preview"));
        preview.lpfnCreate(&preview);

        
        struct NODE set = ui.lpfnBottom(&ui, C_BN, 1, 0, UI_AUTO);
        set.lpfnStyle(&set, WS_NONE, TEXT("Set"));
        set.lpfnCreate(&set);

        struct NODE desc = ui.lpfnBottom(&ui, C_ST, 6, 1, UI_AUTO);
        desc.lpfnStyle(&desc, WS_VSCROLL, TEXT("Details, Desc"));
        desc.lpfnCreate(&desc);

        struct NODE details = ui.lpfnBottom(&ui, C_ST, 1, 0, UI_AUTO);
        details.lpfnStyle(&details, WS_NONE, TEXT("Details"));
        details.lpfnCreate(&details);

        return 0;
    }

    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;

    case WM_PAINT:
        return 0;

    case WM_COMMAND:
    {
        WORD hiword = HIWORD(wParam);
        switch (LOWORD(wParam))
        {
            /*
        case IDC_BT_BROWSE:
            break;
        case IDC_BT_LOAD:
            break;
        case IDC_BT_SET:
            break;
            */
        }
        return 0;
    }

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
}
