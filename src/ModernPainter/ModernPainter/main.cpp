#include "framework.h"
#include "Application.h"
#include "ErrorDialog.h"


INT APIENTRY _tWinMain(_In_ HINSTANCE hInst, _In_opt_ HINSTANCE, _In_ LPTSTR, _In_ INT)
{
	HeapSetInformation(NULL, HeapEnableTerminationOnCorruption, NULL, 0);
	if (SUCCEEDED(CoInitializeEx(NULL, COINIT_MULTITHREADED)))
	{
		{
			Application* app = new Application(hInst);
			HRESULT hr = app->Initialize();
			if (SUCCEEDED(hr))
				app->RunMessageLoop();
			DialogWhenError(hr);
			delete app;
		}
		CoUninitialize();
	}
	return 0;
}
