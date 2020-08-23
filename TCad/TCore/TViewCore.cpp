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
    if (m_hDC == NULL || m_hRC == NULL)
        return;

    wglMakeCurrent(m_hDC, m_hRC);
    glClearColor(red_, green_, blue_, 0.0f);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glLoadIdentity();

    glTranslatef(0.0f, 0.0f, -2.6f);
    glBegin(GL_TRIANGLES);
    glColor3f(1.0f, 0.0f, 0.0f);	glVertex3f(-1.0f, -1.0f, 0.0f);
    glColor3f(0.0f, 1.0f, 0.0f);	glVertex3f(0.0f, 1.0f, 0.0f);
    glColor3f(0.0f, 0.0f, 1.0f);	glVertex3f(1.0f, -1.0f, 0.0f);
    glEnd();

    glFlush();
    SwapBuffers(m_hDC);
}


void TViewCore::OnSize(const INT& nWidth, const INT& nHeight)
{
    if (m_hDC == NULL || m_hRC == NULL)
        return;

    int iWidth  = nWidth;
    int iHeight = nHeight;

    if (iWidth == 0 || iHeight == 0)
        return;

    wglMakeCurrent(m_hDC, m_hRC);
    glViewport(0, 0, iWidth, iHeight);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    //glOrtho(-1.0, 1.0, -1.0, 1.0, 1.0, 100.0);
    gluPerspective( 67.5, ((double)(iWidth) / (double)(iHeight)), 1.0, 500.0);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
}
