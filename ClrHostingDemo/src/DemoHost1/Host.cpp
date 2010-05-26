// Host.cpp : Simple CLR host
//

#include "stdafx.h"

int _tmain(int argc, _TCHAR* argv[]) {
	HRESULT hr;
	DWORD retVal, strLen;
	ICLRRuntimeHost *pClrHost = NULL;
	WCHAR verstr[16];
	WCHAR asmverstr[64];
	WCHAR instdirstr[255];

	// Loading CLR to the current OS process. Once loaded, it's impossible to unload CLR
	// without restarting the process
	hr = CorBindToRuntimeEx(
		L"v2.0.50727",   // Load the specified CLR version
		L"wks", // L"svr"
		STARTUP_CONCURRENT_GC | STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN,	//STARTUP_CONCURRENT_GC, STARTUP_SERVER_GC,
		CLSID_CLRRuntimeHost,
		IID_ICLRRuntimeHost,
		(PVOID*) &pClrHost);

	if (FAILED(hr)) {
		fprintf(stderr, "Bind to runtime failed (0x%x)", hr);
        exit(-1);
	}

	// Starting runtime
	pClrHost->Start();

	// Get information about CLR version being loaded
	GetCORVersion(verstr, 16, &strLen);
	GetCORSystemDirectory(instdirstr, 255, &strLen);
	wprintf(L"Loaded CLR %s from %s\n", verstr, instdirstr);

	/*hr = GetFileVersion(L"ManagedApp.exe", asmverstr, 64, &strLen);
	if (SUCCEEDED(hr)) {
		wprintf(L"Assembly's target runtime: %s\n", asmverstr, strLen);
	}*/

	// Executing managed code
	hr = pClrHost->ExecuteInDefaultAppDomain(L"ManagedApp.exe", L"ManagedApp.Program", L"_Main", L"", &retVal);
	if (FAILED(hr)) {
		fprintf(stderr, "Error while executing method (0x%x)", hr);
        exit(-1);
	}
	printf("RetVal=%d\n", retVal);

	pClrHost->Stop();
	pClrHost->Release();

	return 0;
}

