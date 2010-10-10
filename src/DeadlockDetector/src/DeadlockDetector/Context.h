#include "stdafx.h"
#include <map>
#include <list>
#include <vector>

using namespace std;

class DDTask;
class DDTaskManager;
class DDSyncManager;

typedef map<DDTask*, SIZE_T> DeadlockDetails;

struct ThreadInfo {
	HANDLE threadHandle;
	int threadId;
};

class DDContext {
private:
    DDTaskManager *m_pTaskManager;
	DDSyncManager *m_pSyncManager;
	list<ThreadInfo> *m_pThreads;
	map<IHostTask*, SIZE_T> *m_pLockWaits;
	list<DeadlockDetails> *m_pDeadlockDetails;
	CrstLock m_CrstLock;

public:
    DDContext();
    ~DDContext();
    
	// DDContext methods
	
	list<ThreadInfo>* GetTasksList() {
		return m_pThreads;
	}

    static HRESULT HostWait(HANDLE hWait, DWORD dwMilliseconds, DWORD dwOption);    
    void SetTaskManager(DDTaskManager *pTaskManager);
	void SetSyncManager(DDSyncManager *pSyncManager);
	
	void PrintThreadInfo();
	void PrintDeadlockInfo(DeadlockDetails *pDetails, SIZE_T cookie);

	DeadlockDetails* TryEnter(SIZE_T cookie);
    void EndEnter(SIZE_T cookie);
    DeadlockDetails* DetectDeadlock(SIZE_T cookie);
};