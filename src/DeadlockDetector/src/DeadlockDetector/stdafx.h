// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#ifndef __STDAFX_H
#define __STDAFX_H

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows XP or later.                   
#define _WIN32_WINNT 0x0501	// Change this to the appropriate value to target other versions of Windows.
#endif						

#include <stdio.h>
#include <tchar.h>
#include <mscoree.h>
#include <corerror.h>

#include "HostControl.h"
#include "CrstLock.h"
#include "Context.h"
#include "Task.h"
#include "TaskManager.h"
#include "SyncManager.h"
#include "Event.h"
#include "Crst.h"

void Fail(const char *msg);
void FailWin32(const char *msg);
void CheckFail(HRESULT hr, const char *msg);
void CheckFail(HRESULT hr, const char *msg, bool shutdown);

#endif /* __STDAFX_H */
