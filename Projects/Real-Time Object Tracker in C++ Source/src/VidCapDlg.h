// VidCapDlg.h : header file
//

#pragma once
#include "afxwin.h"

#include "lib\motiondetector.h"
#include "lib\imageblobs.h"

// CVidCapDlg dialog
class CVidCapDlg : public CDialog
{
// Construction
public:
        CVidCapDlg(CWnd* pParent = NULL);	// standard constructor        

// Dialog Data
        enum { IDD = IDD_VIDCAP_DIALOG };

protected:
        virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
        HICON m_hIcon;

        // Generated message map functions
        virtual BOOL OnInitDialog();
        afx_msg void OnPaint();
        afx_msg HCURSOR OnQueryDragIcon();
        DECLARE_MESSAGE_MAP()

private:
        enum CaptureState {HALT, GET_BACKGROUND, BLOB_TRACKER} m_CaptureState;
        unsigned int* m_Background;
        unsigned int m_BackgroundFramesNumber;

        float m_Zoom;
        unsigned int m_MinBlobElements;
        MotionDetector m_MotionDetector;
        ImageBlobs m_Blobs;

        unsigned int m_Width;
        unsigned int m_Height;
        unsigned int m_Channels;

        UINT_PTR m_nTimer;
        double m_FpsRate;
        unsigned int m_MsPerFrame;
        unsigned int m_TotalFrames;
        unsigned int m_FramesProcessed;
        unsigned int m_Ms;

        CStatic m_PrvStatic;
        CStatic m_CapImgStatic;
        CComboBox m_AdapterCombo;
        UINT m_TimerInterval;
        CButton m_RunButton;
        CStatic m_VideoFormat;                
        CString m_TrackingStatus;
        bool m_TakeSnapshot;
        

        CLSID pBmpEncoder;
        int GetEncoderClsid(const WCHAR* format, CLSID* pClsid);
        
        void DrawData(unsigned char *pData);        


        afx_msg LRESULT OnGraphMessage(WPARAM wParam, LPARAM lParam);
        afx_msg void OnBnClickedEnumadaptorsButton();
        afx_msg void OnBnClickedRunButton();
        afx_msg void OnTimer(UINT_PTR nIDEvent);
        afx_msg void OnClose();
        afx_msg void OnWindowPosChanged(WINDOWPOS* lpwndpos);
        afx_msg void OnBnClickedFdetectCheck();       
        afx_msg void OnStnDblclickCapimgStatic();
        afx_msg void OnBnClickedBackgroundRadio();
        afx_msg void OnBnClickedTrackobjectsRadio();

};
