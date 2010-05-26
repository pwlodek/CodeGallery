// DeadlockDetector.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

int _tmain(int argc, _TCHAR* argv[]) {
	HRESULT hr;
	DWORD retVal;
	ICLRRuntimeHost *pClrHost = NULL;
	
	hr = CorBindToRuntimeEx(
		L"v2.0.50727",   // Load the latest CLR version available
		L"wks", // Workstation GC ("wks" or "svr" overrides)
		0, //STARTUP_SERVER_GC | STARTUP_CONCURRENT_GC,
		CLSID_CLRRuntimeHost,
		IID_ICLRRuntimeHost,
		(PVOID*) &pClrHost);
	
	CheckFail(hr, "Bind to runtime failed (0x%x)");

	DDHostControl *pHostControl = new DDHostControl(pClrHost);
	hr = pHostControl->SetHostPolicy();
	CheckFail(hr, "Error while setting host policy (0x%x)");

	hr = pClrHost->SetHostControl(pHostControl);
	CheckFail(hr, "Error while setting host control object (0x%x)");

	// Construct the shim path
	WCHAR shimPath[MAX_PATH];
    GetCurrentDirectoryW(MAX_PATH, shimPath);
    wcsncat_s(shimPath, sizeof(shimPath) / sizeof(WCHAR), L"\\DeadlockDetector.Shim.dll", MAX_PATH - wcslen(shimPath) - 1);

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

	printf("Deadlock Detector version 1.0.0.0\n");
	printf("Host's thread %d (%x)\n", GetCurrentThreadId(), GetCurrentThread());

	if (shimArgs == NULL)
		Fail("Error: Missing program path");

	printf("\n********** Executing profiled application **********\n");

	pClrHost->Start();
	hr = pClrHost->ExecuteInDefaultAppDomain(shimPath, L"DeadlockDetector.Shim", L"Start", shimArgs, &retVal);	
	CheckFail(hr, "Error while executing application (0x%x)", false);
	
	printf("\nExitCode = %d\n", retVal);
	printf("********** Finished **********\n\n");	

	if (shimArgs)
		delete [] shimArgs;
	pClrHost->Stop();
	pClrHost->Release();

	pHostControl->GetContext()->PrintThreadInfo();

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

void CheckFail(HRESULT hr, const char *msg, bool shutdown) {
    if (FAILED(hr)) {
        fprintf(stderr, msg, hr);
		if (shutdown) {
			exit(-1);
		}
    }
}