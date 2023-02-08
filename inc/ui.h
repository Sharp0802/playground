#ifndef LWMAN_UI_H
#define LWMAN_UI_H

#include "fw.h"

#define UI_AUTO 0

enum CLASS
{
    C_BN = 0,
    C_IN = 1,
    C_ST = 2
};

struct NODE
{
    HWND hWnd;
    HINSTANCE hInst;
    enum CLASS cls;
    int x;
    int y;
    int w;
    int h;
    int style;
    LPTSTR lpszContent;

    void (*lpfnStyle)(struct NODE *lpNode, int style, LPTSTR lpszContent);
    HWND (*lpfnCreate)(struct NODE *lpNode);
};

struct UI_STACK
{
    HWND hWnd;
    HINSTANCE hInst;

    int x;
    int y;
    int w;
    int h;
    int s;

    int t_x;
    int t_col;

    int b_x;
    int b_col;

    struct NODE (*lpfnTop)(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width);
    struct NODE (*lpfnBottom)(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width);
};

struct UI_STACK NewUIStack(HWND hWnd, int x, int y, int width, int height, int spacing);

#endif
