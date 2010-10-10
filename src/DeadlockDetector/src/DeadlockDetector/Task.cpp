#include "stdafx.h"

// Standard functions

DDTask::DDTask(DDTaskManager *pTaskManager, HANDLE hThread, DWORD dwThreadId) {
    m_cRef = 0;
    m_pTaskManager = pTaskManager;
    m_pTaskManager->AddRef();
    m_hThread = hThread;
	m_dwThreadId = dwThreadId;
    m_pCLRTask = NULL;
}

DDTask::~DDTask()
{
    //TODO: shutdown thread?
    if (m_pTaskManager)
		m_pTaskManager->Release();
    if (m_pCLRTask)
		m_pCLRTask->Release();
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDTask::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDTask::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDTask::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostTask) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostTask functions

STDMETHODIMP DDTask::Start() {
    if (!ResumeThread(m_hThread)) {
        _ASSERTE(!"Couldn't resume thread");
        return HRESULT_FROM_WIN32(GetLastError());
    }

    return S_OK;
}

STDMETHODIMP DDTask::Alert() {
    return S_OK;
}

STDMETHODIMP DDTask::Join(/* in */ DWORD dwMilliseconds, /* in */ DWORD dwOption) {
    return DDContext::HostWait(m_hThread, dwMilliseconds, dwOption);
}

STDMETHODIMP DDTask::SetPriority(/* in */ int newPriority) {
    if (!SetThreadPriority(m_hThread, newPriority)) {
        _ASSERTE(!"Couldn't set thread-priority");
        return HRESULT_FROM_WIN32(GetLastError());
    }

    return S_OK;
}

STDMETHODIMP DDTask::GetPriority(/* out */ int *pPriority) {
    *pPriority = GetThreadPriority(m_hThread);
    return S_OK;
}

STDMETHODIMP DDTask::SetCLRTask(/* in */ ICLRTask *pCLRTask) {
    m_pCLRTask = pCLRTask;
    return S_OK;
}
