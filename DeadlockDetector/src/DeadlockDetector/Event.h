#include "stdafx.h"
#include <list>

class DDAutoEventDeterm : public IHostAutoEvent {
private:
	volatile LONG m_cRef;
	bool m_bSignaled;
	CrstLock m_Lock;
	list<HANDLE> m_Events;

public:
    DDAutoEventDeterm();

	// IUnknown functions
    STDMETHODIMP_(DWORD) AddRef();
    STDMETHODIMP_(DWORD) Release();
    STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

    // Detector method
    STDMETHODIMP Wait(DWORD dwMilliseconds, DWORD option);
	STDMETHODIMP Set();

	// Determinism
	HRESULT DeterministicWait(DWORD dwMilliseconds, DWORD option);
	void DeterministicSet();
};

class DDAutoEvent : public IHostAutoEvent {
private:
	volatile LONG m_cRef;
    HANDLE m_hEvent;
    SIZE_T m_cookie;
    DDContext *m_pContext;
	static CrstLock m_DetectionLock;

public:
    DDAutoEvent(SIZE_T cookie);
    ~DDAutoEvent();

	void SetContext(DDContext *pContext) {
		m_pContext = pContext;
	}

	// IUnknown functions
    STDMETHODIMP_(DWORD) AddRef();
    STDMETHODIMP_(DWORD) Release();
    STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

    // Detector method
    STDMETHODIMP Wait(DWORD dwMilliseconds, DWORD option);
	STDMETHODIMP Set();
};

class DDManualEvent : public IHostManualEvent {
private:
	volatile LONG m_cRef;
    HANDLE m_hEvent;

public:
    DDManualEvent(BOOL bInitialState);
    ~DDManualEvent();

	// IUnknown functions
    STDMETHODIMP_(DWORD) AddRef();
    STDMETHODIMP_(DWORD) Release();
    STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

    // IHostManualEvent functions
    STDMETHODIMP Wait(DWORD dwMilliseconds, DWORD option);
    STDMETHODIMP Reset();
    STDMETHODIMP Set();
};