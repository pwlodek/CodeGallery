#include "stdafx.h"
#include <list>

class DDCrst : public IHostCrst {
private:
	volatile LONG m_cRef;
    CRITICAL_SECTION *m_pCrst;
	/*CrstLock m_CrstLock;
	list<HANDLE> m_WaitingThreads;*/

public:
    DDCrst();
	DDCrst(DWORD dwSpinCount);
	~DDCrst();

	// IUnknown functions
	STDMETHODIMP_(DWORD) AddRef();
	STDMETHODIMP_(DWORD) Release();
	STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

    // IHostCrst functions
    STDMETHODIMP Enter(DWORD option);
    STDMETHODIMP Leave();
    STDMETHODIMP TryEnter(DWORD option, BOOL *pbSucceeded);
    STDMETHODIMP SetSpinCount(DWORD dwSpinCount);
};