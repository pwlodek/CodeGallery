#include "stdafx.h"

DDTaskManager::DDTaskManager(DDContext *pContext) {
    m_cRef = 0;
    m_pCLRTaskManager = NULL;
    m_pContext = pContext;
    m_hSleepEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

    if (!m_hSleepEvent)
        Fail("Failed to create a new sleep event");

    // Instantiate maps and associated critical sections.
    m_pThreadMap = new map<DWORD, IHostTask*>();
	m_pThreadMapCrst = new CrstLock();    
}

DDTaskManager::~DDTaskManager() {	
    if (m_hSleepEvent) CloseHandle(m_hSleepEvent);
    if (m_pThreadMap) delete m_pThreadMap;    
    if (m_pCLRTaskManager) m_pCLRTaskManager->Release();
	if (m_pThreadMapCrst) delete m_pThreadMapCrst;	
}

// IUnknown functions

STDMETHODIMP_(DWORD) DDTaskManager::AddRef() {
    return InterlockedIncrement(&m_cRef);
}

STDMETHODIMP_(DWORD) DDTaskManager::Release() {
	ULONG cRef = InterlockedDecrement(&m_cRef);
	if (cRef == 0)
        delete this;
	return cRef;
}

STDMETHODIMP DDTaskManager::QueryInterface(const IID &riid, void **ppvObject) {
	if (riid == IID_IUnknown || riid == IID_IHostTaskManager) {
		*ppvObject = this;
		AddRef();
		return S_OK;
	}

	*ppvObject = NULL;
	return E_NOINTERFACE;
}

// IHostTaskManager functions

STDMETHODIMP DDTaskManager::GetCurrentTask(/* out */ IHostTask **pTask) {
    DWORD currentThreadId = GetCurrentThreadId();
	    
	m_pThreadMapCrst->Enter();
    map<DWORD, IHostTask*>::iterator match = m_pThreadMap->find(currentThreadId);
    if (match == m_pThreadMap->end()) {		
        // No match was found, create one for the currently executing thread.
        *pTask = new DDTask(this, GetCurrentThread(), currentThreadId);
        m_pThreadMap->insert(map<DWORD, IHostTask*>::value_type(currentThreadId, *pTask));

		ThreadInfo info;
		info.threadHandle = GetCurrentThread();
		info.threadId = currentThreadId;
		m_pContext->GetTasksList()->insert(m_pContext->GetTasksList()->begin(), list<ThreadInfo>::value_type(info));
    } else {
        *pTask = match->second;
    }
    (*pTask)->AddRef();
    m_pThreadMapCrst->Exit();

    return S_OK;
}

STDMETHODIMP DDTaskManager::CreateTask(/* in */ DWORD dwStackSize, /* in */ LPTHREAD_START_ROUTINE pStartAddress, /* in */ PVOID pParameter, /* out */ IHostTask **ppTask) {
    DWORD dwThreadId;
    HANDLE hThread = CreateThread(
        NULL,
        dwStackSize,
        pStartAddress,
        pParameter,
        CREATE_SUSPENDED | STACK_SIZE_PARAM_IS_A_RESERVATION,
        &dwThreadId);

    IHostTask* task = new DDTask(this, hThread, dwThreadId);
    if (!task) {
        _ASSERTE(!"Failed to allocate task");
        *ppTask = NULL;
        return E_OUTOFMEMORY;
    }

	m_pThreadMapCrst->Enter();    
	m_pThreadMap->insert(map<DWORD, IHostTask*>::value_type(dwThreadId, task));
	
	ThreadInfo info;
	info.threadHandle = hThread;
	info.threadId = dwThreadId;
	m_pContext->GetTasksList()->insert(m_pContext->GetTasksList()->begin(), list<ThreadInfo>::value_type(info));
	m_pThreadMapCrst->Exit();

    task->AddRef();
    *ppTask = task;

    return S_OK;
}

STDMETHODIMP DDTaskManager::Sleep(/* in */ DWORD dwMilliseconds, /* in */ DWORD option) {
    // TODO: recognize 'option'?
    SleepEx(dwMilliseconds, option & WAIT_ALERTABLE);
    return S_OK;
}

STDMETHODIMP DDTaskManager::SwitchToTask(/* in */ DWORD option) {
    //TODO: recognize 'option'?
    SwitchToThread();
    return S_OK;
}

STDMETHODIMP DDTaskManager::SetUILocale(/* in */ LCID lcid) {
    _ASSERTE(!"Not implementedUILOCALE");
    return E_NOTIMPL;
}

STDMETHODIMP DDTaskManager::SetLocale(/* in */ LCID lcid) {
    if (!SetThreadLocale(lcid)) {
        _ASSERTE(!"Couldn't set thread-locale");
        return HRESULT_FROM_WIN32(GetLastError());
    }

    return S_OK;
}

STDMETHODIMP DDTaskManager::CallNeedsHostHook(/* in */ SIZE_T target, /* out */ BOOL *pbCallNeedsHostHook) {
    *pbCallNeedsHostHook = FALSE;
    return S_OK;
}

STDMETHODIMP DDTaskManager::LeaveRuntime(/* in */ SIZE_T target) {
    // No need to perform any processing.
    return S_OK;
}

STDMETHODIMP DDTaskManager::EnterRuntime() {
    // No need to perform any processing.
    return S_OK;
}

STDMETHODIMP DDTaskManager::ReverseLeaveRuntime() {
    // No need to perform any processing.
    return S_OK;
}

STDMETHODIMP DDTaskManager::ReverseEnterRuntime() {
    // No need to perform any processing.
    return S_OK;
}

STDMETHODIMP DDTaskManager::BeginDelayAbort() {
    // We don't use aborts in this host; no-op.
    return S_OK;
}

STDMETHODIMP DDTaskManager::EndDelayAbort() {
    // We don't use aborts in this host; no-op.
    return S_OK;
}

STDMETHODIMP DDTaskManager::BeginThreadAffinity() {
    // We don't move tasks in this host; no-op.
    return S_OK;
}

STDMETHODIMP DDTaskManager::EndThreadAffinity() {
    // We don't move tasks in this host; no-op.
    return S_OK;
}

STDMETHODIMP DDTaskManager::SetStackGuarantee(/* in */ ULONG guarantee) {
    //_ASSERTE(!"Not implemented");
    return S_OK;
}

STDMETHODIMP DDTaskManager::GetStackGuarantee(/* out */ ULONG *pGuarantee) {
    //_ASSERTE(!"Not implemented");
    return S_OK;
}

STDMETHODIMP DDTaskManager::SetCLRTaskManager(/* in */ ICLRTaskManager *pManager) {
    m_pCLRTaskManager = pManager;
    return S_OK;
}
