// Host.cpp : Simple CLR host
//

#include "stdafx.h"

ICLRRuntimeHost *pClrHost = NULL;
ICLRControl *pCLRControl = NULL;
FLockClrVersionCallback beginInit, endInit;

STDAPI init() {
	// Notify the shim to grant the exclusive initialization right
	beginInit();

	// Loading CLR to the current OS process. Once loaded, it's impossible to unload CLR
	// without restarting the process
	HRESULT hr = CorBindToRuntimeEx(
		L"v2.0.50727",   // Load the specified CLR version
		L"svr",
		STARTUP_CONCURRENT_GC | STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN,	//STARTUP_CONCURRENT_GC, STARTUP_SERVER_GC,
		CLSID_CLRRuntimeHost,
		IID_ICLRRuntimeHost,
		(PVOID*) &pClrHost);

	if (FAILED(hr)) {
		fprintf(stderr, "Bind to runtime failed (0x%x)", hr);
		exit(-1);
	}
	
	// Retrieve ICLRControl before calling ICLRRuntimeHost::Start
	hr = pClrHost->GetCLRControl(&pCLRControl);    
	if (FAILED(hr)) {
		fprintf(stderr, "Failed to load CLR Control (0x%x)", hr);
        exit(-1);
	}

	pClrHost->Start();

	endInit();

	return S_OK;
}

int _tmain(int argc, _TCHAR* argv[]) {
	HRESULT hr;
	DWORD retVal, strLen;
	ICLRGCManager *pCLRGCManager = NULL;
	WCHAR verstr[16];
	WCHAR asmverstr[64];
	WCHAR instdirstr[255];

	LockClrVersion(init, &beginInit, &endInit);

	// Get information about CLR version being loaded
	GetCORVersion(verstr, 16, &strLen);
	GetCORSystemDirectory(instdirstr, 255, &strLen);
	wprintf(L"Loaded CLR %s from %s\n", verstr, instdirstr);

	hr = GetFileVersion(L"ManagedApp.exe", asmverstr, 64, &strLen);
	if (SUCCEEDED(hr)) {
		wprintf(L"Assembly's target runtime: %s\n", asmverstr, strLen);
	}

	hr = pCLRControl->GetCLRManager(IID_ICLRGCManager, (PVOID *)&pCLRGCManager);    
	if (FAILED(hr)) {
		fprintf(stderr, "Failed to load GC Manager (0x%x)", hr);
        exit(-1);
	}

	// Perform GC in generation 0
	pCLRGCManager->Collect(0);

	// Executing managed code
	hr = pClrHost->ExecuteInDefaultAppDomain(L"ManagedApp.exe", L"ManagedApp.Program", L"_Main", L"", &retVal);
	if (FAILED(hr)) {
		fprintf(stderr, "Error while executing method (0x%x)", hr);
        exit(-1);
	}
	printf("RetVal=%d\n\n", retVal);

	COR_GC_STATS stats;
	stats.Flags = COR_GC_COUNTS | COR_GC_MEMORYUSAGE;
	pCLRGCManager->GetStats(&stats);
	
	printf("CommittedKBytes: %d\n", stats.CommittedKBytes);
	printf("ExplicitGCCount: %d\n", stats.ExplicitGCCount);
	printf("Gen0HeapSizeKBytes: %d\n", stats.Gen0HeapSizeKBytes);
	printf("Gen1HeapSizeKBytes: %d\n", stats.Gen1HeapSizeKBytes);
	printf("Gen2HeapSizeKBytes: %d\n", stats.Gen2HeapSizeKBytes);
	printf("LargeObjectHeapSizeKBytes: %d\n", stats.LargeObjectHeapSizeKBytes);
	printf("ReservedKBytes: %d\n", stats.ReservedKBytes);

	pCLRControl->Release();
	pCLRGCManager->Release();
	pClrHost->Stop();
	pClrHost->Release();

	return 0;
}

