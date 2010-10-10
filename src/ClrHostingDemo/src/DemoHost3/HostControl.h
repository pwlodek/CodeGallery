#include "stdafx.h"

class DDContext;
class DDTaskManager;

class DDHostControl : public IHostControl {
private:
	volatile LONG m_cRef;
    DDContext *m_pContext;
    DDTaskManager *m_pTaskManager;

public:
	DDHostControl();
    ~DDHostControl();

	DDContext* GetContext() { return m_pContext; }

	// IUnknown functions
	STDMETHODIMP_(DWORD) AddRef();
	STDMETHODIMP_(DWORD) Release();
	STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

	// IHostControl functions
	STDMETHODIMP GetHostManager(const IID &riid, void **ppObject);
	STDMETHODIMP SetAppDomainManager(DWORD dwAppDomainID, IUnknown *pUnkAppDomainManager);
};