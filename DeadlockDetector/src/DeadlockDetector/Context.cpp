#include "stdafx.h"

DDContext::DDContext() {
    m_pTaskManager = NULL;
	m_pSyncManager = NULL;
	m_pThreads = new list<ThreadInfo>();
	m_pLockWaits = new map<IHostTask*, SIZE_T>();
	m_pDeadlockDetails = new list<DeadlockDetails>();
}

DDContext::~DDContext() {
    if (m_pTaskManager)
		m_pTaskManager->Release();

	if (m_pSyncManager)
		m_pSyncManager->Release();

	delete m_pThreads;
	delete m_pLockWaits;
	delete m_pDeadlockDetails;
}

void DDContext::SetTaskManager(DDTaskManager *pTaskManager) {
    pTaskManager->AddRef();
    m_pTaskManager = pTaskManager;
}

void DDContext::SetSyncManager(DDSyncManager *pSyncManager) {
    pSyncManager->AddRef();
    m_pSyncManager = pSyncManager;
}

HRESULT DDContext::HostWait(HANDLE hWait, DWORD dwMilliseconds, DWORD dwOption) {
    DWORD dwWaitResult;
    BOOL bAlertable = dwOption & WAIT_ALERTABLE;

    if (dwOption & WAIT_MSGPUMP) {
		DWORD dwFlags = bAlertable ? COWAIT_ALERTABLE : 0;

		// If the caller resides in a single-thread apartment, CoWaitForMultipleHandles enters the COM
		// modal loop, and the thread's message loop will continue to dispatch messages using the
		// thread's message filter. If no message filter is registered for the thread, the default COM
		// message processing is used.
        dwWaitResult = CoWaitForMultipleHandles(dwFlags, dwMilliseconds, 1, &hWait, NULL);
    } else {
        dwWaitResult = WaitForSingleObjectEx(hWait, dwMilliseconds, bAlertable);
    }

    switch (dwWaitResult) {
        case WAIT_OBJECT_0:
            return S_OK;

        case WAIT_ABANDONED:
            return HOST_E_ABANDONED;

        case WAIT_IO_COMPLETION:
            return HOST_E_INTERRUPTED;

        case WAIT_TIMEOUT:
            return HOST_E_TIMEOUT;

        case WAIT_FAILED:
            return E_FAIL;

        default:
            return E_FAIL;
    }
}

void DDContext::PrintThreadInfo() {
	list<ThreadInfo>::iterator iter = m_pThreads->begin();
	
	printf("------------------------------\n");
    printf("Dumping threads information\n");
    printf("------------------------------\n");

	while (iter != m_pThreads->end()) {
		ThreadInfo tnfo = static_cast<ThreadInfo>(*iter);
		printf("Thread %d\t%x\n", tnfo.threadId, tnfo.threadHandle);
		iter++;
	}
}

void DDContext::PrintDeadlockInfo(DeadlockDetails *pDetails, SIZE_T cookie) {
    IHostTask *pTask;
	m_pTaskManager->GetCurrentTask((IHostTask**)&pTask);
	ICLRSyncManager *pClrSyncManager = m_pSyncManager->GetCLRSyncManager();

    fprintf(stderr, "A deadlock has occurred, Your program may stop working properly. Details below.\n");
    fprintf(stderr, "  %d (%x) was attempting to acquire %x, which created the cycle\n",
        dynamic_cast<DDTask*>(pTask)->GetThreadId(), dynamic_cast<DDTask*>(pTask)->GetThreadHandle(), cookie);
    pTask->Release();

    // Walk through the wait-graph and print details
    for (DeadlockDetails::iterator walker = pDetails->begin(); walker != pDetails->end(); walker++) {
        IHostTask *pOwner;
        pClrSyncManager->GetMonitorOwner(walker->second, (IHostTask**)&pOwner);        

        fprintf(stderr, "  %d (%x) waits on lock %x (owned by %d (%x))\n",
            walker->first->GetThreadId(), walker->first->GetThreadHandle(), walker->second,
			dynamic_cast<DDTask*>(pOwner)->GetThreadId(),
            dynamic_cast<DDTask*>(pOwner)->GetThreadHandle());

        pOwner->Release();
        walker->first->Release();
    }
}

DeadlockDetails* DDContext::TryEnter(SIZE_T cookie) {
    IHostTask* pCurrentTask;
    m_pTaskManager->GetCurrentTask((IHostTask**)&pCurrentTask);
    
    // Thread is entering a wait for the target lock
	m_CrstLock.Enter();
    m_pLockWaits->insert(map<IHostTask*, SIZE_T>::value_type(pCurrentTask, cookie));
	m_CrstLock.Exit();

    return DetectDeadlock(cookie);
}

void DDContext::EndEnter(SIZE_T cookie) {
    IHostTask* pCurrentTask;
    m_pTaskManager->GetCurrentTask((IHostTask**)&pCurrentTask);

    // Thread has acquired the lock. Remove wait record to indicate that the thread
	// is no longer waiting for the target lock
    m_CrstLock.Enter();
    m_pLockWaits->erase(pCurrentTask);
    m_CrstLock.Exit();

    // Release the underlying IHostTask twice
    pCurrentTask->Release();
    pCurrentTask->Release();
}

// This method detects whether a calling thread that wants to acquire the lock (cookie)
// caused a deadlock.
DeadlockDetails* DDContext::DetectDeadlock(SIZE_T cookie) {
	DeadlockDetails *pCycle = NULL;
    ICLRSyncManager *pClrSyncManager = m_pSyncManager->GetCLRSyncManager();

    IHostTask* pOwner;
    m_pTaskManager->GetCurrentTask((IHostTask**)&pOwner);

    vector<IHostTask*> waitGraph;
    waitGraph.push_back(pOwner);

    SIZE_T current = cookie;
    while (TRUE) {
        // If the lock is already owned, this will retrieve its owner:
        pClrSyncManager->GetMonitorOwner(current, (IHostTask**)&pOwner);
        if (!pOwner)
            break;

        // The lock is owned already. Check the graph for a cycle. If we've ever seen
        // this owner in our search, we are in a deadlock.
        BOOL bCycleFound = FALSE;
        vector<IHostTask*>::iterator walker = waitGraph.begin();
        while (walker != waitGraph.end()) {
            if (*walker == pOwner) {
                // We have detected a cycle; from here until the end of the list.
                bCycleFound = TRUE;
                break;
            }

            walker++;
        }

        if (bCycleFound) {
            // Construct a list of the cycle information. This is just a list of IHostTask, SIZE_T
            // pairs, listed in order of the wait graph.
            pCycle = new DeadlockDetails();
			m_CrstLock.Enter();
            while (walker != waitGraph.end()) {
                map<IHostTask*, SIZE_T>::iterator waitMatch = m_pLockWaits->find(*walker);
                waitMatch->first->AddRef();
                pCycle->insert(DeadlockDetails::value_type(
                    dynamic_cast<DDTask*>(waitMatch->first), waitMatch->second));
                walker++;
            }
			m_CrstLock.Exit();

			break;
        }

        waitGraph.push_back(pOwner);

        // No cycle was found. Try to determine whether the owner is likewise waiting on
        // somebody. If they are, we continue our deadlock-detection algorithm.
		m_CrstLock.Enter();
        map<IHostTask*, SIZE_T>::iterator waitMatch = m_pLockWaits->find(pOwner);
		if (waitMatch == m_pLockWaits->end()) {
			m_CrstLock.Exit();
            break;
		}
        current = waitMatch->second;
		m_CrstLock.Exit();
    }

    // IHostTasks are AddRef'd when call GetMonitorOwner so they must be
    // released before continuing
    for (vector<IHostTask*>::iterator walker = waitGraph.begin(); walker != waitGraph.end(); walker++) {
        (*walker)->Release();
    }

    pClrSyncManager->Release();

    return pCycle;
}