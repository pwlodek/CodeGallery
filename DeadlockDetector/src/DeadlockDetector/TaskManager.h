#include "stdafx.h"

class DDContext;

class DDTaskManager : public IHostTaskManager {
private:
	volatile LONG m_cRef;
    ICLRTaskManager *m_pCLRTaskManager;
    map<DWORD, IHostTask*> *m_pThreadMap;
	
    //CRITICAL_SECTION m_ThreadMapCrst;
	CrstLock *m_pThreadMapCrst;
    DDContext *m_pContext;
    HANDLE m_hSleepEvent;

public:
    DDTaskManager(DDContext *pContext);
    ~DDTaskManager();

	//list<IHostTask*> *m_pThreads;

	// IUnknown functions
	STDMETHODIMP_(DWORD) AddRef();
	STDMETHODIMP_(DWORD) Release();
	STDMETHODIMP QueryInterface(const IID &riid, void **ppvObject);

	// IHostTaskManager functions
    STDMETHODIMP GetCurrentTask(/* out */ IHostTask **pTask);
    STDMETHODIMP CreateTask(/* in */ DWORD dwStackSize, /* in */ LPTHREAD_START_ROUTINE pStartAddress, /* in */ PVOID pParameter, /* out */ IHostTask **ppTask);
    STDMETHODIMP Sleep(/* in */ DWORD dwMilliseconds, /* in */ DWORD option);
    STDMETHODIMP SwitchToTask(/* in */ DWORD option);
    STDMETHODIMP SetUILocale(/* in */ LCID lcid);
    STDMETHODIMP SetLocale(/* in */ LCID lcid);
    STDMETHODIMP CallNeedsHostHook(/* in */ SIZE_T target, /* out */ BOOL *pbCallNeedsHostHook);
    STDMETHODIMP LeaveRuntime(/* in */ SIZE_T target);
    STDMETHODIMP EnterRuntime();
    STDMETHODIMP ReverseEnterRuntime();
    STDMETHODIMP ReverseLeaveRuntime();
    STDMETHODIMP BeginDelayAbort();
    STDMETHODIMP EndDelayAbort();
    STDMETHODIMP BeginThreadAffinity();
    STDMETHODIMP EndThreadAffinity();
    STDMETHODIMP SetStackGuarantee(/* in */ ULONG guarantee);
    STDMETHODIMP GetStackGuarantee(/* out */ ULONG *pGuarantee);
    STDMETHODIMP SetCLRTaskManager(/* in */ ICLRTaskManager *pManager);
};