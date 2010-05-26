#include "stdafx.h"

class CrstLock {
private:
    BOOL m_bTaken;
    LPCRITICAL_SECTION m_pCrst;

public:
    CrstLock() {        
        m_bTaken = FALSE;
		m_pCrst = new CRITICAL_SECTION;

		InitializeCriticalSection(m_pCrst);        
    };

    ~CrstLock() {
        if (m_bTaken) {
            LeaveCriticalSection(m_pCrst);
        }
        DeleteCriticalSection(m_pCrst);
    }

	void Enter() {
		EnterCriticalSection(m_pCrst);
        m_bTaken = TRUE;
	}

    void Exit() {
        LeaveCriticalSection(m_pCrst);
        m_bTaken = FALSE;
    }
};