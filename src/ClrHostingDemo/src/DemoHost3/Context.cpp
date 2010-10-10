#include "stdafx.h"

DDContext::DDContext() {
	m_pThreads = new list<ThreadInfo>();
}

DDContext::~DDContext() {
	delete m_pThreads;
}

void DDContext::PrintThreadInfo() {
	list<ThreadInfo>::iterator iter = m_pThreads->begin();
	
	printf("------------------------------\n");
    printf("Dumping threads information\n");
    printf("------------------------------\n");

	while (iter != m_pThreads->end()) {		
		printf("Thread %d %x\n", (*iter).threadId, (*iter).threadHandle);
		iter++;
	}
}