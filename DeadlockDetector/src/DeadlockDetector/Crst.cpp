#include "stdafx.h"

// IUnknown functions
DDCrst::DDCrst() {
    m_cRef = 0;
    m_pCrst = new CRITICAL_SECTION;
	if (!m_pCrst)
		FailWin32("Failed to initialize CRST: %d");
    
	InitializeCriticalSection(m_pCrst);
}

DDCrst::DDCrst(DWORD dwSpinCount) {
    m_cRef = 0;
    m_pCrst = new CRITICAL_SECTION;
	if (!m_pCrst)
		FailWin32("Failed to initialize CRST: %d");

    if (!InitializeCriticalSectionAndSpinCount(m_pCrst, dwSpinCount)) {
        FailWin32("Failed to initialize CRST: %d");
    }
}

DDCrst::~DDCrst() {
    if (m_pCrst) {
        DeleteCriticalSection(m_pCrst);
        m_pCrst = NULL;
    }
}

STDMETHODIMP_(DWORD) DDCrst::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDCrst::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDCrst::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostCrst) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostCrst functions

STDMETHODIMP DDCrst::Enter(DWORD option) {
    _ASSERTE(m_pCrst && "Expected a non-null critical section here");

	/*m_CrstLock.Enter();
	BOOL succeeded = TryEnterCriticalSection(m_pCrst);
	while (!succeeded) {
		HANDLE hThread = GetCurrentThread();
		m_WaitingThreads.push_back(hThread);
		m_CrstLock.Exit();
		SuspendThread(hThread);
		m_CrstLock.Enter();
		succeeded = TryEnterCriticalSection(m_pCrst);
	}
	m_CrstLock.Exit();*/

	EnterCriticalSection(m_pCrst);

    return S_OK;
}

STDMETHODIMP DDCrst::Leave() {
    _ASSERTE(m_pCrst && "Expected a non-null critical section here");

	/*m_CrstLock.Enter();
	if (!m_WaitingThreads.empty()) {
		HANDLE hThread = m_WaitingThreads.back();
		m_WaitingThreads.pop_back();
		ResumeThread(hThread);
	}
	LeaveCriticalSection(m_pCrst);
	m_CrstLock.Exit();*/

	LeaveCriticalSection(m_pCrst);

    return S_OK;
}

STDMETHODIMP DDCrst::TryEnter(DWORD option, BOOL *pbSucceeded) {
    _ASSERTE(m_pCrst && "Expected a non-null critical section here");

    *pbSucceeded = TryEnterCriticalSection(m_pCrst);

    return S_OK;
}

STDMETHODIMP DDCrst::SetSpinCount(DWORD dwSpinCount) {
    _ASSERTE(m_pCrst && "Expected a non-null critical section here");

    SetCriticalSectionSpinCount(m_pCrst, dwSpinCount);

    return S_OK;
}