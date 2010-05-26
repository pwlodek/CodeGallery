#include "stdafx.h"

class DDContext;
class DDTaskManager;
class DDSyncManager;

class DDHostControl : public IHostControl {
private:
	volatile LONG m_cRef;
	ICLRRuntimeHost *m_pRuntimeHost;
	ICLRControl *m_pCLRControl;
	ICLRPolicyManager *m_pCLRPolicyManager;
    DDContext *m_pContext;
    DDTaskManager *m_pTaskManager;
	DDSyncManager *m_pSyncManager;

public:
	DDHostControl(ICLRRuntimeHost *pRuntimeHost);
    ~DDHostControl();

	DDContext* GetContext() {
		return m_pContext;
	}

	HRESULT SetHostPolicy() {
		return m_pCLRPolicyManager->SetUnhandledExceptionPolicy(eHostDeterminedPolicy);
	}

	// IUnknown functions
	STDMETHODIMP_(DWORD) AddRef();
	STDMETHODIMP_(DWORD) Release();
	STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

	// IHostControl functions
	STDMETHODIMP GetHostManager(const IID &riid, void **ppObject);
	STDMETHODIMP SetAppDomainManager(DWORD dwAppDomainID, IUnknown *pUnkAppDomainManager);
};