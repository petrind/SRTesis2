// ainecc_rv1Dlg.cpp : implementation file
// Education 

#include "stdafx.h"
#include "ainecc_rv1.h"
#include "ainecc_rv1Dlg.h"

#include <cv.h>
#include <highgui.h>
#include <math.h>

#include <sys/timeb.h>
#include <time.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About 

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// Cainecc_rv1Dlg dialog




Cainecc_rv1Dlg::Cainecc_rv1Dlg(CWnd* pParent /*=NULL*/)
	: CDialog(Cainecc_rv1Dlg::IDD, pParent)
	, m_hWnd(_T("Video Source  ->"))
	, intCamSRCHeight(480)
	, intCamSRCWidth(640)
	, intImgSRCHeight(480)
	, intImgSRCWindth(640)
	, dblCamSRC_FPS(5)
	, strAINEC_Color(_T(""))
	, intEdgeCoef(4)
	, intEdgeSobelX(1)
	, intEdgeSobelY(0)
{
	
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

}

void Cainecc_rv1Dlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);

	DDX_Text(pDX, IDC_VIDEO_SOURCE_WND, m_hWnd);
	DDX_Control(pDX, IDC_SRC_HEIGHT, m_CamSRCHeight);
	DDX_Text(pDX, IDC_SRC_HEIGHT, intCamSRCHeight);
	DDV_MinMaxInt(pDX, intCamSRCHeight, 120, 1080);
	DDX_Control(pDX, IDC_SRC_WIDTH, m_CamSRCWidth);
	DDX_Text(pDX, IDC_SRC_WIDTH, intCamSRCWidth);
	DDV_MinMaxInt(pDX, intCamSRCWidth, 160, 1920);
	DDX_Control(pDX, IDC_SRC_FPS, m_CamSRC_FPS);
	DDX_Text(pDX, IDC_SRC_FPS, dblCamSRC_FPS);
	DDV_MinMaxDouble(pDX, dblCamSRC_FPS, 1, 30);
	DDX_Control(pDX, IDC_IMG_HEIGHT, m_ImgSRCHeight);
	DDX_Text(pDX, IDC_IMG_HEIGHT, intImgSRCHeight);
	DDV_MinMaxInt(pDX, intImgSRCHeight, 120, 1080);
	DDX_Control(pDX, IDC_IMG_WIDTH, m_ImgSRCWindth);
	DDX_Text(pDX, IDC_IMG_WIDTH, intImgSRCWindth);
	DDV_MinMaxInt(pDX, intImgSRCWindth, 160, 1920);
	DDX_Control(pDX, IDC_SRC_COLOR, m_CmbSRC_Color);
	DDX_Text(pDX, IDC_SRC_COLOR, strCamSRC_COLOR);
	DDX_Control(pDX, IDC_IMG_FPS, m_ImgSRC_FPS);
	DDX_Control(pDX, IDC_IMG_COLOR, m_CmbIMG_Color);
	DDX_Control(pDX, IDC_AINEC_COLOR, m_cmbAINEC_Color);
	DDX_CBString(pDX, IDC_AINEC_COLOR, strAINEC_Color);
	DDX_Control(pDX, IDC_EDGE_COEF, m_EdgeCoef);
	DDX_Text(pDX, IDC_EDGE_COEF, intEdgeCoef);
	DDV_MinMaxInt(pDX, intEdgeCoef, 0, 100);
	DDX_Control(pDX, IDC_EDGE_SOBELX, m_EdgeSobelX);
	DDX_Text(pDX, IDC_EDGE_SOBELX, intEdgeSobelX);
	DDV_MinMaxInt(pDX, intEdgeSobelX, -100, 100);
	DDX_Control(pDX, IDC_EDGE_SOBELY, m_EdgeSobelY);
	DDX_Text(pDX, IDC_EDGE_SOBELY, intEdgeSobelY);
	DDV_MinMaxInt(pDX, intEdgeSobelY, -100, 100);
}

BEGIN_MESSAGE_MAP(Cainecc_rv1Dlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTONPRUEBA, &Cainecc_rv1Dlg::OnBnClickedButtonOpenVid)
	ON_BN_CLICKED(IDC_CLOSEVID, &Cainecc_rv1Dlg::OnBnClickedButtonCloseVid)
	ON_BN_CLICKED(IDOK, &Cainecc_rv1Dlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &Cainecc_rv1Dlg::OnBnClickedCancel)
	ON_COMMAND(IDM_ABOUT_AINEC, &Cainecc_rv1Dlg::OnAboutAinec)
	ON_CBN_SELCHANGE(IDC_IMG_COLOR, &Cainecc_rv1Dlg::OnCbnSelchangeImgColor)
	ON_EN_KILLFOCUS(IDC_EDGE_COEF, &Cainecc_rv1Dlg::OnEnKillfocusEdgeCoef)
	ON_CBN_SELCHANGE(IDC_AINEC_COLOR, &Cainecc_rv1Dlg::OnCbnSelchangeAinecColor)
	ON_BN_CLICKED(IDC_IMG_SAVE, &Cainecc_rv1Dlg::OnBnClickedImgSave)
	ON_EN_KILLFOCUS(IDC_EDGE_SOBELX, &Cainecc_rv1Dlg::OnEnKillfocusEdgeSobelx)
	ON_EN_KILLFOCUS(IDC_EDGE_SOBELY, &Cainecc_rv1Dlg::OnEnKillfocusEdgeSobely)
END_MESSAGE_MAP()


// Cainecc_rv1Dlg message handlers

BOOL Cainecc_rv1Dlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	cameraRE = NULL;
	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(false);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, true);			// Set big icon
	SetIcon(m_hIcon, false);		// Set small icon

	// Cainecc_rv1Dlg Initialization here
	CameraREFrame = 0;
	FrameRE = 0;
	FrameAux = 0;
	Edge_aux = 0;
	AINECC_aux = 0;

	m_CamSRCHeight.SetWindowTextW (_T("480"));
	m_CamSRCWidth.SetWindowTextW (_T("640"));
	m_CamSRC_FPS.SetWindowTextW (_T("5"));
	m_ImgSRCHeight.SetWindowTextW (_T("480"));
	m_ImgSRCWindth.SetWindowTextW (_T("640"));
	
	strColor_Model[0] = _T("3RGB->1BW 8U");
	strColor_Model[1] = _T("BW 8U");
	strColor_Model[2] = _T("RGB 8U");


	strColor_AINECC[0] = _T("Relative RGB Levels");
	strColor_AINECC[1] = _T("Max. of RGB Level");
	strColor_AINECC[2] = _T("Min. of RGB Level");
	intColor_AINECC[0] = AI_RGB3C_MabC;
	intColor_AINECC[1] = AI_RGB3C_MaxC;
	intColor_AINECC[2] = AI_RGB3C_MinC;
	strAINEC_Color = _T("Relative RGB Levels");

	
	strCamSRC_COLOR = _T("Closed");

	m_CmbIMG_Color.InsertString(0,strColor_Model[0]);
	m_CmbIMG_Color.InsertString(1,strColor_Model[1]);
	m_CmbIMG_Color.InsertString(1,strColor_Model[2]);
	m_CmbIMG_Color.SetCurSel(0);

	m_cmbAINEC_Color.InsertString(0,strColor_AINECC[0]);
	m_cmbAINEC_Color.InsertString(1,strColor_AINECC[1]);
	m_cmbAINEC_Color.InsertString(2,strColor_AINECC[2]);
	m_cmbAINEC_Color.SetCurSel(0);
	
	UpdateData(false);

	return true;  // return true  unless you set the focus to a control
}

void Cainecc_rv1Dlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void Cainecc_rv1Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR Cainecc_rv1Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void Cainecc_rv1Dlg::OnBnClickedButtonOpenVid()
{
	CameraREFrame = 0;
	cameraRE = 0;

	HWND hWnd;
	HWND hParent;

	RECT rcDlg; //ScreenToClient

	int vWndType1Height;
	int vWndType1Windth;

	int SourceDepth;
	int ImageChannels;
	int EdgeChannels;

	struct _timeb timebuffer;
	double vTime, vTime0;


	RGBChannelSeq = "RGB";
	BGRChannelSeq = "BGR";
	GRAYChannelSeq = "GRAY";

 UpdateData(true); // Update Data coming from Dialog
	
 cameraRE = cvCreateCameraCapture( CV_CAP_ANY );
 
 // Setting camera parameters
 cvSetCaptureProperty( cameraRE, CV_CAP_PROP_FRAME_WIDTH, double(intCamSRCWidth));
 cvSetCaptureProperty( cameraRE, CV_CAP_PROP_FRAME_HEIGHT, double(intCamSRCHeight));
 cvSetCaptureProperty( cameraRE, CV_CAP_PROP_FPS, dblCamSRC_FPS );

 intCamSRCWidth = int(cvGetCaptureProperty( cameraRE, CV_CAP_PROP_FRAME_WIDTH));
 intCamSRCHeight = int (cvGetCaptureProperty( cameraRE, CV_CAP_PROP_FRAME_HEIGHT));
 dblCamSRC_FPS = cvGetCaptureProperty( cameraRE, CV_CAP_PROP_FPS);
 if (dblCamSRC_FPS <= 0) dblCamSRC_FPS = 30; // There is a limitation or bug in opencv. It can not change de camera's FPS, returning 0

if (cameraRE > 0) 
{
	m_CamSRCHeight.EnableWindow(false);
	m_CamSRCWidth.EnableWindow(false);
	m_CamSRC_FPS.EnableWindow(false);
	m_ImgSRCHeight.EnableWindow(false);
	m_ImgSRCWindth.EnableWindow(false);
	m_CmbIMG_Color.EnableWindow(false);

 	GetDlgItem(IDC_VIDEO_SOURCE_WND)->GetWindowRect(&rcDlg);
	vWndType1Windth = rcDlg.right - rcDlg.left -10;
	vWndType1Height = vWndType1Windth * intImgSRCHeight;
	vWndType1Height = vWndType1Height / intImgSRCWindth; 

		//  Source Window
	cvNamedWindow("AIWndSrcCameraRE", CV_WINDOW_AUTOSIZE);
	hWnd = (HWND) cvGetWindowHandle("AIWndSrcCameraRE");
	hParent = ::GetParent(hWnd);
	::SetParent(hWnd, GetDlgItem(IDC_VIDEO_SOURCE_WND)->m_hWnd);
	::ShowWindow(hParent, SW_HIDE);
	
	cvResizeWindow("AIWndSrcCameraRE", vWndType1Windth,vWndType1Height);

	//  Color Window

	cvNamedWindow("AIWndSrcExper", CV_WINDOW_AUTOSIZE);
	hWnd = (HWND) cvGetWindowHandle("AIWndSrcExper");
	hParent = ::GetParent(hWnd);
	::SetParent(hWnd, GetDlgItem(IDC_VIDEO_SOURCE_WND2)->m_hWnd);
	::ShowWindow(hParent, SW_HIDE);

	cvResizeWindow("AIWndSrcExper", vWndType1Windth,vWndType1Height);
	
	//  Edge Window
	cvNamedWindow("AIWndRAD", CV_WINDOW_AUTOSIZE);
	hWnd = (HWND) cvGetWindowHandle("AIWndRAD");
	hParent = ::GetParent(hWnd);
	::SetParent(hWnd, GetDlgItem(IDC_WND_RAD)->m_hWnd);
	::ShowWindow(hParent, SW_HIDE);

	cvResizeWindow("AIWndRAD", vWndType1Windth,vWndType1Height);

	CameraREFrame =cvRetrieveFrame (cameraRE);

	if (CameraREFrame->depth == IPL_DEPTH_8U)
	SourceDepth = IPL_DEPTH_8U;
	else
	{
	MessageBox (_T("This camera depth is not supported"));
	return;
	}
	
	CamChannelSeq = (CString)CameraREFrame->colorModel;
	if (CamChannelSeq == RGBChannelSeq || CamChannelSeq == BGRChannelSeq)	
	strCamSRC_COLOR = strColor_Model[2];
	else if (CamChannelSeq == GRAYChannelSeq)
	strCamSRC_COLOR = strColor_Model[1];
	else
	strCamSRC_COLOR = strColor_Model[0];

	m_CmbSRC_Color.SetWindowTextW (strCamSRC_COLOR);
	UpdateData(false); // Updating Data coming from Dialog

	/*
The “color” combo box selection affects both images.  
- 3RGB->1BW 8: (Color > RGB 8U, Edge BW 8U). The “edge” are detected in each channel independently and then mixed in one.
- RGB 8U: (Color > RGB 8U, Edge RGB 8U). The “edge” are detected in each channel independently and shown them in a RGB image.
- BW 8U: (Color > BW 8U, Edge BW 8U). This mode simulates Black and White cameras (BW). Booth “Color” and “Edge” images are treated in one Gray channel.
	*/
	if (CameraREFrame != NULL)
	{
		if (strImgSRC_COLOR == strColor_Model[0])
		{
			ImageChannels = 3;
			EdgeChannels = 1;
		}
		else if (strImgSRC_COLOR == strColor_Model[1])
		{
			ImageChannels = 1;
			EdgeChannels = 1;
		}
		else if (strImgSRC_COLOR == strColor_Model[2])
		{
			ImageChannels = 3;
			EdgeChannels = 3;
		}
		else
		{
			ImageChannels = CameraREFrame->nChannels;
			EdgeChannels = 1;
		}

		FrameRE = cvCreateImage( cvSize(intImgSRCWindth,intImgSRCHeight), IPL_DEPTH_8U, CameraREFrame->nChannels);
		AINECC_aux = cvCreateImage( cvGetSize(FrameRE), IPL_DEPTH_8U, ImageChannels );
		Edge_aux = cvCreateImage( cvGetSize(FrameRE), IPL_DEPTH_8U, EdgeChannels );
		FrameAux = cvCreateImage( cvGetSize(CameraREFrame), CameraREFrame->depth, CameraREFrame->nChannels); //IPL_DEPTH_32F

		_ftime64_s( &timebuffer );	
		vTime0 = double(timebuffer.time) + double(timebuffer.millitm)/1000;	

while (FrameRE != NULL) // this application is very simple so to keep it working it is "enclosed" in a loop
{
	// Calculating the Frames 
		_ftime64_s( &timebuffer );	
		vTime = double(timebuffer.time) + double(timebuffer.millitm)/1000;
		strImgSRC_FPS.Format(_T("%f"), (1/(vTime-vTime0)));
		m_ImgSRC_FPS.SetWindowTextW(strImgSRC_FPS);
		vTime0 = vTime;
		
		CameraREFrame =cvQueryFrame (cameraRE);

		if(!CameraREFrame)
			return;
		else
		{
		// Copy image to avoid some troubles releasing camera resources
			if( CameraREFrame->origin == IPL_ORIGIN_TL )
			cvResize (CameraREFrame, FrameRE, CV_INTER_NN);
			else
			cvFlip( CameraREFrame, FrameRE, 0 );

			ainGetColor8U(FrameRE,AINECC_aux, intColor_AINECC[m_cmbAINEC_Color.GetCurSel()]);

			// For 3 channels source we chose the diference algoritm
			if (AINECC_aux->nChannels == 1 && Edge_aux->nChannels == 1)
			{
				m_EdgeCoef.EnableWindow(false);
				aiSobel8UC3(AINECC_aux,Edge_aux,intEdgeSobelX,intEdgeSobelY,3);
			}
			else
			{
				m_EdgeSobelX.EnableWindow(false);
				m_EdgeSobelY.EnableWindow(false);
				ainGetDifer8U(AINECC_aux, Edge_aux, intEdgeCoef);
			}

 
// just an small exercice to grite a cross in the image
		CvPoint punt1, punt2, punt3, punt4;

		punt1.x = int ((FrameRE->width)/2.1);
		punt1.y = int ((FrameRE->height)/2);
		punt2.x = int ((FrameRE->width)/1.9);
		punt2.y = int ((FrameRE->height)/2);
		punt3.x = int ((FrameRE->width)/2);
		punt3.y = int ((FrameRE->height)/2.1);
		punt4.x = int ((FrameRE->width)/2);
		punt4.y = int ((FrameRE->height)/1.9);

		cvLine(AINECC_aux, punt1,punt2,cvScalar(1,1,1,0),4);
		cvLine(AINECC_aux, punt3,punt4,cvScalar(1,0,1,0),4);
		
		cvLine(FrameRE, punt1,punt2,cvScalar(0,0,0,0),4);
		cvLine(FrameRE, punt3,punt4,cvScalar(1,0,1,0),4);

		ainImgInCVWnd(FrameRE, "AIWndSrcCameraRE", cvSize(vWndType1Windth,vWndType1Height));
		ainImgInCVWnd(AINECC_aux, "AIWndSrcExper", cvSize(vWndType1Windth,vWndType1Height));
		ainImgInCVWnd(Edge_aux, "AIWndRAD", cvSize(vWndType1Windth,vWndType1Height));
		}
}
	m_CamSRCHeight.EnableWindow(true);
	m_CamSRCWidth.EnableWindow(true);
	m_CamSRC_FPS.EnableWindow(true);
	m_ImgSRCHeight.EnableWindow(true);
	m_ImgSRCWindth.EnableWindow(true);
	m_CmbIMG_Color.EnableWindow(true);

	m_EdgeCoef.EnableWindow(true);
	m_EdgeSobelX.EnableWindow(true);
	m_EdgeSobelY.EnableWindow(true);
	}
return;
}
}


void Cainecc_rv1Dlg::OnBnClickedButtonCloseVid()
{
	if ( cameraRE != 0)
	{
	cvReleaseCapture( &cameraRE );
	cvDestroyAllWindows();

	strCamSRC_COLOR = _T("Closed");
	UpdateData(false);

	if (FrameAux != 0) cvReleaseImage(&FrameAux);
	if (AINECC_aux != 0) cvReleaseImage(&AINECC_aux);
	if (Edge_aux != 0) cvReleaseImage(&Edge_aux);

	}
	m_CamSRCHeight.EnableWindow(true);
	m_CamSRCWidth.EnableWindow(true);
	m_CamSRC_FPS.EnableWindow(true);
	m_ImgSRCHeight.EnableWindow(true);
	m_ImgSRCWindth.EnableWindow(true);
	m_CmbIMG_Color.EnableWindow(true);

	m_EdgeCoef.EnableWindow(true);
	m_EdgeSobelX.EnableWindow(true);
	m_EdgeSobelY.EnableWindow(true);
}

void Cainecc_rv1Dlg::OnBnClickedOk()
{
	if ( cameraRE != 0)
	{
	cvReleaseCapture( &cameraRE );
	cvDestroyAllWindows();

	if (FrameAux != 0) cvReleaseImage(&FrameAux);
	if (AINECC_aux != 0) cvReleaseImage(&AINECC_aux);
	if (Edge_aux != 0) cvReleaseImage(&Edge_aux);

	}
	OnOK();
}

void Cainecc_rv1Dlg::OnBnClickedCancel()
{
	if ( cameraRE != 0 && CameraREFrame != 0)
	{
	cvReleaseCapture( &cameraRE );
	cvDestroyAllWindows();

	if (FrameAux != 0) cvReleaseImage(&FrameAux);
	if (AINECC_aux != 0) cvReleaseImage(&AINECC_aux);
	if (Edge_aux != 0) cvReleaseImage(&Edge_aux);
	}
	OnCancel();
}


int Cainecc_rv1Dlg::ainImgInCVWnd(IplImage* srcImg, char* name, CvSize dst_size)
{

	IplImage* dstImgAdapt;

    int src_cn, src_depth;
	if (!srcImg || dst_size.height < 8 || dst_size.width < 8 )
		return (-1);

	src_depth = srcImg->depth;
	src_cn = srcImg->nChannels;

	// depth could be: IPL_DEPTH_8U, IPL_DEPTH_8S, IPL_DEPTH_16U,IPL_DEPTH_16S, IPL_DEPTH_32S, IPL_DEPTH_32F and IPL_DEPTH_64F  
	// we work only with unsigned depth IPL_DEPTH_8U and IPL_DEPTH_32F
    if( src_depth != IPL_DEPTH_8U && src_depth != IPL_DEPTH_32F)
	{
		if (MessageBox( _T ("Sts Unsupported Format"), _T ("AINECC ERROR"),MB_OKCANCEL ) == IDCANCEL)
		{
			OnBnClickedCancel();
		}
		return (-1);
	}

	dstImgAdapt = cvCreateImage(dst_size ,src_depth ,src_cn );
	cvResize(srcImg, dstImgAdapt, CV_INTER_LINEAR);
	cvShowImage(name, dstImgAdapt);
	if ((cvWaitKey(10) & 255) == 27 ) return (0); 

	cvReleaseImage(&dstImgAdapt);
return (0);
}


int Cainecc_rv1Dlg::ainGetColor8U( IplImage* srcCo8Uarr, IplImage* dstCo8Uarr, int code)
{
/*
AI_RGB3C_MabC	// AINEC Color Model  
AI_RGB3C_MaxC	// Maximum level of the R, G or B Color 
AI_RGB3C_MinC	// Minimum level of the R, G or B Color
This function make difference between RGB o BGR Color model and 1 o 3 channels of depth
*/

	CvMat srcstub, *srcMat = (CvMat*)srcCo8Uarr;
    CvMat dststub, *dstMat = (CvMat*)dstCo8Uarr;
    CvSize size;
    int src_step, dst_step;
    int src_cn, dst_cn, src_depth, dst_depth;
	int idxBlue, idxRed;
	CString BGRChSeq, chSeq;
	
	bool bSizesEq = false;
	bool bTypeEq = false;

	src_depth = srcCo8Uarr->depth;
	dst_depth = dstCo8Uarr->depth;
	src_cn = srcCo8Uarr->nChannels ; // chanels
    dst_cn = dstCo8Uarr->nChannels ; // chanels
    size = cvGetSize( srcCo8Uarr );
    src_step = srcCo8Uarr->widthStep; 
    dst_step = dstCo8Uarr->widthStep;

	BGRChSeq = "BGR";
	chSeq = (CString)srcCo8Uarr->channelSeq;
	if (chSeq == BGRChSeq)
	{
		idxBlue = 0;
		idxRed = 2;
	}
	else 
	{
		idxBlue = 2;
		idxRed = 0;
	}


if (src_depth == IPL_DEPTH_8U)
{

	int i, j;  
	int tmpCol, tmpRow;
	int tmpMax, tmpMin;
	int srcLineSize4ptr, dstLineSize4ptr;
	double dblNoiseLevel;

	unsigned char* src_ptr;
	unsigned char* dst_ptr;
    
    srcMat = cvGetMat( srcCo8Uarr, &srcstub );
    dstMat = cvGetMat( dstCo8Uarr, &dststub );
    
	if (!srcCo8Uarr)
		return (-1);

    if( CV_ARE_SIZES_EQ( srcMat, dstMat ))
        bSizesEq = true;
	else
	{
		bSizesEq = false;
		return (-1);
	}

	src_ptr = srcMat->data.ptr ;
	dst_ptr = dstMat->data.ptr ;

	tmpRow = size.height;
	tmpCol = size.width;
	srcLineSize4ptr = tmpCol * src_cn;
	dstLineSize4ptr = tmpCol * dst_cn;

	int intRed, intGreen, intBlue;
	double dblRed, dblGreen, dblBlue;
	int ScaleRGB = 255;
	
	double aiGdRB, aiRdGB, aiBdRG;

switch (code)
{
case AI_RGB3C_MaxC:

	dblNoiseLevel = 10;

	for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr)
	{      
		j = 0;

        for(i = 0; i <= (srcLineSize4ptr - src_cn); i += src_cn)
        {   	
			intBlue = int (src_ptr[i+idxBlue]);		// blue  
			intGreen = int (src_ptr[i+1]);			// green
			intRed = int (src_ptr[i+idxRed]);		// red 

			ScaleRGB = 255;

			tmpMax = aimax(intRed, aimax(intBlue,intGreen ) );
			if (tmpMax > dblNoiseLevel) // lower than "dblNoiseLevel" of light means dark scene.
			{
			ScaleRGB = 65025/(tmpMax);
			intGreen = intGreen * ScaleRGB;
			intBlue = intBlue * ScaleRGB;
			intRed = intRed * ScaleRGB;
			}
			else
			{
			intGreen = 0;
			intBlue = 0;
			intRed = 0;
			}
		
		if ( dst_cn >= 3)
		{
			dst_ptr[j+idxBlue] = unsigned char (intBlue/255);                                            
            dst_ptr[j+1] = unsigned char (intGreen/255);						
			dst_ptr[j+idxRed] = unsigned char (intRed/255);                                             
		}
		else if ( dst_cn == 1)
			dst_ptr[j] = unsigned char ((aimax(intBlue, aimax(intGreen,intRed)))/255); 
		else 
			return -1;

			j += dst_cn;
		}
	}
	 break;

case AI_RGB3C_MabC:

dblNoiseLevel = 0.0;
if (dst_cn >= 3)
{
	for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr)
	{      
		j = 0;

        for(i = 0; i <= srcLineSize4ptr - src_cn; i += src_cn)
		{   

		dblBlue = double (src_ptr[i+idxBlue]);		// blue  
		dblGreen = double (src_ptr[i+1]);			// green
		dblRed = double (src_ptr[i+idxRed]);			// red 
			
		dblBlue = dblBlue/255.0;
		dblGreen = dblGreen/255.0;
		dblRed = dblRed/255.0;

		if ((dblRed > 0) && (dblBlue > 0) )
			aiGdRB = atan (dblGreen/(dblRed * dblBlue) ); // Range {0,(pi/2)
		else if (dblGreen > 0)
			aiGdRB = 1.570796;
		else
			aiGdRB = 0;

		if ((dblGreen > 0) && (dblBlue > 0) )
			aiRdGB = atan (dblRed/(dblGreen * dblBlue)); // Range {0,(pi/2)
		else if (dblRed > 0)
			aiRdGB = 1.570796;
		else
			aiRdGB = 0;
		
		if ((dblRed > 0) && (dblGreen > 0) )
			aiBdRG = atan(dblBlue/(dblGreen * dblRed)); // Range {0,(pi/2)
		else if (dblBlue > 0)
			aiBdRG = 1.570796;
		else
			aiBdRG = 0;

			dst_ptr[j+idxBlue] = unsigned char (aiBdRG*162);// 162 = 255/(pi/2) => Range {0,255}                                        
            dst_ptr[j+1] = unsigned char (aiGdRB*162);						
			dst_ptr[j+idxRed] = unsigned char (aiRdGB*162);                                             
			j += dst_cn;
		}
	}
	 break;
}
else if ( dst_cn == 1)
{
	 for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr)
	{      
		j = 0;

        for(i = 0; i <= srcLineSize4ptr - src_cn; i += src_cn)
        {   
		dblBlue = double (src_ptr[i+idxBlue]);	// blue  
		dblGreen = double (src_ptr[i+1]);		// green
		dblRed = double (src_ptr[i+idxRed]);		// red 
			
			if (dblGreen > 0)
			{
				if (dblBlue > 0)
				{
					if (dblRed > 0)
					{
					dblBlue = dblBlue/255.0;
					dblGreen = dblGreen/255.0;
					dblRed = dblRed/255.0;
					
					if ((dblGreen >= dblRed) && (dblGreen >= dblBlue)) 
						aiRdGB = atan (dblGreen / (dblRed * dblBlue)) ; // Range {0,(pi/2)
					else if ((dblBlue > dblRed) && (dblBlue > dblGreen)) 
						aiRdGB = atan (dblBlue / (dblRed * dblGreen)) ; // Range {0,(pi/2)
					else
						aiRdGB = atan (dblRed / (dblGreen * dblBlue)); // Range {0,(pi/2)
					}
					else
						aiRdGB = 0;
				}
				else
					aiRdGB = 1 ;
			}
			else if (dblRed > 0) // green = 0
			{
				if (dblBlue > 0)
					aiRdGB = 1 ;
				else
					aiRdGB = 0 ;
			}
			else		// green and red = 0
			{
				if (dblBlue > 0)
					aiRdGB = 1 ;
				else
					aiRdGB = 0 ; // green, red and blue = 0
			}

			dst_ptr[j] = unsigned char (aiRdGB*162);  //  162 = 255/(pi/2) => Range {0,255}                                                      
			j += dst_cn;
		}
	 }
    break;
}
else 
	return -1;


case AI_RGB3C_MinC:
  
	for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr)
	{      
		j = 0;

        for(i = 0; i <= srcLineSize4ptr - src_cn; i += src_cn)
        {   
			intRed = int (src_ptr[i+idxRed]);		// red 
			intGreen = int (src_ptr[i+1]);			// green
			intBlue = int (src_ptr[i+idxBlue]);		// blue 

//			RGB3C TO LOWGRAY3C (LOWER GRAY LEVEL)

			tmpMin = aimin(intRed,aimin(intBlue,intGreen ) );

		if ( dst_cn >= 3)
		{
			dst_ptr[j+idxRed] = unsigned char (tmpMin);                                            
            dst_ptr[j+1] = unsigned char (tmpMin);						
			dst_ptr[j+idxBlue] = unsigned char (tmpMin);
		}
		else if ( dst_cn == 1)
			dst_ptr[j] = unsigned char (tmpMin);
		else 
			return -1;

			j += dst_cn;
 		}
	}
	break;

default:
return (-1);	
}
return (code);
}
else return (-1);
}


int Cainecc_rv1Dlg::ainGetDifer8U(IplImage* srcDifArr, IplImage* dstDifArr, int code)
{

    CvMat srcstubDif, *srcMatDif = (CvMat*)srcDifArr;
    CvMat dststubDif, *dstMatDif = (CvMat*)dstDifArr;
    CvSize size;
    int src_step, dst_step;
    int src_cn, dst_cn, src_depth, dst_depth; 
	
	bool bSizesEq = false;
	bool bDepthsEq = false;
	bool bTypeEq = false;

	src_depth = srcDifArr->depth;
	dst_depth = dstDifArr->depth;
	src_cn = srcDifArr->nChannels ; // chanels
    dst_cn = dstDifArr->nChannels ; // chanels
    size = cvGetSize( srcDifArr );
    src_step = srcDifArr->widthStep; 
    dst_step = dstDifArr->widthStep;

if( src_depth == IPL_DEPTH_8U && dst_depth == IPL_DEPTH_8U)
{
	int i, j;  
 	int tmpCol, tmpRow;

	unsigned char* src_ptr;
	unsigned char* dst_ptr;
    
    srcMatDif = cvGetMat( srcDifArr, &srcstubDif );
    dstMatDif = cvGetMat( dstDifArr, &dststubDif );
    
	if (!srcDifArr)
		return (-1);

    if( CV_ARE_SIZES_EQ( srcMatDif, dstMatDif ))
        bSizesEq = true;
	else
	{
		bSizesEq = false;
		return (-1);
	}

	if (!code) code = 5;

	src_ptr = srcMatDif->data.ptr ;
	dst_ptr = dstMatDif->data.ptr ;

	int srcLineSize4ptr, dstLineSize4ptr;

	srcLineSize4ptr = size.width * src_cn;
	dstLineSize4ptr = size.width * dst_cn;

if 	(src_cn == 1)
{
	unsigned char* src_ptrLB;
	unsigned char* src_ptrLN;
	int vaR[] = {0,0,0,0,0,0,0,0,0};
	int n;
	int vAngle = 0;
	int intTmp1 = 0;
	int intTmp2 = 0;
	int intSum = 0;

	tmpCol = size.width;
	tmpRow = size.height - 1;

  	 for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr  )      
	 { 
		if ( size.height - 2 >= tmpRow)
		{
		src_ptrLB = src_ptr + srcLineSize4ptr;
		src_ptrLN = src_ptr;
		}
		else if ( tmpRow == 0)
		{
		src_ptrLB = src_ptr;
		src_ptrLN = src_ptr - srcLineSize4ptr;
		}
		else
		{
		src_ptrLB = src_ptr + srcLineSize4ptr;
		src_ptrLN = src_ptr - srcLineSize4ptr;
		}
	
		j = 0;
        for( i = src_cn; i <= srcLineSize4ptr - src_cn; i += src_cn)                  
        { 
			vaR[0] = src_ptr[i];
			vaR[1] = src_ptrLB[i-1]; 
			vaR[2] = src_ptrLB[i];
			vaR[3] = src_ptrLB[i+1]; 
			vaR[4] = src_ptr[i-1];  
			vaR[5] = src_ptr[i+1];	
			vaR[6] = src_ptrLN[i-1];	 
			vaR[7] = src_ptrLN[i];
			vaR[8] = src_ptrLN[i+1];	
			
			vAngle = 0;
			n = 0;
			
			intSum = (vaR[1] + vaR[2] + vaR[3] + vaR[4] + vaR[5] + vaR[6] + vaR[7] + vaR[8]) / 8;
			intTmp2 = (code * intSum) / 100;

			while ((vAngle == 0) && (n<9))
			{
			intTmp1 = abs (intSum - vaR[n]) ;
			if (intTmp2 < intTmp1)
				vAngle = n;
			n++;
			}
		
		if (vAngle > 0)
			dst_ptr[j] = unsigned char (0);
		else
			dst_ptr[j] = unsigned char (255);

		for (n = 1; n < dst_cn; n++)
			dst_ptr[j+n] = dst_ptr[j];

		j += dst_cn;

        }                                                               
    }


 return (dst_cn);
}

else if (src_cn == 3)
{

	unsigned char* src_ptrLB;
	unsigned char* src_ptrLN;
	int vaRGB[27];
	int n;
	int vAngle[] = {0,0,0};
	int intTmp1[] = {0,0,0};
	int intTmp2[] = {0,0,0};
	int intSum[] = {0,0,0};

	tmpCol = size.width;
	tmpRow = size.height - 1;


	 for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr )              
	 { 
		if ( size.height - 2 >= tmpRow)
		{
		src_ptrLB = src_ptr + srcLineSize4ptr;
		src_ptrLN = src_ptr;
		}
		else if ( tmpRow == 0)
		{
		src_ptrLB = src_ptr;
		src_ptrLN = src_ptr - srcLineSize4ptr;
		}
		else
		{
		src_ptrLB = src_ptr + srcLineSize4ptr;
		src_ptrLN = src_ptr - srcLineSize4ptr;
		}
	
		j = 0;
        for( i = src_cn; i <= srcLineSize4ptr - src_cn; i += src_cn)                  
        { 
			vaRGB[0] = src_ptr[i];
			vaRGB[1] = src_ptrLB[i-3];
			vaRGB[2] = src_ptrLB[i];
			vaRGB[3] = src_ptrLB[i+3];
			vaRGB[4] = src_ptr[i-3];
			vaRGB[5] = src_ptr[i+3];
			vaRGB[6] = src_ptrLN[i-3];
			vaRGB[7] = src_ptrLN[i];
			vaRGB[8] = src_ptrLN[i+3];

			vaRGB[9] = src_ptr[i+1];
			vaRGB[10] = src_ptrLB[i-2];
			vaRGB[11] = src_ptrLB[i+1];
			vaRGB[12] = src_ptrLB[i+4];
			vaRGB[13] = src_ptr[i-2];
			vaRGB[14] = src_ptr[i+4];
			vaRGB[15] = src_ptrLN[i-2];
			vaRGB[16] = src_ptrLN[i+1];
			vaRGB[17] = src_ptrLN[i+4];

			vaRGB[18] = src_ptr[i+2];
			vaRGB[19] = src_ptrLB[i-1];
			vaRGB[20] = src_ptrLB[i+2];
			vaRGB[21] = src_ptrLB[i+5];
			vaRGB[22] = src_ptr[i-1];
			vaRGB[23] = src_ptr[i+5];
			vaRGB[24] = src_ptrLN[i-1];
			vaRGB[25] = src_ptrLN[i+2];
			vaRGB[26] = src_ptrLN[i+5];

			intSum[0] = (vaRGB[1] + vaRGB[2] + vaRGB[3] + vaRGB[4] + vaRGB[5] + vaRGB[6] + vaRGB[7] + vaRGB[8]) / 8;
			intTmp2[0] = (code * intSum[0]) / 100;

			intSum[1] = (vaRGB[10] + vaRGB[11] + vaRGB[12] + vaRGB[13] + vaRGB[14] + vaRGB[15] + vaRGB[16] + vaRGB[17]) / 8;
			intTmp2[1] = (code * intSum[1]) / 100;

			intSum[2] = (vaRGB[19] + vaRGB[20] + vaRGB[21] + vaRGB[22] + vaRGB[23] + vaRGB[24] + vaRGB[25] + vaRGB[26]) / 8;
			intTmp2[2] = (code * intSum[2]) / 100;
			
			vAngle[0] = 0;
			n=0;
			while ((vAngle[0] == 0) && (n<9))
			{
				intTmp1[0] = abs (intSum[0] - vaRGB[n]) ;
				if (intTmp2[0] < intTmp1[0])
					vAngle[0] = n;
				n++;
			}
			vAngle[1] = 0;
			n=0;
			while ((vAngle[1] == 0) && (n<9))
			{
				intTmp1[1] = abs (intSum[1] - vaRGB[n+9]) ;
				if (intTmp2[1] < intTmp1[1])
					vAngle[1] = n;
				n++;
			}
			vAngle[2] = 0;
			n=0;
			while ((vAngle[2] == 0) && (n<9))
			{
				intTmp1[2] = abs (intSum[2] - vaRGB[n+18]) ;
				if (intTmp2[2] < intTmp1[2])
					vAngle[2] = n;
				n++;
			}
		
			if ( dst_cn == 1)
			{
				if (vAngle[0] > 0 && vAngle[1] > 0 && vAngle[2] > 0)
					dst_ptr[j] = unsigned char (0);
				else
					dst_ptr[j] = unsigned char (255);
			}
	
			else if ( dst_cn == 3 || dst_cn == 4)
			{		
				if (vAngle[0] > 0)
					dst_ptr[j] = unsigned char (0);
				else
					dst_ptr[j] = unsigned char (255);
		
				if (vAngle[1] > 0)
					dst_ptr[j+1] = unsigned char (0);
				else
					dst_ptr[j+1] = unsigned char (255);
		
				if (vAngle[2] > 0)
					dst_ptr[j+2] = unsigned char (0);
				else
					dst_ptr[j+2] = unsigned char (255);
			}

			j += dst_cn;

        }                                                               
	 }

return (dst_cn);
}
return (0);
}
else
	return (-1);
}

int Cainecc_rv1Dlg::aiSobel8UC3( CvArr* srcarr, CvArr* dstarr, int orderA, int orderB, int aperture_size)
{
	// This function is made following opencv style, ie, image is treated as matrix

	CvMat *dx = 0,  *dy = 0, *dxy = 0;
    void *buffer = 0;

    CvMat srcstub, *src = (CvMat*)srcarr;
    CvMat dststub, *dst = (CvMat*)dstarr;
    CvSize size;
    int flags = aperture_size;

    src = cvGetMat( src, &srcstub );
    dst = cvGetMat( dst, &dststub );

	if ( orderA == 0 && orderB == 0) return (-1);

 if( CV_MAT_TYPE( src->type ) == CV_8UC1 && CV_MAT_TYPE( dst->type ) == CV_8UC1 ) 
 {
	if( !CV_ARE_SIZES_EQ( src, dst ))
        return (-1);

    aperture_size &= INT_MAX;

    if( (aperture_size & 1) == 0 || aperture_size < 3 || aperture_size > 7 )
        return (-1);

    size = cvGetMatSize( src );

    dx = cvCreateMat( size.height, size.width, CV_16SC1 );
    dy = cvCreateMat( size.height, size.width, CV_16SC1 );
    dxy = cvCreateMat( size.height, size.width, CV_16SC1 );
    cvSobel( src, dx, orderA, orderB, aperture_size );
    cvSobel( src, dy, orderB, orderA, aperture_size );
	cvAdd(dx, dy, dxy, NULL);
	cvConvertScaleAbs(dxy, dst,1,0);

	if (dx !=0) cvReleaseMat( &dx );
    if (dy !=0) cvReleaseMat( &dy );
    if (dxy !=0) cvReleaseMat( &dxy );
 }

return (0);
}

int Cainecc_rv1Dlg::ainU8to32F(IplImage* srcarr, IplImage* dstarr, float cha4value)
{

    CvMat srcstub, *srcMat = (CvMat*)srcarr;
    CvMat dststub, *dstMat = (CvMat*)dstarr;
    CvSize size;
    int src_step, dst_step;
    int src_cn, dst_cn, src_depth, dst_depth; 

	bool bSizesEq = false;
	bool bDepthsEq = false;
	bool bTypeEq = false;

	unsigned char* src_ptr;
	float* dst_ptr;
    
    srcMat = cvGetMat( srcarr, &srcstub );
    dstMat = cvGetMat( dstarr, &dststub );
    
	if (!srcarr)
		return (-1);

    src_depth = CV_MAT_DEPTH(srcMat->type);
	dst_depth = CV_MAT_DEPTH(dstMat->type);
	src_cn = CV_MAT_CN(srcMat->type) ; // chanels
    dst_cn = CV_MAT_CN(dstMat->type) ; // chanels
    size = cvGetSize( srcMat );
    src_step = srcMat->step; 
    dst_step = dstMat->step;

if( src_depth == CV_8U  && dst_depth == CV_32F)
{
	 int i, j;  
 	 int tmpCol, tmpRow;
	 int srcLineSize4ptr, dstLineSize4ptr;

	 src_ptr = srcMat->data.ptr ;
	 dst_ptr = dstMat->data.fl ;

	 tmpRow = size.height;
	 tmpCol = size.width;
	 srcLineSize4ptr = tmpCol * src_cn;
	 dstLineSize4ptr = tmpCol * dst_cn;

// First posibility: number of chanels en source and destination are equals
if 	(src_cn == dst_cn)
{
	for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr  )      
        for( i = 0; i < src_step; i++)                  
        		dst_ptr[i] = float (float(src_ptr[i])/255);
	return (0);
}
// second condition number of chanels en source are 3 or more and destination are only 1
// In this case the average o the first 3 chanels is going to be put in the destination chanel.
else if ( src_cn >= 3 && dst_cn == 1 )
{
	 for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr  )      
	 { 
		 j = 0;
        for( i = 0; i < tmpCol; i ++)                  
        { 
		dst_ptr[i] = float(src_ptr[j] + src_ptr[j+1] + src_ptr[j+2]); 
		dst_ptr[i] = dst_ptr[i] / 765.0f;	// 765 = 255*3
		j += src_cn;
        }                                                               
    }
return (0);
}
// third condition: number of chanels en source is 1 and destination are 3 or more
// In this case the first 3 destinations chanels get a copy of the source chanel.
// the value for the 4th chanel is getting from the command
else if ( src_cn == 1 && dst_cn >= 3 )
{
	 for( ; tmpRow--; src_ptr += srcLineSize4ptr, dst_ptr += dstLineSize4ptr  )      
	 { 
		j = 0;
        for( i = 0; i < src_step; i ++)                  
        { 
		dst_ptr[j] = float(src_ptr[i])/255.0f;
		dst_ptr[j+1] = dst_ptr[j] ;
		dst_ptr[j+2] = dst_ptr[j] ;
		if (dst_cn == 4)
			dst_ptr[j+3] = cha4value ; 
		j += dst_cn;
        }                                                               
    } 
return (0);
}
else
	return (-1);
}
else
	return (-1);	
}

void Cainecc_rv1Dlg::OnAboutAinec()
{
	CAboutDlg dlgAbout;
	dlgAbout.DoModal();
}

void Cainecc_rv1Dlg::OnCbnSelchangeImgColor()
{
    m_CmbIMG_Color.GetLBText(m_CmbIMG_Color.GetCurSel(), strImgSRC_COLOR);
	UpdateData(false);
}



void Cainecc_rv1Dlg::OnCbnSelchangeAinecColor()
{
	m_cmbAINEC_Color.GetLBText(m_cmbAINEC_Color.GetCurSel(), strAINEC_Color);
	UpdateData(true);
}

void Cainecc_rv1Dlg::OnBnClickedImgSave()
{
	int p[3];
	char strFileSource[100], strFileEdge[100], strFileColor[100];
	char strTime[26];
	time_t ltime;
	int result1, result2, result3;


	p[0] = CV_IMWRITE_JPEG_QUALITY;
    p[1] = 100;
    p[2] = 0;
	
	time( &ltime );
	_itoa_s(int(ltime),strTime,10);

	strcpy_s( strFileSource, "SOURCE" );
	strcat_s( strFileSource, strTime );
	strcat_s( strFileSource, ".JPG" );

	strcpy_s( strFileColor, "COLOR" );
	strcat_s( strFileColor, strTime );
	strcat_s( strFileColor, ".JPG" );

	strcpy_s( strFileEdge, "EDGE" );
	strcat_s( strFileEdge, strTime );
	strcat_s( strFileEdge, ".JPG" );

	if (FrameRE != 0)
		result1 = cvSaveImage(strFileSource, FrameRE, p );
	if (AINECC_aux != 0)
		result2 = cvSaveImage(strFileColor, AINECC_aux, p );
	if (Edge_aux != 0)
		result3 = cvSaveImage(strFileEdge, Edge_aux, p );
	if (result1 > 0 && result2 > 0 && result3 > 0)
		MessageBox(_T("Saved in same directory that applicaion"));
	else
		MessageBox(_T("ERROR, Not saved"));


}
void Cainecc_rv1Dlg::OnEnKillfocusEdgeCoef()
{
	UpdateData(true);
}

void Cainecc_rv1Dlg::OnEnKillfocusEdgeSobelx()
{
	UpdateData(true);
}

void Cainecc_rv1Dlg::OnEnKillfocusEdgeSobely()
{
	UpdateData(true);
}
