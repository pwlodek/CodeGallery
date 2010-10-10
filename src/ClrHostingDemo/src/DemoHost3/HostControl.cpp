#include "stdafx.h"

DDHostControl::DDHostControl() {
	m_cRef = 0;

    m_pContext = new DDContext();
    m_pTaskManager = new DDTaskManager(m_pContext);    
	
    if (!m_pContext || !m_pTaskManager) {
        Fail("Context or manager allocation failed");
    }

    m_pTaskManager->AddRef();
}

DDHostControl::~DDHostControl() {
    delete m_pContext;
    m_pTaskManager->Release();    
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDHostControl::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDHostControl::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDHostControl::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostControl) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostControl functions

STDMETHODIMP DDHostControl::GetHostManager(const IID &riid, void **ppvHostManager) {
	if (riid == IID_IHostTaskManager) {
        m_pTaskManager->QueryInterface(IID_IHostTaskManager, ppvHostManager);
		return S_OK;
	}
    
    ppvHostManager = NULL;
	return E_NOINTERFACE;
}

STDMETHODIMP DDHostControl::SetAppDomainManager(DWORD dwAppDomainID, IUnknown *pUnkAppDomainManager) {
    _ASSERTE(!"Not implemented");
	return E_NOTIMPL;
}