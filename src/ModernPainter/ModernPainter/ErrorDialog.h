#pragma once

#include "framework.h"


void DialogError(const TCHAR* title, const LPCTSTR text);

void DialogWhenError(const HRESULT hr, const LPCTSTR text);
