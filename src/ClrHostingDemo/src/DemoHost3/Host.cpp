#include "stdafx.h"

int _tmain(int argc, _TCHAR* argv[]) {
	DWORD retVal;
	ICLRRuntimeHost *pClrHost = NULL;
	
	HRESULT hr = CorBindToRuntimeEx(
		L"v2.0.50727",
		L"wks", // Workstation GC ("wks" or "svr" overrides)
		0, //STARTUP_SERVER_GC | STARTUP_CONCURRENT_GC,
		CLSID_CLRRuntimeHost,
		IID_ICLRRuntimeHost,
		(PVOID*) &pClrHost);
	
	CheckFail(hr, "Bind to runtime failed (0x%x)");

	DDHostControl *pHostControl = new DDHostControl();
	hr = pClrHost->SetHostControl(pHostControl);

	CheckFail(hr, "Error while setting host control (0x%x)");

	// Construct the shim path
	WCHAR shimPath[MAX_PATH];
    GetCurrentDirectoryW(MAX_PATH, shimPath);
    wcsncat_s(shimPath, sizeof(shimPath) / sizeof(WCHAR), L"\\DemoHost3.Shim.dll", MAX_PATH - wcslen(shimPath) - 1);

	// Gather the arguments to pass to the shim.
    LPWSTR shimArgs = NULL;
    if (argc > 1) {
        int totalLength = 1; // 1 is the NULL terminator
        for(int i = 1; i < argc; i++) {
            totalLength += _tcslen(argv[i]) + 1;
		}

        shimArgs = new WCHAR[totalLength];
        shimArgs[0] = '\0';
 
        for(int i = 1; i < argc; i++) {
            if (i != 1)
                wcscat_s(shimArgs, totalLength, L" ");
            wcsncat_s(shimArgs, totalLength, argv[i], wcslen(argv[i]));
		}
	}

	if (shimArgs == NULL)
		Fail("Error: Missing program path");

	hr = pClrHost->Start();

	CheckFail(hr, "Error while starting CLR (0x%x)");

	hr = pClrHost->ExecuteInDefaultAppDomain(shimPath, L"DemoHost3.Shim", L"Start", shimArgs, &retVal);

	CheckFail(hr, "Error while executing application (0x%x)", false);
	
	printf("\nExitCode = %d\n", retVal);
	pHostControl->GetContext()->PrintThreadInfo();

	if (shimArgs)
		delete [] shimArgs;
	pClrHost->Stop();
	pClrHost->Release();

	delete pHostControl;
	return 0;
}

// Helper methods

void Fail(const char *msg) {
    fprintf(stderr, msg);
    exit(-1);
}

void FailWin32(const char *msg) {
    fprintf(stderr, msg, GetLastError());
    exit(-1);
}

void CheckFail(HRESULT hr, const char *msg) {
    if (FAILED(hr)) {
        fprintf(stderr, msg, hr);
        exit(-1);
    }
}

void CheckFail(HRESULT hr, const char *msg, bool doExit) {
    if (FAILED(hr)) {
        fprintf(stderr, msg, hr);
		if (doExit) {
			exit(-1);
		}
    }
}