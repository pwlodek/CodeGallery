#include "stdafx.h"

DDSyncManager::DDSyncManager(DDContext *pContext) {
    m_cRef = 0;
    m_pCLRSyncManager = NULL;
    m_pContext = pContext;
}

DDSyncManager::~DDSyncManager() {
    m_pCLRSyncManager->Release();
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDSyncManager::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDSyncManager::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0) {
        delete this;
    }
	return cRef;
}

STDMETHODIMP DDSyncManager::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostSyncManager) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostSyncManager functions

// This function is called by the runtime
STDMETHODIMP DDSyncManager::SetCLRSyncManager(/* in */ ICLRSyncManager *pManager) {
	m_pCLRSyncManager = pManager;
	return S_OK;
}

STDMETHODIMP DDSyncManager::CreateCrst(/* out */ IHostCrst **ppCrst) {
	IHostCrst* pCrst = new DDCrst;
	if (!pCrst) {
        _ASSERTE(!"Failed to allocate a new DDCrst");
        *ppCrst = NULL;
		return E_OUTOFMEMORY;
    }

    pCrst->QueryInterface(IID_IHostCrst, (void**)ppCrst);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateCrstWithSpinCount(/* in */ DWORD dwSpinCount, /* out */ IHostCrst **ppCrst) {
    IHostCrst* pCrst = new DDCrst(dwSpinCount);
    if (!pCrst) {
        _ASSERTE(!"Failed to allocate a new DDCrst");
        *ppCrst = NULL;
        return E_OUTOFMEMORY;
    }

    pCrst->QueryInterface(IID_IHostCrst, (void**)ppCrst);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateAutoEvent(/* out */IHostAutoEvent **ppEvent) {
    DDAutoEvent* pEvent = new DDAutoEvent(-1);
    if (!pEvent) {
        _ASSERTE(!"Failed to allocate a new AutoEvent");
        *ppEvent = NULL;
        return E_OUTOFMEMORY;
    }

    pEvent->QueryInterface(IID_IHostAutoEvent, (void**)ppEvent);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateManualEvent(/* in */ BOOL bInitialState, /* out */ IHostManualEvent **ppEvent) {
    DDManualEvent* pEvent = new DDManualEvent(bInitialState);
    if (!pEvent) {
        _ASSERTE(!"Failed to allocate a new ManualEvent");
        *ppEvent = NULL;
        return E_OUTOFMEMORY;
    }

    pEvent->QueryInterface(IID_IHostManualEvent, (void**)ppEvent);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateMonitorEvent(/* in */ SIZE_T Cookie, /* out */ IHostAutoEvent **ppEvent) {
    DDAutoEvent* pEvent = new DDAutoEvent(Cookie);
	//DDAutoEventDeterm *pEvent = new DDAutoEventDeterm();
    if (!pEvent) {
        _ASSERTE(!"Failed to allocate a new AutoEvent");
        *ppEvent = NULL;
        return E_OUTOFMEMORY;
    }

    pEvent->SetContext(m_pContext);
    pEvent->QueryInterface(IID_IHostAutoEvent, (void**)ppEvent);

    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateRWLockWriterEvent(/* in */ SIZE_T Cookie, /* out */ IHostAutoEvent **ppEvent) {
    DDAutoEvent* pEvent = new DDAutoEvent(-1);
    if (!pEvent) {
        _ASSERTE(!"Failed to allocate a new AutoEvent");
        *ppEvent = NULL;
        return E_OUTOFMEMORY;
    }

    pEvent->QueryInterface(IID_IHostAutoEvent, (void**)ppEvent);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateRWLockReaderEvent(/* in */ BOOL bInitialState, /* in */ SIZE_T Cookie, /* out */ IHostManualEvent **ppEvent) {
    DDAutoEvent* pEvent = new DDAutoEvent(-1);
    if (!pEvent) {
        _ASSERTE(!"Failed to allocate a new AutoEvent");
        *ppEvent = NULL;
        return E_OUTOFMEMORY;
    }

    pEvent->QueryInterface(IID_IHostAutoEvent, (void**)ppEvent);
    return S_OK;
}

STDMETHODIMP DDSyncManager::CreateSemaphore(/* in */ DWORD dwInitial, /* in */ DWORD dwMax, /* out */ IHostSemaphore **ppSemaphore) {
	*ppSemaphore = NULL;
	return E_NOTIMPL;
}