#include "ErrorDialog.h"

void DialogError(const TCHAR* title, const TCHAR* text)
{
	MessageBox(NULL, text, title, MB_ICONERROR);
}

void DialogWhenError(const HRESULT hr)
{
	if (SUCCEEDED(hr)) return;
	_com_error err(hr);
	DWORD id = GetLastError();
	LPTSTR buf = nullptr;
	if (id != 0)
	{
		FormatMessage(
			FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL,
			id,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			reinterpret_cast<LPTSTR>(&buf),
			0,
			NULL);
	}
	else
	{
		buf = const_cast<LPTSTR>(TEXT("GetLastError() returns 0. Unknown error."));
	}
	MessageBox(
		NULL,
		buf,
		err.ErrorMessage(),
		MB_ICONERROR | MB_TASKMODAL);
	LocalFree(buf);
}
