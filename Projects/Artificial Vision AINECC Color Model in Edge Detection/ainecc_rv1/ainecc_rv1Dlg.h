// ainecc_rv1Dlg.h : header file
//

#pragma once
#include "afxwin.h"
#include <highgui.h>


#define AI_RGB3C_MinC   313
#define AI_RGB3C_MaxC	323
#define AI_RGB3C_MabC	343




#ifndef aimax
#define aimax(a,b)            (((a) > (b)) ? (a) : (b))
#endif

#ifndef aimin
#define aimin(a,b)            (((a) < (b)) ? (a) : (b))
#endif

// Cainecc_rv1Dlg dialog
class Cainecc_rv1Dlg : public CDialog
{
// Construction
public:
	Cainecc_rv1Dlg(CWnd* pParent = NULL);	// standard constructor
	
	CvCapture* cameraRE; // camera Right Eye

	IplImage* CameraREFrame;
	IplImage* FrameRE;
	IplImage* FrameAux;
	IplImage* Edge_aux;
	IplImage* AINECC_aux;

	afx_msg void OnBnClickedButtonOpenVid();
	afx_msg void OnBnClickedButtonCloseVid();
	CString m_hWnd;
	
	CString CamChannelSeq;
	CString RGBChannelSeq;
	CString BGRChannelSeq;
	CString GRAYChannelSeq;

	CString m_hWndEx;

// Dialogs	
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();


// functions
	int ainImgInCVWnd(IplImage* srcImg, char* name, CvSize size);
	int ainGetColor8U( IplImage* srcCo8Uarr, IplImage* dstCo8Uarr, int code);
	int ainGetDifer8U( IplImage* srcDifArr, IplImage* dstDifArr, int code);
	int aiSobel8UC3( CvArr* src, CvArr* dst, int xorder, int yorder, int aperture_size CV_DEFAULT(3));
	int ainU8to32F( IplImage* srcTo32fArr, IplImage* dstTo32fArr, float cha4value);

// Dialog Data
	enum { IDD = IDD_AINECC_RV1_DIALOG };


private:
        UINT_PTR m_nTimer;


// Implementation
protected:
	HICON m_hIcon;
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg void OnClose();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	int intCamSRCHeight;
	int intCamSRCWidth;
	int intImgSRCHeight;
	int intImgSRCWindth;
	double dblCamSRC_FPS;
	CString strCamSRCHeight;
	CString strCamSRCWidth;
	CString strImgSRCHeight;
	CString strImgSRCWindth;
	CString strCamSRC_FPS;
	CString strImgSRC_FPS;
	CString strCamSRC_COLOR;
	CString strImgSRC_COLOR;
	CString strColor_Model[3];
	CString strColor_AINECC[3];
	int intColor_AINECC[3];
	CEdit m_CamSRCHeight;
	CEdit m_CamSRCWidth;
	CEdit m_CamSRC_FPS;
	CEdit m_ImgSRC_FPS;
	CEdit m_ImgSRCHeight;
	CEdit m_ImgSRCWindth;
	CEdit m_CmbSRC_Color;
	afx_msg void OnAboutAinec();
	afx_msg void OnCbnSelchangeImgColor();
	CComboBox m_CmbIMG_Color;
	CComboBox m_cmbAINEC_Color;
	CString strAINEC_Color;
	CEdit m_EdgeCoef;
	int intEdgeCoef;
	afx_msg void OnEnKillfocusEdgeCoef();
	afx_msg void OnCbnSelchangeAinecColor();
	afx_msg void OnBnClickedImgSave();
	CEdit m_EdgeSobelX;
	int intEdgeSobelX;
	CEdit m_EdgeSobelY;
	int intEdgeSobelY;
	afx_msg void OnEnKillfocusEdgeSobelx();
	afx_msg void OnEnKillfocusEdgeSobely();
};


