#pragma once

#include "framework.h"
#include "Palette.h"

struct Theme
{
	D2D1::ColorF Accent0		= { 0.00f, 0.47f, 0.83f };
	D2D1::ColorF Accent1		= { 0.60f, 0.92f, 1.00f };

	D2D1::ColorF Text			= { 1.00f, 1.00f, 1.00f };
	D2D1::ColorF GrayText		= { 0.80f, 0.80f, 0.80f };

	D2D1::ColorF Background0_0	= { 0.13f, 0.13f, 0.13f };
	D2D1::ColorF Background0_1	= { 0.18f, 0.18f, 0.18f };
	D2D1::ColorF Background0_2	= { 0.16f, 0.16f, 0.16f };

	D2D1::ColorF Background1_0	= { 0.17f, 0.17f, 0.17f };
	D2D1::ColorF Background1_1	= { 0.20f, 0.20f, 0.20f };
	D2D1::ColorF Background1_2	= { 0.15f, 0.15f, 0.15f };

	ID2D1SolidColorBrush* Accent0Brush;
	ID2D1SolidColorBrush* Accent1Brush;

	ID2D1SolidColorBrush* TextBrush;
	ID2D1SolidColorBrush* GrayTextBrush;

	ID2D1SolidColorBrush* Background0_0Brush;
	ID2D1SolidColorBrush* Background0_1Brush;
	ID2D1SolidColorBrush* Background0_2Brush;

	ID2D1SolidColorBrush* Background1_0Brush;
	ID2D1SolidColorBrush* Background1_1Brush;
	ID2D1SolidColorBrush* Background1_2Brush;

	HRESULT Update(Palette* palette);
};

