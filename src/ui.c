#include "ui.h"

#define COL_HC 20
#define MAX_COL(height, spacing, cheight) ((height - spacing) / (cheight + spacing) - 1)
#define COL_H(span, spacing) (COL_HC * span + (span - 1) * spacing)
#define COL_Y(column, cheight, spacing) (column * (cheight + spacing))

#define WS_DEFAULT (WS_CHILD | WS_VISIBLE)

#define MOD_I 0
#define MOD_T 1
#define MOD_B 2

static LPCTSTR arrlpszCls[] = { TEXT("BUTTON"), TEXT("EDIT"), TEXT("STATIC") };

static size_t curID = 0;

size_t NewID();
struct NODE NewNode(enum CLASS cls, HWND hwnd, int x, int y, int w, int h, int style, LPTSTR lpszContent);
struct NODE STACK_Bottom(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width);
struct NODE STACK_Top(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width);
void NODE_Style(struct NODE *this, int style, LPTSTR lpszContent);
HWND NODE_Create(struct NODE *this);

size_t NewID()
{
    return 20000 + curID++;
}

struct UI_STACK NewUIStack(HWND hWnd, int x, int y, int width, int height, int spacing)
{
    struct UI_STACK stack;
    MEMSET0(stack);
    stack.hWnd = hWnd;
    stack.x = x;
    stack.y = y;
    stack.w = width;
    stack.h = height;
    stack.s = spacing;

    stack.t_x = 0;
    stack.t_col = 0;

    stack.b_x = 0;
    stack.b_col = MAX_COL(height, spacing, COL_HC);

    stack.lpfnTop = STACK_Top;
    stack.lpfnBottom = STACK_Bottom;

    return stack;
}

struct NODE NewNode(enum CLASS cls, HWND hwnd, int x, int y, int w, int h, int style, LPTSTR lpszContent)
{
    struct NODE node;
    MEMSET0(node);
    node.hWnd = hwnd;
    node.cls = cls;
    node.x = x;
    node.y = y;
    node.w = w;
    node.h = h;
    node.style = style;
    node.lpszContent = lpszContent;
    node.lpfnStyle = NODE_Style;
    node.lpfnCreate = NODE_Create;
    return node;
}

struct NODE STACK_Top(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width)
{
    if (offset != 0)
    {
        this->t_col += offset;
        this->t_x = 0;
    }
    else if (this->w < this->t_x + this->s + width)
    {
        this->t_x = 0;
        this->t_col++;
    }

    if (width == 0 /* UI_AUTO */)
    {
        if (this->t_x != 0)
            this->t_x += this->s;

        if (this->t_x == this->w)
        {
            width = this->w - this->s;
            this->t_x = 0;
            this->t_col++;
        }
        else
        {
            width = this->w - this->t_x - this->s;
        }
    }

    struct NODE ret = NewNode(cls, this->hWnd, this->t_x + this->x, COL_Y(this->t_col, COL_HC, this->s) + this->y, width, COL_H(span, this->s), WS_DEFAULT, TEXT(""));

    this->t_x += width;
    this->t_col += span - 1;

    return ret;
}

struct NODE STACK_Bottom(struct UI_STACK *this, enum CLASS cls, int span, int offset, int width)
{
    if (offset != 0)
    {
        this->b_col -= offset;
        this->b_x = 0;
    }

    if (this->w < this->b_x + this->s + width)
    {
        this->b_x = 0;
        this->b_col--;
    }

    if (width == 0 /* UI_AUTO */)
    {
        if (this->b_x != 0)
            this->b_x += this->s;

        if (this->b_x == this->w)
        {
            width = this->w - this->s;
            this->b_x = 0;
            this->b_col--;
        }
        else
        {
            width = this->w - this->b_x - this->s;
        }
    }
    
    this->b_col -= span - 1;

    struct NODE ret = NewNode(cls, this->hWnd, this->b_x + this->x, COL_Y(this->b_col, COL_HC, this->s) + this->y, width, COL_H(span, this->s), WS_DEFAULT, TEXT(""));

    this->b_x += width;

    return ret;
}

void NODE_Style(struct NODE *this, int style, LPTSTR lpszContent)
{
    this->style |= style;
    this->lpszContent = lpszContent;
}

HWND NODE_Create(struct NODE *this)
{
    struct NODE node = *this;
    LPTSTR lpszContent = node.lpszContent;
    LPCTSTR cls = arrlpszCls[node.cls];
    HINSTANCE hInst = GetModuleHandle(NULL);
    HMENU id = (HMENU)NewID();

    return CreateWindow(cls, lpszContent, node.style, node.x, node.y, node.w, node.h, node.hWnd, id, hInst, NULL);
}
