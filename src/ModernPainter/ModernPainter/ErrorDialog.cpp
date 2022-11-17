#include "ErrorDialog.h"

void DialogError(const TCHAR* title, const LPCTSTR text)
{
	MessageBox(NULL, text, title, MB_ICONERROR);
}

void DialogWhenError(const HRESULT hr, const LPCTSTR msg)
{
	if (SUCCEEDED(hr)) return;
	_com_error err(hr);
	DWORD id = GetLastError();

	LPTSTR innerBuf = nullptr;
	if (id != 0)
	{
		FormatMessage(
			FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			NULL,
			id,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			reinterpret_cast<LPTSTR>(&innerBuf),
			0,
			NULL);
	}
	else
	{
		innerBuf = const_cast<LPTSTR>(TEXT("GetLastError() returns 0. Unknown error."));
	}
	size_t innerLen = lstrlen(innerBuf);
	size_t len = lstrlen(msg);

	LPTSTR buf = new TCHAR[innerLen + len + 1];
	buf[innerLen + len] = 0;
	memcpy(buf, innerBuf, sizeof(TCHAR) * innerLen);
	memcpy(buf + innerLen, msg, sizeof(TCHAR) * len);

	LocalFree(innerBuf);

	MessageBox(
		NULL,
		buf,
		err.ErrorMessage(),
		MB_ICONERROR | MB_TASKMODAL);

	delete[] buf;
}
