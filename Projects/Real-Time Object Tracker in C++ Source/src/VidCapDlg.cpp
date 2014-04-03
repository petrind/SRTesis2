// VidCapDlg.cpp : implementation file
//

#include "stdafx.h"
#include "VidCap.h"
#include "VidCapDlg.h"

#include "capture.h"
#include "samplegrab.h"

#include "lib\vec2D.h"
#include "lib\vec2Dc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


#pragma


LARGE_INTEGER m_nFreq;
LARGE_INTEGER m_nBeginTime;

void tic()
{
        QueryPerformanceFrequency(&m_nFreq);
        QueryPerformanceCounter(&m_nBeginTime);
}
__int64 toc()
{
        LARGE_INTEGER nEndTime;
        __int64 nCalcTime;

        QueryPerformanceCounter(&nEndTime);
        nCalcTime = (nEndTime.QuadPart - m_nBeginTime.QuadPart) * 1000 / m_nFreq.QuadPart;

        return nCalcTime;
}


// CVidCapDlg dialog
CVidCapDlg::CVidCapDlg(CWnd* pParent /*=NULL*/)
                : CDialog(CVidCapDlg::IDD, pParent)
                , m_Width(640), m_Height(480), m_Channels(3)
                , m_Zoom(0.125f)
                , m_nTimer(0), m_TimerInterval(1000)                                
                , m_TakeSnapshot(false)
                , pBmpEncoder(GUID_NULL)
                , m_CaptureState(HALT)
                , m_Background(0)
                , m_BackgroundFramesNumber(0)                
                , m_MinBlobElements(10)
                , m_TrackingStatus(_T(""))
{        
        m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CVidCapDlg::DoDataExchange(CDataExchange* pDX)
{
        CDialog::DoDataExchange(pDX);
        DDX_Control(pDX, IDC_PRV_STATIC, m_PrvStatic);
        DDX_Control(pDX, IDC_ADAPTORS_COMBO, m_AdapterCombo);
        DDX_Text(pDX, IDC_SAMPLEINTERVAL_EDIT, m_TimerInterval);
        DDV_MinMaxUInt(pDX, m_TimerInterval, 10, 10000);
        DDX_Control(pDX, IDC_RUN_BUTTON, m_RunButton);
        DDX_Control(pDX, IDC_CAPIMG_STATIC, m_CapImgStatic);
        DDX_Control(pDX, IDC_VIDINFO_STATIC, m_VideoFormat);                        
        DDX_Text(pDX, IDC_STATUS_STATIC, m_TrackingStatus);
}

BEGIN_MESSAGE_MAP(CVidCapDlg, CDialog)
        ON_MESSAGE(WM_GRAPHNOTIFY, OnGraphMessage)
        ON_WM_PAINT()
        ON_WM_QUERYDRAGICON()
        //}}AFX_MSG_MAP
        ON_BN_CLICKED(IDC_ENUMADAPTORS_BUTTON, &CVidCapDlg::OnBnClickedEnumadaptorsButton)
        ON_BN_CLICKED(IDC_RUN_BUTTON, &CVidCapDlg::OnBnClickedRunButton)
        ON_WM_TIMER()
        ON_WM_CLOSE()
        ON_WM_WINDOWPOSCHANGED()        
        ON_STN_DBLCLK(IDC_CAPIMG_STATIC, &CVidCapDlg::OnStnDblclickCapimgStatic)
        ON_BN_CLICKED(IDC_BACKGROUND_RADIO, &CVidCapDlg::OnBnClickedBackgroundRadio)
        ON_BN_CLICKED(IDC_TRACKOBJECTS_RADIO, &CVidCapDlg::OnBnClickedTrackobjectsRadio)
END_MESSAGE_MAP()


// CVidCapDlg message handlers
LRESULT CVidCapDlg::OnGraphMessage(WPARAM wParam, LPARAM lParam)
{
        HRESULT hr = vcHandleGraphEvent();
        TRACE(L" WM_GRAPH 0x%X\n", hr);
        return 0;
}

BOOL CVidCapDlg::OnInitDialog()
{
        CDialog::OnInitDialog();

        // Set the icon for this dialog.  The framework does this automatically
        //  when the application's main window is not a dialog
        SetIcon(m_hIcon, TRUE);			// Set big icon
        SetIcon(m_hIcon, FALSE);		// Set small icon

        // TODO: Add extra initialization here

        // Initialize COM
        if (FAILED(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))) {
                MessageBox(L"CoInitialize Failed!", L"COM error");
                m_RunButton.EnableWindow(FALSE);
                return TRUE;
        }

        if (GetEncoderClsid(L"image/jpeg", &pBmpEncoder) < 0) {
                MessageBox(L"Failed to get image/jpeg encoder", L"warning");
        }


        return TRUE;  // return TRUE  unless you set the focus to a control
}

void CVidCapDlg::OnClose()
{
        // TODO: Add your message handler code here and/or call default
        KillTimer(m_nTimer);
        vcStopCaptureVideo();
        CoUninitialize();

        if (m_Background != 0)
                delete[] m_Background;

        CDialog::OnClose();
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CVidCapDlg::OnPaint()
{
        if (IsIconic()) {
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
        } else {
                CDialog::OnPaint();
        }
}


int CVidCapDlg::GetEncoderClsid(const WCHAR* format, CLSID* pClsid)
{
        UINT  num = 0;          // number of image encoders
        UINT  size = 0;         // size of the image encoder array in bytes

        Gdiplus::ImageCodecInfo* pImageCodecInfo = NULL;

        Gdiplus::GetImageEncodersSize(&num, &size);
        if (size == 0)
                return -1;  // Failure

        pImageCodecInfo = (Gdiplus::ImageCodecInfo*)(malloc(size));
        if (pImageCodecInfo == NULL)
                return -1;  // Failure

        Gdiplus::GetImageEncoders(num, size, pImageCodecInfo);

        for (UINT j = 0; j < num; ++j) {
                if (wcscmp(pImageCodecInfo[j].MimeType, format) == 0) {
                        *pClsid = pImageCodecInfo[j].Clsid;
                        free(pImageCodecInfo);
                        return j;  // Success
                }
        }

        free(pImageCodecInfo);
        return -1;  // Failure
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CVidCapDlg::OnQueryDragIcon()
{
        return static_cast<HCURSOR>(m_hIcon);
}


void CVidCapDlg::OnBnClickedEnumadaptorsButton()
{
        //Enum adaptors
        vcGetCaptureDevices(m_AdapterCombo);
}

void CVidCapDlg::OnBnClickedRunButton()
{
        UpdateData();

        HRESULT hr;
        if (m_nTimer == 0) {
                //Run capture
                hr = vcCaptureVideo(m_hWnd, m_PrvStatic.m_hWnd, m_AdapterCombo.GetCurSel() + 1);
                if (hr != S_OK) {
                        vcStopCaptureVideo();
                        return;
                }
                
                CString str;
                str.Format(L"Video output: %dx%d %dbpp", sgGetDataWidth(), sgGetDataHeight(), 8 * sgGetDataChannels()); 
                m_VideoFormat.SetWindowTextW(str);

                //Setup Timer
                if (sgGetDataWidth() == m_Width && sgGetDataHeight() == m_Height && sgGetDataChannels() == m_Channels) {
                        m_nTimer = SetTimer(1, m_TimerInterval, 0);
                        m_FpsRate = 0.0;
                        m_Ms = 0;
                        m_MsPerFrame = 0;
                        m_FramesProcessed = 0;
                        m_TotalFrames = 1000 / m_TimerInterval;
                        if (m_TotalFrames == 0)
                                m_TotalFrames = 1;
                }

                m_RunButton.SetWindowTextW(L"Stop");
                
                m_Background = new unsigned int[sgGetDataWidth() * sgGetDataHeight() * sgGetDataChannels()];
                memset(m_Background, 0, sgGetDataWidth() * sgGetDataHeight() * sgGetDataChannels() * sizeof(unsigned int));
                m_BackgroundFramesNumber = 0;
                m_CaptureState = HALT;

                m_MotionDetector.init(sgGetDataWidth(), sgGetDataHeight(), m_Zoom);
                m_Blobs.init((unsigned int)(m_Zoom * (float)sgGetDataWidth()), (unsigned int)(m_Zoom * (float)sgGetDataHeight()));
        } else {
                //Close Timer
                KillTimer(m_nTimer);
                m_nTimer = 0;
                m_RunButton.SetWindowTextW(L"Run");

                m_VideoFormat.SetWindowTextW(L"Video output");
                //Close Capture
                vcStopCaptureVideo();

                delete[] m_Background;
                m_Background = 0;
                m_CaptureState = HALT;
        }
}

void CVidCapDlg::OnTimer(UINT_PTR nIDEvent)
{
        // TODO: Add your message handler code here and/or call default
        SYSTEMTIME SystemTime;
        GetLocalTime(&SystemTime);
        TRACE(L" %d:%d:%d\n", SystemTime.wHour, SystemTime.wMinute, SystemTime.wSecond);

        unsigned char* pData = sgGrabData();
        if (pData != 0) 
                DrawData(pData);                

        CDialog::OnTimer(nIDEvent);
}


void CVidCapDlg::OnWindowPosChanged(WINDOWPOS* lpwndpos)
{
        CDialog::OnWindowPosChanged(lpwndpos);

        // TODO: Add your message handler code here
        vcChangePreviewState(!IsIconic());
}

void CVidCapDlg::DrawData(unsigned char *pData)
{                
        Gdiplus::Bitmap* pBitmap = sgGetBitmap();
        if (pBitmap == 0)
                return;        

        if (m_TakeSnapshot == true) {
                m_TakeSnapshot = false;
                sndPlaySound(L"snap.wav", SND_ASYNC);
                if (pBmpEncoder != GUID_NULL) {
                        wchar_t FileName[_MAX_PATH] = L"";
                        for (unsigned int i = 1; i < 0xFFFFFFFF; i++) {
                                swprintf_s(FileName, _MAX_PATH, L"Snapshot %04d.jpg", i);
                                FILE* fp = _wfopen(FileName, L"rb");
                                if (fp == 0) {
                                        pBitmap->Save(FileName, &pBmpEncoder);
                                        break;
                                }
                                else
                                        fclose(fp);

                        }                        
                }
        }

        RECT r;
        m_CapImgStatic.GetClientRect(&r);
        Gdiplus::Graphics g(m_CapImgStatic.GetDC()->m_hDC);
        

        //draw on Bitmap
        Gdiplus::Graphics mem_g(pBitmap);
        Gdiplus::SolidBrush GreenBrush(Gdiplus::Color(100, 0, 255, 0));
        Gdiplus::Pen RedPen(Gdiplus::Color(255, 255, 0, 0), 2.5f);

        switch (m_CaptureState) {
        default:
        case HALT:
                break;

        case GET_BACKGROUND:                
                m_BackgroundFramesNumber++;
                for (long i = 0; i < sgGetBufferSize(); i++)
                        m_Background[i] += (unsigned int)pData[i];
                break;

        case BLOB_TRACKER:
                tic();
                //mark motion regions
                const vec2Dc* mvec = m_MotionDetector.detect(pData);                
                m_Ms += (unsigned int)toc();                

                for (unsigned int y = 0; y < mvec->height(); y++) {
                        for (unsigned int x = 0; x < mvec->width(); x++) {
                                if ((*mvec)(y, x) == 1) 
                                        mem_g.FillEllipse(&GreenBrush, x * 8, y * 8, 8, 8);
                        }
                }

                tic();
                //estimate blobs                
                m_Blobs.find_blobs(*mvec, m_MinBlobElements);
                m_Ms += (unsigned int)toc();                

                if (m_Blobs.get_blobs_number() > 0) {
                        TRACE(L" found %d blobs\n", m_Blobs.get_blobs_number());
                        m_Blobs.find_bounding_boxes();
                        for (unsigned int i = 0; i < m_Blobs.get_blobs_number(); i++) {                        
                                const RECT r = m_Blobs.get_blob(i)->bounding_box;
                                INT x = int((float)r.left / m_Zoom);
                                INT y = int((float)r.top / m_Zoom);
                                INT width = int((float)r.right / m_Zoom) - x;
                                INT height = int((float)r.bottom / m_Zoom) - y;
                                mem_g.DrawRectangle(&RedPen, x, y, width, height);
                        }
                        m_Blobs.delete_blobs();
                }                

                m_FramesProcessed++;
                if (m_FramesProcessed >= m_TotalFrames) {                
                        m_MsPerFrame = m_Ms / m_TotalFrames;
                        m_FpsRate = 1000.0 / double(m_MsPerFrame);
                        m_FramesProcessed = 0;
                        m_Ms = 0;
                }

                m_TrackingStatus.Format(L"Object tracking: %dms (%.2lffps)", m_MsPerFrame, m_FpsRate);

                break;
        }                


        g.DrawImage(pBitmap, Gdiplus::Rect(0, 0, r.right, r.bottom));

        UpdateData(FALSE);
}

void CVidCapDlg::OnStnDblclickCapimgStatic()
{
        m_TakeSnapshot = true;
}

void CVidCapDlg::OnBnClickedBackgroundRadio()
{
        m_CaptureState = GET_BACKGROUND;

        m_BackgroundFramesNumber = 0;
        if (m_Background != 0)
                memset(m_Background, 0, sgGetDataWidth() * sgGetDataHeight() * sgGetDataChannels() * sizeof(unsigned int));
}

void CVidCapDlg::OnBnClickedTrackobjectsRadio()
{
        if (m_CaptureState == HALT)
                return;

        m_CaptureState = BLOB_TRACKER;

        if (m_Background != 0) {
                unsigned char* tmp = new unsigned char[sgGetDataWidth() * sgGetDataHeight() * sgGetDataChannels()];
                for (unsigned int i = 0; i < sgGetDataWidth() * sgGetDataHeight() * sgGetDataChannels(); i++)                 
                        tmp[i] = unsigned char(m_Background[i] / m_BackgroundFramesNumber);                
                
                m_MotionDetector.set_background(tmp);

                Gdiplus::Bitmap pBitmap(sgGetDataWidth(), sgGetDataHeight(), 
                                        sgGetDataChannels() * sgGetDataWidth(), PixelFormat24bppRGB, tmp);
                pBitmap.Save(L"background.jpg", &pBmpEncoder);

                delete[] tmp;
        }   
}
