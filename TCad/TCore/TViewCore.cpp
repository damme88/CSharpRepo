// TViewCore.cpp : implementation file
//

#include "stdafx.h"
#include "TViewCore.h"


// TViewCore

TViewCore::TViewCore()
{
    m_hDC = NULL;
    m_hRC = NULL;

    red_ = 0.0;
    green_ = 0.0;
    blue_ = 0.0;

    cx_ = 0;
    cy_ = 0;
    rendering_rate_ = 0.5f;
    is_ldown_ = false;
    is_rdown_ = false;
}

TViewCore::~TViewCore()
{

}

void TViewCore::SetColor(int red, int green, int blue)
{
    red_ = red / 255.0;
    green_ = green / 255.0;
    blue_ = blue / 255.0;
}

BOOL TViewCore::InitializeOpenGL(HWND hwnd)
{
    BOOL  bRet = TRUE;
    if (hwnd)
    {
        m_hDC = GetDC(hwnd);
        if (m_hDC)
        {
            UINT PixelFormat;
            BYTE iAlphaBits = 0;
            BYTE iColorBits = 32;
            BYTE iDepthBits = 16;
            BYTE iAccumBits = 0;
            BYTE iStencilBits = 0;

            static PIXELFORMATDESCRIPTOR pfd =
            {
                sizeof(PIXELFORMATDESCRIPTOR),	
                1,								
                PFD_DRAW_TO_WINDOW |			//flags
                PFD_SUPPORT_OPENGL |
                PFD_DOUBLEBUFFER,
                PFD_TYPE_RGBA,					//pixel type
                iColorBits,
                0, 0, 0, 0, 0, 0,				//color bits ignored
                iAlphaBits,
                0,								//alpha shift ignored
                iAccumBits,						//accum buffer
                0, 0, 0, 0,						//accum bits ignored
                iDepthBits,						//depth buffer
                iStencilBits,					//stencil buffer
                0,								//aux buffer
                PFD_MAIN_PLANE,					//layer type
                0,								//reserved
                0, 0, 0							//masks ignored
            };

            PixelFormat = ChoosePixelFormat(m_hDC, &pfd);
            if (!PixelFormat)
            {
                bRet = FALSE;
                return bRet;
            }

            if (!SetPixelFormat(m_hDC, PixelFormat, &pfd))
            {
                bRet = FALSE;
                return bRet;
            }

            m_hRC = wglCreateContext(m_hDC);
            if (!m_hRC)
            {
                bRet = FALSE;
                return bRet;
            }

            if (!wglMakeCurrent(m_hDC, m_hRC))
            {
                bRet = FALSE;
                return bRet;
            }

            glEnable(GL_DEPTH_TEST);
            glDisable(GL_TEXTURE_2D);
        }
    }

    return bRet;
}

void TViewCore::Renderscene()
{
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
    glClearColor(red_, green_, blue_, 0.0f);

    glLoadIdentity();
    p_cameral_.ViewDirection();

    glTranslatef(0.0f, 0.0f, 0.0f);
    MakeAxis();
    //glutWireCube(10.0);
    glFlush();
    ::glFinish();
    ::SwapBuffers(m_hDC);
}


void TViewCore::OnSize(const INT& nWidth, const INT& nHeight)
{
    if (m_hDC == NULL || m_hRC == NULL)
        return;

    int iWidth  = nWidth;
    int iHeight = nHeight;

    if (iWidth == 0 || iHeight == 0)
        return;

    cx_ = iWidth;
    cy_ = iHeight;

    wglMakeCurrent(m_hDC, m_hRC);
    glViewport(0, 0, iWidth, iHeight);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    SetViewFrustum();
    glMatrixMode(GL_MODELVIEW);
}

void TViewCore::SetViewFrustum()
{
    double left_ = -(double)cx_ *0.5 / rendering_rate_;
    double right_ = (double)cx_ *0.5 / rendering_rate_;
    double top_ = (double)cy_ *0.5 / rendering_rate_;
    double bottom_ = -(double)cy_ *0.5 / rendering_rate_;

    double zfar = 20000 / rendering_rate_;
    zfar = max(20000, rendering_rate_);
    glOrtho(left_, right_, bottom_, top_, -zfar, zfar);
}

void TViewCore::OnMouseMove(UINT nFlags, CPoint point)
{
    if (is_rdown_ == true)
    {
        p_cameral_.CalAngle(point, cx_, cy_);
    }

    if (is_ldown_)
    {
        ms_down_pt_ = point;
        p_cameral_.SetDownPt(ms_down_pt_);
    }
}

void TViewCore::OnRButtonUp(UINT nFlags, CPoint point)
{
    ms_down_pt_ = CPoint(0, 0);
    p_cameral_.SetDownPt(ms_down_pt_);
    is_rdown_ = false;
}

void TViewCore::OnRButtonDown(UINT nFlags, CPoint point)
{
    ms_down_pt_ = point;
    p_cameral_.SetDownPt(ms_down_pt_);
    is_rdown_ = true;
}

void TViewCore::MakeAxis()
{
    glLineWidth(1.0f);
    glBegin(GL_LINES);
    glColor3f(1.0f, 0.0f, 0.0f);
    glVertex3f(-1.0*VALUE_AXIS, 0.0f, 0.0f);
    glVertex3f(VALUE_AXIS, 0.0f, 0.0f);

    glColor3f(0.0f, 1.0f, 0.0f);
    glVertex3f(0.0f, VALUE_AXIS, 0.0f);
    glVertex3f(0.0f, -1.0*VALUE_AXIS, 0.0f);

    glColor3f(0.0f, 0.0f, 1.0f);
    glVertex3f(0.0f, 0.0f, VALUE_AXIS);
    glVertex3f(0.0f, 0.0f, -1 * VALUE_AXIS);
    glEnd();
}
