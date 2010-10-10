#include "stdafx.h"

class DDTask : public IHostTask {
private:
	volatile LONG m_cRef;
    HANDLE m_hThread;
    ICLRTask *m_pCLRTask;

public:
    DDTask(HANDLE hThread);
    ~DDTask();

	// IUnknown functions
	STDMETHODIMP_(DWORD) AddRef();
	STDMETHODIMP_(DWORD) Release();
	STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

    // IHostTask functions
    STDMETHODIMP Start();
    STDMETHODIMP Alert();
    STDMETHODIMP Join(/* in */ DWORD dwMilliseconds, /* in */ DWORD dwOption);
    STDMETHODIMP SetPriority(/* in */ int newPriority);
    STDMETHODIMP GetPriority(/* out */ int *pPriority);
    STDMETHODIMP SetCLRTask(/* in */ ICLRTask *pCLRTask);
};