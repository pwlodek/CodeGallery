#include "stdafx.h"
#include <list>

using namespace std;

struct ThreadInfo {
	HANDLE threadHandle;
	int threadId;
};

class DDContext {
private:
	list<ThreadInfo> *m_pThreads;

public:
    DDContext();
	~DDContext();
    
	// DDContext methods
	list<ThreadInfo>* GetTasksList() { return m_pThreads; }
	void PrintThreadInfo();
};