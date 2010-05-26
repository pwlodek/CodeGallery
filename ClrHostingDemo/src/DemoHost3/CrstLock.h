#include "stdafx.h"

class CrstLock {
private:
    BOOL m_bTaken;
    CRITICAL_SECTION m_Crst;

public:
    CrstLock() {        
        m_bTaken = FALSE;
		InitializeCriticalSection(&m_Crst);        
    };

    ~CrstLock() {
        if (m_bTaken) {
            LeaveCriticalSection(&m_Crst);
        }
        DeleteCriticalSection(&m_Crst);
    }

	void Enter() {
		EnterCriticalSection(&m_Crst);
        m_bTaken = TRUE;
	}

    void Exit() {
        LeaveCriticalSection(&m_Crst);
        m_bTaken = FALSE;
    }
};