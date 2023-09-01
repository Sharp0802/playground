#pragma once

#include "framework.h"
#include "EventSource.h"
#include "Palette.h"
#include "Panel.h"
#include "List.h"
#include "Theme.h"

#define REF_APP_INST extern Application* ApplicationInstance


class Application
{
public:
	Application(HINSTANCE hInst);
	~Application();

	HRESULT Initialize();
	void RunMessageLoop();

public:
	EventSource<WPARAM, LPARAM> OnMouseEvnetRaised;

private:
	HINSTANCE _hInst;
	HWND _hWnd;

	ID2D1Factory* _d2dFactory;
	ID2D1HwndRenderTarget* _d2dRenderTarget;

	Theme* _theme;
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
