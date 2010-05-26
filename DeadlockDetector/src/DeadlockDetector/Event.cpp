#include "stdafx.h"

// static lock
CrstLock DDAutoEvent::m_DetectionLock;

//////////////////////////////////
// Deterministic Auto Event
//////////////////////////////////

// Standard functions

DDAutoEventDeterm::DDAutoEventDeterm() {
    m_cRef = 0;
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDAutoEventDeterm::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDAutoEventDeterm::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDAutoEventDeterm::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostAutoEvent) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// Deterministic monitor functions

HRESULT DDAutoEventDeterm::DeterministicWait(DWORD dwMilliseconds, DWORD option) {
	m_Lock.Enter();
	if (m_bSignaled) {
		m_bSignaled = false;
		m_Lock.Exit();
		return WAIT_OBJECT_0;
	} else {
		HANDLE hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
		m_Events.push_back(hEvent);
		m_Lock.Exit();
		return DDContext::HostWait(hEvent, dwMilliseconds, option);
	}
}

void DDAutoEventDeterm::DeterministicSet() {
	m_Lock.Enter();
	if (!m_bSignaled) {
		if (m_Events.empty()) {
			m_bSignaled = true;
			m_Lock.Exit();
		} else {
			HANDLE hEvent = m_Events.back();
			m_Events.pop_back();
			SetEvent(hEvent);
			CloseHandle(hEvent);
			m_Lock.Exit();
		}
	} else m_Lock.Exit();
}

STDMETHODIMP DDAutoEventDeterm::Set() {
	DeterministicSet();
    return S_OK;
}

STDMETHODIMP DDAutoEventDeterm::Wait(DWORD dwMilliseconds, DWORD option) {
    return DeterministicWait(dwMilliseconds, option);
}

//////////////////////////////////
// Auto Event
//////////////////////////////////

// Standard functions

DDAutoEvent::DDAutoEvent(SIZE_T cookie) {
    m_cRef = 0;
    m_hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
    if (!m_hEvent)
        FailWin32("Error creating auto event: %d");
    m_cookie = cookie;
    m_pContext = NULL;
}

DDAutoEvent::~DDAutoEvent() {
    if (m_hEvent) {
        CloseHandle(m_hEvent);
        m_hEvent = NULL;
    }
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDAutoEvent::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDAutoEvent::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDAutoEvent::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostAutoEvent) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostManualEvent functions

STDMETHODIMP DDAutoEvent::Set() {
    HRESULT hr = S_OK;

    if (!SetEvent(m_hEvent)) {
        _ASSERTE(!"SetEvent failed");
        hr = HRESULT_FROM_WIN32(GetLastError());
    }

    return hr;
}

STDMETHODIMP DDAutoEvent::Wait(DWORD dwMilliseconds, DWORD option) {
    if (!m_pContext || option & WAIT_NOTINDEADLOCK) {
        return DDContext::HostWait(m_hEvent, dwMilliseconds, option);
    } else {
		HRESULT hr;

		m_DetectionLock.Enter();
		DeadlockDetails *pDetails = m_pContext->TryEnter(m_cookie);

		if (pDetails) {
			// If the return value was non-null, we encountered a deadlock
			m_pContext->EndEnter(m_cookie);
			m_DetectionLock.Exit();

			// Get some info
			m_pContext->PrintDeadlockInfo(pDetails, m_cookie);
			return HOST_E_DEADLOCK;
		} else {
			m_DetectionLock.Exit();

			hr = DDContext::HostWait(m_hEvent, dwMilliseconds, option);
			m_pContext->EndEnter(m_cookie);
			return hr;
		}
    }
}

//////////////////////////////////
// Manual Event
//////////////////////////////////

// Standard functions

DDManualEvent::DDManualEvent(BOOL bInitialState) {
    m_cRef = 0;
    m_hEvent = CreateEvent(NULL, TRUE, bInitialState, NULL);
    if (!m_hEvent)
        FailWin32("Error creating manual event: %d");
}

DDManualEvent::~DDManualEvent() {
    if (m_hEvent) {
        CloseHandle(m_hEvent);
        m_hEvent = NULL;
    }
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDManualEvent::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDManualEvent::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDManualEvent::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostManualEvent) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostManualEvent functions

STDMETHODIMP DDManualEvent::Set() {
    if (!SetEvent(m_hEvent)) {
        _ASSERTE(!"SetEvent failed");
        return HRESULT_FROM_WIN32(GetLastError());
    }

    return S_OK;
}

STDMETHODIMP DDManualEvent::Reset() {
    if (!ResetEvent(m_hEvent)) {
        _ASSERTE(!"ResetEvent failed");
        return HRESULT_FROM_WIN32(GetLastError());
    }

    return S_OK;
}

STDMETHODIMP DDManualEvent::Wait(DWORD dwMilliseconds, DWORD option) {
    return DDContext::HostWait(m_hEvent, dwMilliseconds, option);
}