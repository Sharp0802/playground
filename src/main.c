#include "fw.h"

#define CLASS_NAME "lwman"
#define APP_NAME "lwman"

#define IDC_BT_BROWSE (10001)
#define IDC_BT_SET    (10002)
#define IDC_IN_PATH   (10003)

#define MARGIN 10
#define SPACING 5
#define COL_HV 20
#define COL_H(c) (c * COL_HV + (c - 1) * SPACING + MARGIN)

void InitWindow(void);

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
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
        NULL,
        NULL,
        hInstance,
        NULL);
    if (hwnd == NULL)
        exit(-1);

    ShowWindow(hwnd, SW_SHOW);

    MSG msg;
    MEMSET0(msg);
    while (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE) > 0)
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}

void CreateChild(HWND hwnd, LPCTSTR lpszCls, int style, int column, int x, int width, size_t id, LPCTSTR lpszContent)
{
    CreateWindow(
        lpszCls, 
        lpszContent, 
        WS_CHILD | WS_VISIBLE | style, 
        x + MARGIN, COL_H(column), width, COL_HV, 
        hwnd, 
        (HMENU)id, 
        GetModuleHandle(NULL), 
        NULL);
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    switch (uMsg)
    {
    case WM_CREATE:
        {
            const int width = 400 - 2 * MARGIN;
            const int btW = 60;

            // browse button
            CreateChild(hwnd, TEXT("BUTTON"), 0, 0, width - btW, btW, IDC_BT_BROWSE, TEXT("Browse"));
            // set button
            CreateChild(hwnd, TEXT("BUTTON"), 0, 1, 0, width, IDC_BT_SET, TEXT("Set"));
            // path field
            CreateChild(hwnd, TEXT("EDIT"), WS_BORDER, 0, 0, width - (btW + SPACING), IDC_IN_PATH, TEXT("C:\\Users\\"));
        }  
        return 0;

    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;

    case WM_PAINT:
        return 0;

    case WM_COMMAND:
        {
            switch (LOWORD(wParam))
            {
                case IDC_BT_BROWSE:
                    break;
                case IDC_BT_SET:
                    break;
                case IDC_IN_PATH:
                    break;
            }
        }
        return 0;

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
}
