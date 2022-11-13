#pragma once

#include "framework.h"
#include "Palette.h"
#include "Panel.h"
#include "List.h"

class Application
{
public:
	Application(HINSTANCE hInst);
	~Application();

	HRESULT Initialize();
	void RunMessageLoop();

private:
	HINSTANCE _hInst;
	HWND _hWnd;

	ID2D1Factory* _d2dFactory;
	ID2D1HwndRenderTarget* _d2dRenderTarget;

	Palette* _palette;
	List<Panel*> _panels;

private:
	HRESULT CreateDeviceIndependentResources();
	HRESULT CreateDeviceResources();

	void DiscardDeviceResources();
	void InitializeComponents();
	HRESULT OnRender();

	void OnResize(UINT width, UINT height);

	static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
};
