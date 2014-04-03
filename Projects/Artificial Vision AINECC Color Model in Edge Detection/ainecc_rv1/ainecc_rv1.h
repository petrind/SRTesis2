// ainecc_rv1.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// Cainecc_rv1App:
// See ainecc_rv1.cpp for the implementation of this class
//

class Cainecc_rv1App : public CWinApp
{
public:
	Cainecc_rv1App();

// Overrides
	public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern Cainecc_rv1App theApp;