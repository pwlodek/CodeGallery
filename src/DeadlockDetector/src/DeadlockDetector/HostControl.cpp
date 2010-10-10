#include "stdafx.h"

DDHostControl::DDHostControl(ICLRRuntimeHost *pRuntimeHost) {
	m_cRef = 0;
	m_pRuntimeHost = pRuntimeHost;
    m_pRuntimeHost->AddRef();    

    m_pContext = new DDContext();
    m_pTaskManager = new DDTaskManager(m_pContext);
	m_pSyncManager = new DDSyncManager(m_pContext);
	
    if (!m_pContext || !m_pTaskManager || !m_pSyncManager) {
        Fail("Context or managers allocation failed");
    }

    m_pTaskManager->AddRef();
	m_pSyncManager->AddRef();
    m_pContext->SetTaskManager(m_pTaskManager);
	m_pContext->SetSyncManager(m_pSyncManager);

	HRESULT hr = m_pRuntimeHost->GetCLRControl(&m_pCLRControl);
	CheckFail(hr, "Unable to get CLRControl (0x%x)");

	hr = m_pCLRControl->GetCLRManager(IID_ICLRPolicyManager, (PVOID*)&m_pCLRPolicyManager);
	CheckFail(hr, "Unable to get CLRPolicyManager (0x%x)");
}

DDHostControl::~DDHostControl() {
    delete m_pContext;
    m_pRuntimeHost->Release();
    m_pTaskManager->Release();
	m_pSyncManager->Release();
	m_pCLRControl->Release();
	m_pCLRPolicyManager->Release();
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
    else if (riid == IID_IHostSyncManager) {
        m_pSyncManager->QueryInterface(IID_IHostSyncManager, ppvHostManager);
		return S_OK;
	}
    
    ppvHostManager = NULL;
	return E_NOINTERFACE;
}

STDMETHODIMP DDHostControl::SetAppDomainManager(DWORD dwAppDomainID, IUnknown *pUnkAppDomainManager) {
	return E_NOTIMPL;
}